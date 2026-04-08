const API_URL = import.meta.env.VITE_APP_URL || 'http://localhost:5173';

const ERROR_MESSAGES = {
    USER_NOT_FOUND: 'Usuario o contraseña incorrectos',
    API_ERROR: 'Error al validar información del usuario, intente más tarde'
}

export async function validateUser(username, password) {

    try {
        const response = await fetch(`${API_URL}/users.json`)

        if (!response.ok) throw new Error(ERROR_MESSAGES.API_ERROR);

        const data = await response.json()

        const userFound = data?.find(user => user.username === username && user.password == password);

        if (!userFound) throw new Error(ERROR_MESSAGES.USER_NOT_FOUND);

        const { password: _, username: __, ...userClean } = userFound;

        return userClean
    } catch (error) {
        if (error.message === ERROR_MESSAGES.USER_NOT_FOUND) throw error;
        throw new Error(ERROR_MESSAGES.API_ERROR);
    }
}