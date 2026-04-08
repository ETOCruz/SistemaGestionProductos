import { API_BASE_URL, fetchWithIntercept } from './apiClient';

export async function validateStock(items) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/orders/validate-stock`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ items })
        });
        
        if (!response.ok) {
            throw new Error('Error al validar el stock de los productos');
        }
        
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function createOrder(sellerId, items) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/orders`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            // Formato de acuerdo al DTO CreateOrderDto
            body: JSON.stringify({ sellerId, items })
        });
        
        if (!response.ok) {
            throw new Error('Error al intentar generar la orden');
        }
        
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}
