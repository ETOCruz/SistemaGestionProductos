import { useState, useEffect } from 'react';
import './StadoOrdenes.css';

import { Navbar } from '@components/layout/Navbar';
import { Footer } from '@components/Footer';
import { Modal } from '@components/common/Modal';
import { getAllOrders } from '@services/orders.service';
import { useLoader } from '@hooks/useLoader';
import { STATUS_ORDER } from '../../constants';

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

export default function StadoOrdenes() {
    const [activeTab, setActiveTab] = useState(null);
    const [orders, setOrders] = useState([]);
    const [pagination, setPagination] = useState({ pageNumber: 1, pageSize: 12, totalPages: 1, totalCount: 0 });
    const { showLoader, hideLoader } = useLoader();
    const [error, setError] = useState('');

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedOrder, setSelectedOrder] = useState(null);

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
            setError('Hubo un error al cargar las órdenes.');
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

    const openOrderDetails = (order) => {
        setSelectedOrder(order);
        setIsModalOpen(true);
    };

    return (
        <>
            <header>
                <Navbar />
            </header>

            <main className="stado-ordenes">
                <div className="stado-ordenes__header">
                    <h1 className="stado-ordenes__title">Mis Órdenes</h1>
                    <p className="stado-ordenes__subtitle">Consulta y gestiona el estado de tus órdenes de servicio</p>
                </div>

                <div className="stado-ordenes__tabs">
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

                <div className="stado-ordenes__content">
                    {error && <p className="error-message">{error}</p>}

                    {!error && orders.length === 0 && (
                        <div className="empty-state">No se encontraron órdenes para este estatus.</div>
                    )}

                    {!error && orders.length > 0 && (
                        <div className="orders-grid">
                            {orders.map(order => (
                                <div key={order.id} className="order-card">
                                    <div className="order-card__header">
                                        <span className="order-folio">Folio: {order.folio || 'N/A'}</span>
                                        {getStatusBadge(order.statusId, order.status)}
                                    </div>
                                    <div className="order-card__body">
                                        <p className="order-date">Fecha: {new Date(order.createdAt).toLocaleDateString()}</p>
                                        <p className="order-items-count">Artículos: {order.details?.length || 0}</p>
                                    </div>
                                    <div className="order-card__footer">
                                        <button className="btn-details" onClick={() => openOrderDetails(order)}>
                                            Ver Detalles
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

            <Modal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                title={`Detalles de Orden ${selectedOrder?.folio || ''}`}
            >
                {selectedOrder && (
                    <div className="order-details-modal">
                        <div className="modal-info-row">
                            <strong>Estatus:</strong> {getStatusBadge(selectedOrder.statusId, selectedOrder.status)}
                        </div>
                        <div className="modal-info-row">
                            <strong>Fecha de Creación:</strong> {new Date(selectedOrder.createdAt).toLocaleString()}
                        </div>

                        <h4 className="modal-subtitle">Productos ({selectedOrder.details?.length || 0})</h4>
                        {selectedOrder.details && selectedOrder.details.length > 0 ? (
                            <table className="details-table">
                                <thead>
                                    <tr>
                                        <th>Producto</th>
                                        <th>Cant. Pedida</th>
                                        <th>Cant. Surtida</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {selectedOrder.details.map((detail, index) => (
                                        <tr key={index}>
                                            <td>{detail.productName}</td>
                                            <td className="text-center">{detail.quantityOrdered}</td>
                                            <td className="text-center">{detail.quantityScanned}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        ) : (
                            <p>No hay productos en esta orden.</p>
                        )}
                    </div>
                )}
            </Modal>
        </>
    );
}
