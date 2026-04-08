import { useState, useContext } from 'react'
import './OrderSummary.css'
import { CartContext } from '@contexts/cartContext'
import { useAuth } from '@hooks/useAuth'
import { validateStock, createOrder } from '@services/orders.service'
import { useLoader } from '@hooks/useLoader'

export function OrderSummary() {
    const { cart, removeFromCart, updateQuantity, clearCart } = useContext(CartContext)
    const { user } = useAuth()
    const { showLoader, hideLoader } = useLoader()

    const [stockWarnings, setStockWarnings] = useState([])
    const [successMessage, setSuccessMessage] = useState('')
    const [error, setError] = useState('')

    const total = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0)

    const handleValidateStock = async () => {
        if (cart.length === 0) return

        setError('')
        setStockWarnings([])
        setSuccessMessage('')
        showLoader()

        try {
            const items = cart.map(item => ({ productId: item.id, quantity: item.quantity }))
            const response = await validateStock(items)

            if (response.isAvailable) {
                setSuccessMessage('¡Todos los productos tienen stock disponible!')
            } else {
                const shortages = response.details.filter(d => d.isShortage)
                setStockWarnings(shortages)
            }
        } catch (err) {
            setError(err.message || 'Error al validar el stock.')
        } finally {
            hideLoader()
        }
    }

    const handleCreateOrder = async () => {
        if (cart.length === 0) return

        setError('')
        setStockWarnings([])
        setSuccessMessage('')
        showLoader()

        try {
            const items = cart.map(item => ({ productId: item.id, quantity: item.quantity }))
            const response = await createOrder(user?.id, items)

            setSuccessMessage(`¡Orden creada exitosamente! Folio: ${response.folio || response.id}`)
            clearCart() // Vaciamos el carrito tras crear la orden
        } catch (err) {
            setError(err.message || 'Error al generar la orden.')
        } finally {
            hideLoader()
        }
    }

    if (cart.length === 0) {
        return (
            <div className="order-summary">
                <h2 className="order-summary__title">Resumen de la Orden</h2>
                <p style={{ color: '#6b7280', textAlign: 'center' }}>
                    Agrega productos para crear una orden.
                </p>
                {successMessage && <p style={{ color: '#059669', textAlign: 'center', marginTop: '1rem', fontWeight: 'bold' }}>{successMessage}</p>}
            </div>
        )
    }

    return (
        <div className="order-summary">
            <h2 className="order-summary__title">Resumen de la Orden</h2>

            <div className="order-items">
                {cart.map(item => (
                    <div key={item.id} className="order-item">
                        <div className="order-item__info">
                            <h4 className="order-item__name">{item.name}</h4>
                            <p className="order-item__price">${item.price ? item.price.toFixed(2) : '0.00'}</p>
                        </div>
                        <div className="order-item__actions">
                            <input
                                type="number"
                                min="1"
                                max="100"
                                title="Máximo 100 unidades por producto"
                                className="order-item__qty"
                                value={item.quantity}
                                onChange={(e) => {
                                    const valStr = e.target.value;
                                    if (valStr === '') {
                                        updateQuantity(item.id, '');
                                        return;
                                    }
                                    let val = parseInt(valStr, 10);
                                    if (!isNaN(val)) {
                                        if (val > 100) val = 100;
                                        updateQuantity(item.id, val);
                                    }
                                }}
                                onBlur={(e) => {
                                    const val = parseInt(e.target.value, 10);
                                    if (isNaN(val) || val < 1) {
                                        updateQuantity(item.id, 1);
                                    }
                                }}
                            />
                            <button
                                className="order-item__remove"
                                onClick={() => removeFromCart(item.id)}
                            >
                                Quitar
                            </button>
                        </div>
                    </div>
                ))}
            </div>

            <div className="order-summary__footer">
                <div className="order-summary__total">
                    <span>Total:</span>
                    <span>${total.toFixed(2)}</span>
                </div>

                {stockWarnings.length > 0 && (
                    <div className="order-summary__stock-warning">
                        <strong>Atención, stock insificiente:</strong>
                        <ul>
                            {stockWarnings.map(w => (
                                <li key={w.productId}>
                                    {w.productName} (Solicitado: {w.requestedQuantity}, Disp: {w.availableStock})
                                </li>
                            ))}
                        </ul>
                    </div>
                )}

                {error && <p style={{ color: '#ef4444', marginBottom: '1rem' }}>{error}</p>}
                {successMessage && <p style={{ color: '#059669', marginBottom: '1rem', fontWeight: '600', backgroundColor: '#ecfdf5', padding: '0.5rem', borderRadius: '8px', border: '1px solid #10b981' }}>✅ {successMessage}</p>}
                <div className="order-summary__buttons">
                    <button
                        className="order-summary__btn order-summary__btn--secondary"
                        onClick={handleValidateStock}
                    >
                        Validar Stock
                    </button>
                    <button
                        className="order-summary__btn order-summary__btn--primary"
                        onClick={handleCreateOrder}
                    >
                        Generar Orden
                    </button>
                </div>
            </div>
        </div>
    )
}
