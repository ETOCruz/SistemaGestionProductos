import { createContext, useState, useEffect } from 'react';

// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext();

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const storedUser = JSON.parse(window.localStorage.getItem('app_user'));
        if (storedUser) {
            setUser(storedUser)
            setIsAuthenticated(true)
        }
        setLoading(false)
    }, []);

    const login = (userData) => {
        setUser(userData)
        setIsAuthenticated(true)
        localStorage.setItem('app_user', JSON.stringify(userData))
    };

    const logout = () => {
        setUser(null)
        setIsAuthenticated(false)
        localStorage.removeItem('app_user')
    };

    return (
        <AuthContext.Provider value={{
            user,
            loading,
            isAuthenticated,
            login,
            logout
        }}>
            {!loading && children}
        </AuthContext.Provider>
    );
}


