import { useState } from 'react'
import './GenerarOrdenes.css'

import { Navbar } from '@components/layout/Navbar'
import { Footer } from '@components/Footer'
import { ProductSearch } from '@components/products/ProductSearch'
import { ProductList } from '@components/products/ProductList'
import { OrderSummary } from '@components/orders/OrderSummary'
import { CartProvider } from '@contexts/cartContext'

import { searchProducts, getProductByBarcode } from '@services/products.service'
import { useLoader } from '@hooks/useLoader'

export default function GenerarOrdenes() {

    const [searchResults, setSearchResults] = useState([])
    const { showLoader, hideLoader } = useLoader()
    const [searchError, setSearchError] = useState('')

    const handleSearch = async (query, isBarcode) => {
        showLoader()
        setSearchError('')
        setSearchResults([])

        try {
            if (isBarcode) {
                const product = await getProductByBarcode(query)
                if (product) {
                    setSearchResults([product])
                } else {
                    setSearchResults([])
                    setSearchError('Producto no encontrado con el código de barras especificado.')
                }
            } else {
                const response = await searchProducts({ name: query, pageNumber: 1, pageSize: 20 })
                // Asumiendo que el response es un array, de lo contrario ajusta a response.items
                const items = Array.isArray(response) ? response : (response.items || [])
                setSearchResults(items)
                if (items.length === 0) {
                    setSearchError('No se encontraron productos.')
                }
            }
        } catch (error) {
            setSearchError('Hubo un error al realizar la búsqueda.')
            console.error(error)
        } finally {
            hideLoader()
        }
    }

    return (
        <CartProvider>
            <header>
                <Navbar />
            </header>

            <main className="generar-ordenes">
                <div className="generar-ordenes__header">
                    <h1 className="generar-ordenes__title">Nueva Orden de Servicio</h1>
                    <p className="generar-ordenes__subtitle">Busca productos y agrégalos a la orden actual</p>
                </div>

                <div className="generar-ordenes__grid">
                    <div className="generar-ordenes__main-content">
                        <ProductSearch onSearch={handleSearch} />

                        {searchError && <p style={{ color: '#ef4444', marginBottom: '1rem' }}>{searchError}</p>}

                        <ProductList products={searchResults} />
                    </div>

                    <aside className="generar-ordenes__sidebar">
                        <OrderSummary />
                    </aside>
                </div>
            </main>

            <footer>
                <Footer />
            </footer>
        </CartProvider>
    )
}
