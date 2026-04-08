import "@components/layout/Navbar.css"

import { AvatarIcon } from "@icons/AvatarIcon"

export const Navbar = ({ name, onLogout }) => {
    return (
        <nav className="navbar">
            <div className="navbar_logo">
                Fleeca Bank
            </div>
            <div className="navbar_user">
                <span className="navbar_user_name">Hola, {name || 'Usuario'}</span>
                <AvatarIcon />
                <button
                    onClick={onLogout}
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
