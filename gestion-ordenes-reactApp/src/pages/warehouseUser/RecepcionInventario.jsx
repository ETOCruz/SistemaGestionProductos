import { useState, useRef, useEffect } from 'react';
import { Navbar } from '@components/layout/Navbar';
import { getProductByBarcode, addProductStock } from '@services/products.service';
import { useLoader } from '@hooks/useLoader';
import { useAuth } from '@hooks/useAuth';
import './RecepcionInventario.css';

export default function RecepcionInventario() {
    const { showLoader, hideLoader } = useLoader();
    const { user } = useAuth();
    
    const [barcode, setBarcode] = useState('');
    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState(1);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    
    const barcodeRef = useRef(null);
    const quantityRef = useRef(null);

    // Foco inicial en el código de barras
    useEffect(() => {
        barcodeRef.current?.focus();
    }, []);

    const handleBarcodeSubmit = async (e) => {
        e.preventDefault();
        if (!barcode.trim()) return;

        setError('');
        setSuccess('');
        showLoader();
        
        try {
            const data = await getProductByBarcode(barcode.trim());
            if (data) {
                setProduct(data);
                // Mover foco a cantidad después de un breve delay para que el DOM se actualice
                setTimeout(() => quantityRef.current?.focus(), 100);
            } else {
                setError('Producto no encontrado. Verifique el código de barras.');
                setBarcode('');
                barcodeRef.current?.focus();
            }
        } catch (err) {
            setError('Error al buscar el producto.');
        } finally {
            hideLoader();
        }
    };

    const handleAddStock = async (e) => {
        e.preventDefault();
        if (!product || quantity <= 0) return;

        setError('');
        setSuccess('');
        showLoader();

        try {
            await addProductStock(product.id, quantity);
            setSuccess(`¡Éxito! Se añadieron ${quantity} unidades a "${product.name}".`);
            
            // Limpiar para el siguiente
            setProduct(null);
            setBarcode('');
            setQuantity(1);
            setTimeout(() => barcodeRef.current?.focus(), 100);
        } catch (err) {
            setError(err.message || 'No se pudo actualizar el inventario.');
        } finally {
            hideLoader();
        }
    };

    const handleCancel = () => {
        setProduct(null);
        setBarcode('');
        setQuantity(1);
        setError('');
        setSuccess('');
        setTimeout(() => barcodeRef.current?.focus(), 100);
    };

    return (
        <>
            <header><Navbar /></header>
            <main className="inventario-container">
                <div className="inventario-header">
                    <h1>Entradas de Inventario</h1>
                    <p>Escanea o busca productos para incrementar sus existencias en bodega.</p>
                </div>

                {error && <div className="action-alert action-alert--error">{error}</div>}
                {success && <div className="action-alert action-alert--success">{success}</div>}

                <div className="inventario-card">
                    {!product ? (
                        <div className="scanner-section">
                            <form onSubmit={handleBarcodeSubmit} className="scanner-form">
                                <label htmlFor="barcode">Código de Barras</label>
                                <input
                                    id="barcode"
                                    ref={barcodeRef}
                                    type="text"
                                    className="scanner-input"
                                    placeholder="Esperando lectura de escáner..."
                                    value={barcode}
                                    onChange={(e) => setBarcode(e.target.value)}
                                    autoComplete="off"
                                />
                                <button type="submit" className="btn-scan">Buscar Producto</button>
                            </form>
                        </div>
                    ) : (
                        <div className="product-entry-section">
                            <div className="product-info-badge">
                                <h3>{product.name}</h3>
                                <p className="product-barcode">Cód: {product.barcode}</p>
                                <p className="product-stock">Stock actual: <strong>{product.productQuantity || 0}</strong></p>
                            </div>

                            <form onSubmit={handleAddStock} className="entry-form">
                                <div className="form-group">
                                    <label htmlFor="quantity">Cantidad a Ingresar</label>
                                    <input
                                        id="quantity"
                                        ref={quantityRef}
                                        type="number"
                                        min="1"
                                        className="quantity-input"
                                        value={quantity}
                                        onChange={(e) => setQuantity(parseInt(e.target.value) || 0)}
                                    />
                                </div>
                                <div className="form-actions">
                                    <button type="button" className="btn-cancel" onClick={handleCancel}>
                                        Cancelar
                                    </button>
                                    <button type="submit" className="btn-massive btn-massive--primary">
                                        Registrar Entrada
                                    </button>
                                </div>
                            </form>
                        </div>
                    )}
                </div>
            </main>
        </>
    );
}
