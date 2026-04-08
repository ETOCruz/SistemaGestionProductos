import './ProductList.css'
import { useContext } from 'react'
import { CartContext } from '@contexts/cartContext'

export function ProductList({ products }) {
    const { addToCart } = useContext(CartContext)

    if (!products || products.length === 0) {
        return (
            <div className="product-list__empty">
                No se encontraron productos para mostrar.
            </div>
        )
    }

    return (
        <div className="product-list">
            {products.map(product => (
                <div key={product.guid} className="product-card">
                    <div className="product-card__header">
                        <h3 className="product-card__title">{product.name}</h3>
                        <p className="product-card__barcode">Cód: {product.barcode}</p>
                    </div>
                    
                    <p className="product-card__price">
                        ${product.price ? product.price.toFixed(2) : '0.00'}
                    </p>
                    
                    <button 
                        className="product-card__btn"
                        onClick={() => addToCart(product)}
                    >
                        Agregar a Orden
                    </button>
                </div>
            ))}
        </div>
    )
}
