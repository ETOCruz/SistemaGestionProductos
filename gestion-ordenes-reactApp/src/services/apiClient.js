export const API_BASE_URL = import.meta.env.VITE_API_GESTION_ORDENES || 'https://localhost:7008/api/v1';

/**
 * Un wrapper de `fetch` para interceptar errores de red
 * de forma global y notificar al usuario.
 */
export async function fetchWithIntercept(url, options = {}) {
    try {
        const response = await fetch(url, options);
        return response;
    } catch (error) {

        if (error.name === 'TypeError' || error.message.includes('Failed to fetch')) {
            const errorMsg = '¡Ups! Algo no salió bien con la información enviada. Por favor, verifica tu conexión a internet e intenta nuevamente más tarde.';

            window.dispatchEvent(new CustomEvent('global-api-error', {
                detail: { message: errorMsg }
            }));
        }
        throw error;
    }
}
