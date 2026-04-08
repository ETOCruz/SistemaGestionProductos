import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Navbar } from '@components/layout/Navbar';
import { getOrderById, authorizeOrder, scanProduct, pauseOrder } from '@services/orders.service';
import { useLoader } from '@hooks/useLoader';
import { useAuth } from '@hooks/useAuth';
import { Modal } from '@components/common/Modal';
import './DespachoOrden.css';

// Reutilizamos la función de badges temporalmente
const getStatusBadge = (statusId, statusText) => {
    switch (statusId) {
        case 1: return <span className="status-badge status-cotizacion">{statusText || 'Cotización'}</span>;
        case 2: return <span className="status-badge status-autorizacion">{statusText || 'Autorización'}</span>;
        case 3: return <span className="status-badge status-surtido">{statusText || 'Surtido'}</span>;
        case 4: return <span className="status-badge status-pausa">{statusText || 'Pausa'}</span>;
        case 5: return <span className="status-badge status-finalizada">{statusText || 'Finalizada'}</span>;
        default: return <span className="status-badge">{statusText || statusId}</span>;
    }
};

export default function DespachoOrden() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { user } = useAuth();
    const { showLoader, hideLoader } = useLoader();

    const [order, setOrder] = useState(null);
    const [barcodeInput, setBarcodeInput] = useState('');
    const [actionError, setActionError] = useState('');
    const [actionSuccess, setActionSuccess] = useState('');

    // Modal Faltantes
    const [isMissingModalOpen, setIsMissingModalOpen] = useState(false);
    const [missingProductsList, setMissingProductsList] = useState([]);

    // Referencia para mantener el foco en la pistola
    const inputRef = useRef(null);

    const fetchOrderDetails = async () => {
        showLoader();
        try {
            const data = await getOrderById(id);
            setOrder(data);
        } catch (err) {
            setActionError('No se pudo cargar la información de la orden.');
        } finally {
            hideLoader();
        }
    };

    useEffect(() => {
        if (id) {
            fetchOrderDetails();
        }
    }, [id]);

    // Mantener siempre el foco en el input al escanear si estamos en modo surtido
    useEffect(() => {
        if ((order?.statusId === 2 || order?.statusId === 3) && inputRef.current) {
            inputRef.current.focus();
        }
    }, [order, actionError, actionSuccess]);

    const handleAuthorize = async () => {
        setActionError('');
        setActionSuccess('');
        showLoader();
        try {
            await authorizeOrder(id, user.id || '00000000-0000-0000-0000-000000000000');
            setActionSuccess('Orden autorizada exitosamente. Iniciando modo de surtido.');
            await fetchOrderDetails(); // Refrescar para ver el nuevo estatus
        } catch (err) {
            setActionError(err.message || 'Error al autorizar.');
            if (err.missingProducts && err.missingProducts.length > 0) {
                setMissingProductsList(err.missingProducts);
                setIsMissingModalOpen(true);
            }
        } finally {
            hideLoader();
        }
    };

    const handlePause = async () => {
        setActionError('');
        setActionSuccess('');
        showLoader();
        try {
            await pauseOrder(id, user.id || '00000000-0000-0000-0000-000000000000');
            setActionSuccess('Orden pausada temporalmente.');
            await fetchOrderDetails();
        } catch (err) {
            setActionError(err.message || 'Error al pausar.');
        } finally {
            hideLoader();
        }
    };

    const handleScanSubmit = async (e) => {
        e.preventDefault();
        if (!barcodeInput.trim()) return;

        setActionError('');
        setActionSuccess('');
        const code = barcodeInput.trim();
        setBarcodeInput(''); // Limpiar inmediatamente para la pistola lectora
        showLoader();

        try {
            // Mandamos quantity 1 por defecto al escanear
            await scanProduct(id, code, 1);
            setActionSuccess(`¡Escaneo exitoso!`);
            await fetchOrderDetails();
        } catch (err) {
            setActionError(err.message || `No se pudo registrar el escaneo del código: ${code}`);
        } finally {
            hideLoader();
            // Regresar el foco
            if (inputRef.current) inputRef.current.focus();
        }
    };

    const handleResume = async () => {
        // Importamos dinámico o usamos la func de orders.service.js
        // ¡Ojo! Necesitamos importar resumeOrder al inicio del archivo. (Revisar los imports arriba)
        import('@services/orders.service').then(async ({ resumeOrder }) => {
            setActionError('');
            setActionSuccess('');
            showLoader();
            try {
                await resumeOrder(id, user.id || '00000000-0000-0000-0000-000000000000');
                setActionSuccess('Orden reanudada. Puedes seguir surtiendo.');
                await fetchOrderDetails();
            } catch (err) {
                setActionError(err.message || 'Error al reanudar.');
            } finally {
                hideLoader();
            }
        });
    };

    const handleBack = () => {
        navigate('/bodega/ordenes');
    };

    if (!order) {
        return (
            <>
                <header><Navbar /></header>
                <main className="despacho-container"><p>Cargando orden...</p></main>
            </>
        );
    }

    return (
        <>
            <header>
                <Navbar />
            </header>

            <main className="despacho-container">
                <div className="despacho-header">
                    <div className="despacho-header__title-area">
                        <button className="btn-back" onClick={handleBack}>&larr; Volver</button>
                        <h2>Folio: {order.folio}</h2>
                        {getStatusBadge(order.statusId, order.status)}
                    </div>
                </div>

                {/* Notificaciones globales de la acción actual */}
                {actionError && <div className="action-alert action-alert--error">{actionError}</div>}
                {actionSuccess && <div className="action-alert action-alert--success">{actionSuccess}</div>}

                <div className="despacho-grid">
                    {/* Panel Izquierdo: Acciones */}
                    <div className="despacho-panel">
                        <h3>Acciones Operativas</h3>

                        {/* Estatus 1: Requiere Autorización */}
                        {order.statusId === 1 && (
                            <div className="action-box">
                                <p>Esta orden se encuentra en fase de cotización. Debe confirmar disponibilidad de inventario para aprobar su surtido.</p>
                                <button className="btn-massive btn-massive--primary" onClick={handleAuthorize}>
                                    VALIDAR Y AUTORIZAR SURTIDO
                                </button>
                            </div>
                        )}

                        {/* Estatus 2 y 3: Modo de Surtido Escaner Activo */}
                        {(order.statusId === 2 || order.statusId === 3) && (
                            <div className="action-box scanner-box">
                                <p className="scanner-instruction">Escanea el código de barras del producto (o captúralo manual)</p>
                                <form onSubmit={handleScanSubmit} className="scanner-form">
                                    <input
                                        ref={inputRef}
                                        type="text"
                                        className="scanner-input"
                                        placeholder="Esperando lectura de escáner..."
                                        value={barcodeInput}
                                        onChange={(e) => setBarcodeInput(e.target.value)}
                                        autoFocus
                                    />
                                    <button type="submit" className="btn-scan">Registrar Producto</button>
                                </form>
                                <button className="btn-pause-action" onClick={handlePause}>
                                    Falta Inventario - Enviar a Pausa temporal
                                </button>
                            </div>
                        )}

                        {/* Estatus 4: Pausada */}
                        {order.statusId === 4 && (
                            <div className="action-box">
                                <p className="text-warning">Esta orden se encuentra pausada. Revise el inventario y reanude el surtido.</p>
                                <button className="btn-massive btn-massive--success" onClick={handleResume}>
                                    REANUDAR SURTIDO
                                </button>
                            </div>
                        )}

                        {/* Estatus 5: Finalizada */}
                        {order.statusId === 5 && (
                            <div className="action-box">
                                <p className="text-success">✅ Esta orden ya ha sido procesada y surtida en su totalidad.</p>
                            </div>
                        )}
                    </div>

                    {/* Panel Derecho: Progreso Surtido */}
                    <div className="despacho-panel details-panel">
                        <h3>Progreso del Pedido</h3>

                        <div className="progress-summary">
                            <div className="progress-stat">
                                <span className="stat-label">Total Productos Diferentes</span>
                                <span className="stat-value">{order.details?.length || 0}</span>
                            </div>
                        </div>

                        <div className="products-checklist">
                            {order.details?.map(detail => {
                                const isComplete = detail.quantityScanned >= detail.quantityOrdered;

                                return (
                                    <div key={detail.guid} className={`checklist-item ${isComplete ? 'checklist-item--done' : ''}`}>
                                        <div className="item-info">
                                            <h4>{detail.productName}</h4>
                                            <span className="code">Código/ID: {detail.productId}</span>
                                        </div>
                                        <div className="item-progress">
                                            <div className="progress-pill">
                                                <span className="scanned">{detail.quantityScanned}</span>
                                                <span className="separator">/</span>
                                                <span className="ordered">{detail.quantityOrdered}</span>
                                            </div>
                                            {isComplete && <span className="check-icon">✓</span>}
                                        </div>
                                    </div>
                                );
                            })}
                        </div>
                    </div>
                </div>
            </main>

            <Modal
                isOpen={isMissingModalOpen}
                onClose={() => setIsMissingModalOpen(false)}
                title="Inventario Insuficiente"
            >
                <div className="missing-products-container">
                    <p className="missing-alert-text">
                        No se pudo autorizar la orden debido a que los siguientes productos no cuentan con stock suficiente en ninguna bodega.
                    </p>
                    <table className="missing-table">
                        <thead>
                            <tr>
                                <th>Producto</th>
                                <th>Solicitado</th>
                                <th>Disponible</th>
                                <th>Faltante</th>
                            </tr>
                        </thead>
                        <tbody>
                            {missingProductsList.map((p) => (
                                <tr key={p.productId}>
                                    <td>{p.productName}</td>
                                    <td className="text-center">{p.requestedQuantity}</td>
                                    <td className="text-center">{p.availableQuantity || 0}</td>
                                    <td className="text-center shortage-text">
                                        {p.requestedQuantity - (p.availableQuantity || 0)}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <div style={{ display: 'flex', justifyContent: 'flex-end', marginTop: '1.5rem' }}>
                        <button
                            className="btn-scan"
                            style={{ backgroundColor: '#4f46e5' }}
                            onClick={() => setIsMissingModalOpen(false)}
                        >
                            Entendido
                        </button>
                    </div>
                </div>
            </Modal>
        </>
    );
}
