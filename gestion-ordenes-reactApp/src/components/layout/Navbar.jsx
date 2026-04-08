import "@components/layout/Navbar.css"

import { AvatarIcon } from "@icons/AvatarIcon"
import { useAuth } from "@hooks/useAuth"

export const Navbar = () => {
    const { user, logout } = useAuth()

    return (
        <nav className="navbar">
            <div className="navbar_logo">
                {
                    user?.rol === 'sales-user' ? 'Ordenes de Servicio' : 'Gestión de Inventario'
                }
            </div>
            <div className="navbar_user">
                <span className="navbar_user_name">Hola, {user?.name || 'Usuario'}</span>
                <AvatarIcon />
                <button
                    onClick={logout}
                    style={{
                        marginLeft: '1rem',
                        padding: '0.25rem 0.5rem',
                        backgroundColor: '#ef4444',
                        color: 'white',
                        border: 'none',
                        borderRadius: '0.25rem',
                        cursor: 'pointer',
                        fontSize: '0.75rem'
                    }}
                >
                    Salir
                </button>
            </div>
        </nav>
    )
}
