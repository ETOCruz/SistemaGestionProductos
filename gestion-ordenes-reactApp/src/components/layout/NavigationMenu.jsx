import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '@hooks/useAuth';
import './NavigationMenu.css';

export const NavigationMenu = () => {
    const { user } = useAuth();
    const location = useLocation();

    if (!user) return null;

    const navLinks = {
        'sales-user': [
            { path: '/', label: 'Inicio' },
            { path: '/ventas/generar-orden', label: 'Generar Orden' },
            { path: '/ventas/ordenes', label: 'Mis Órdenes' }
        ],
        'warehouse-manager': [
            { path: '/', label: 'Inicio' },
            { path: '/bodega/ordenes', label: 'Recepción de Órdenes' },
            { path: '/bodega/inventario', label: 'Inventario' }
        ]
    };

    // La API parece guardar el rol en `rol` o `role`
    const role = user.rol || user.role;
    const links = navLinks[role] || [];

    if (links.length === 0) return null;

    return (
        <div className="navigation-menu-wrapper">
            <div className="navigation-menu">
                {links.map((link) => {
                    const isActive = location.pathname === link.path;
                    return (
                        <Link
                            key={link.path}
                            to={link.path}
                            className={`nav-link ${isActive ? 'active' : ''}`}
                        >
                            {link.label}
                        </Link>
                    )
                })}
            </div>
        </div>
    );
};
