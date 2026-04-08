import { useState } from 'react'
import './ProductSearch.css'

export function ProductSearch({ onSearch, isLoading }) {
    const [query, setQuery] = useState('')
    const [searchType, setSearchType] = useState('name') // 'name' o 'barcode'

    const handleSubmit = (e) => {
        e.preventDefault()
        if (query.trim()) {
            onSearch(query.trim(), searchType === 'barcode')
        }
    }

    return (
        <form className="product-search" onSubmit={handleSubmit}>
            <div className="product-search__wrapper">
                <input
                    type="text"
                    className="product-search__input"
                    placeholder="Buscar producto..."
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    disabled={isLoading}
                />
                <div className="product-search__radios">
                    <label className="product-search__label">
                        <input
                            type="radio"
                            name="searchType"
                            value="name"
                            checked={searchType === 'name'}
                            onChange={(e) => setSearchType(e.target.value)}
                        />
                        Por Nombre
                    </label>
                    <label className="product-search__label">
                        <input
                            type="radio"
                            name="searchType"
                            value="barcode"
                            checked={searchType === 'barcode'}
                            onChange={(e) => setSearchType(e.target.value)}
                        />
                        Por Código de barras
                    </label>
                </div>
            </div>
            <button
                type="submit"
                className="product-search__button"
                disabled={isLoading || !query.trim()}
            >
                Buscar
            </button>
        </form>
    )
}
