import { API_BASE_URL, fetchWithIntercept } from './apiClient';

export async function searchProducts(params) {
    try {
        const queryParams = new URLSearchParams();
        if (params.name) queryParams.append('name', params.name);
        if (params.categoryId) queryParams.append('categoryId', params.categoryId);
        if (params.subCategoryId) queryParams.append('subCategoryId', params.subCategoryId);
        if (params.pageNumber) queryParams.append('pageNumber', params.pageNumber);
        if (params.pageSize) queryParams.append('pageSize', params.pageSize);

        const response = await fetchWithIntercept(`${API_BASE_URL}/products/search?${queryParams.toString()}`);
        
        if (!response.ok) {
            throw new Error('Error al buscar productos');
        }
        
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function getProductByBarcode(barcode) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/products/barcode/${barcode}`);
        
        if (!response.ok) {
            if (response.status === 404) return null;
            throw new Error(`Error al buscar producto por código de barras: ${barcode}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function addProductStock(productId, quantityToAdd) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/products/${productId}/add-stock`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ quantityToAdd: parseInt(quantityToAdd, 10) })
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Error al actualizar el stock');
        }

        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}
