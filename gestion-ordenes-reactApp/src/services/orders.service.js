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

export async function getAllOrders({ status, pageNumber = 1, pageSize = 10 }) {
    try {
        const queryParams = new URLSearchParams();
        if (status !== undefined && status !== null) queryParams.append('status', status);
        queryParams.append('pageNumber', pageNumber);
        queryParams.append('pageSize', pageSize);

        const response = await fetchWithIntercept(`${API_BASE_URL}/orders?${queryParams.toString()}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });
        
        if (!response.ok) {
            throw new Error('Error al obtener la lista de órdenes');
        }
        
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function getOrderById(orderId) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/orders/${orderId}`, {
            method: 'GET',
            headers: { 'Accept': 'application/json' }
        });
        if (!response.ok) throw new Error('Error al obtener los detalles de la orden');
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function authorizeOrder(orderId, userId) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/orders/${orderId}/authorize?userId=${userId}`, {
            method: 'POST',
            headers: { 'Accept': 'application/json' }
        });
        
        let data;
        try {
            data = await response.json();
        } catch(e) {}
        
        if (!response.ok) {
            const err = new Error(data?.message || 'Error al intentar autorizar la orden por falta de stock o inventario.');
            err.missingProducts = data?.missingProducts || [];
            throw err;
        }
        return data;
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function scanProduct(orderId, barcode, quantity) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/orders/${orderId}/scan`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ barcode, quantity })
        });
        
        if (!response.ok) {
            let data;
            try { data = await response.json(); } catch(e) {}
            throw new Error(data?.error || 'Error al escanear el producto.');
        }
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function pauseOrder(orderId, userId) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/orders/${orderId}/pause?userId=${userId}`, {
            method: 'PUT',
            headers: { 'Accept': 'application/json' }
        });
        if (!response.ok) throw new Error('Error al pausar la orden');
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

export async function resumeOrder(orderId, userId) {
    try {
        const response = await fetchWithIntercept(`${API_BASE_URL}/orders/${orderId}/resume?userId=${userId}`, {
            method: 'PUT',
            headers: { 'Accept': 'application/json' }
        });
        if (!response.ok) throw new Error('Error al reanudar la orden');
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        throw error;
    }
}

