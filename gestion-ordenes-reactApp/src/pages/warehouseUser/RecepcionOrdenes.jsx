import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './RecepcionOrdenes.css';

import { Navbar } from '@components/layout/Navbar';
import { Footer } from '@components/Footer';
import { getAllOrders } from '@services/orders.service';
import { useLoader } from '@hooks/useLoader';
import { STATUS_ORDER } from '../../constants';

// Función helper para estatus (repetida aquí temporalmente para no extraerla de StadoOrdenes)
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

export default function RecepcionOrdenes() {
    const [activeTab, setActiveTab] = useState(null);
    const [orders, setOrders] = useState([]);
    const [pagination, setPagination] = useState({ pageNumber: 1, pageSize: 12, totalPages: 1, totalCount: 0 });
    const { showLoader, hideLoader } = useLoader();
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const fetchOrders = async (status, page) => {
        showLoader();
        setError('');
        try {
            const data = await getAllOrders({ status, pageNumber: page, pageSize: pagination.pageSize });
            setOrders(data.items || []);
            setPagination({
                pageNumber: data.pageNumber,
                pageSize: data.pageSize,
                totalPages: data.totalPages,
                totalCount: data.totalCount
            });
        } catch (err) {
            setError('Hubo un error al cargar las órdenes para recepción.');
            console.error(err);
        } finally {
            hideLoader();
        }
    };

    useEffect(() => {
        fetchOrders(activeTab, 1);
    }, [activeTab]);

    const handlePageChange = (newPage) => {
        if (newPage >= 1 && newPage <= pagination.totalPages) {
            fetchOrders(activeTab, newPage);
        }
    };

    const handleManageOrder = (orderId) => {
        navigate(`/bodega/ordenes/${orderId}`);
    };

    return (
        <>
            <header>
                <Navbar />
            </header>

            <main className="recepcion-ordenes">
                <div className="recepcion-ordenes__header">
                    <h1 className="recepcion-ordenes__title">Recepción de Órdenes</h1>
                    <p className="recepcion-ordenes__subtitle">Autoriza órdenes, surte mercancía y despacha pedidos.</p>
                </div>

                <div className="recepcion-ordenes__tabs">
                    {STATUS_ORDER.map(tab => (
                        <button
                            key={tab.label}
                            className={`tab-button ${activeTab === tab.id ? 'active' : ''}`}
                            onClick={() => setActiveTab(tab.id)}
                        >
                            {tab.label}
                        </button>
                    ))}
                </div>

                <div className="recepcion-ordenes__content">
                    {error && <p className="error-message">{error}</p>}

                    {!error && orders.length === 0 && (
                        <div className="empty-state">No hay órdenes en este estatus para procesar.</div>
                    )}

                    {!error && orders.length > 0 && (
                        <div className="orders-grid">
                            {orders.map(order => (
                                <div key={order.guid} className={`order-card ${order.statusId === 1 ? 'card-highlight' : ''}`}>
                                    <div className="order-card__header">
                                        <span className="order-folio">Folio: {order.folio || 'N/A'}</span>
                                        {getStatusBadge(order.statusId, order.status)}
                                    </div>
                                    <div className="order-card__body">
                                        <p className="order-date">Generada: {new Date(order.createdAt).toLocaleDateString()}</p>
                                        <p className="order-items-count">Artículos: {order.details?.length || 0}</p>
                                    </div>
                                    <div className="order-card__footer">
                                        <button className="btn-manage" onClick={() => handleManageOrder(order.id)}>
                                            {order.statusId === 5 ? 'Ver Resumen' : 'Gestionar Orden'}
                                        </button>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}

                    {!error && pagination.totalPages > 1 && (
                        <div className="pagination">
                            <button
                                disabled={pagination.pageNumber === 1}
                                onClick={() => handlePageChange(pagination.pageNumber - 1)}
                            >
                                Anterior
                            </button>
                            <span>Página {pagination.pageNumber} de {pagination.totalPages}</span>
                            <button
                                disabled={pagination.pageNumber === pagination.totalPages}
                                onClick={() => handlePageChange(pagination.pageNumber + 1)}
                            >
                                Siguiente
                            </button>
                        </div>
                    )}
                </div>
            </main>

            <footer>
                <Footer />
            </footer>
        </>
    );
}
