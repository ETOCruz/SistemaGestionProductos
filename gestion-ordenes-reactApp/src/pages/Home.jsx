
import { Navbar } from "@components/layout/Navbar"
import { Footer } from "@components/Footer"
import { useAuth } from "@hooks/useAuth"
import "./Home.css"

export default function Home() {
    const { user } = useAuth()

    return (
        <>
            <header>
                <Navbar />
            </header>
            <main className="home-container">
                <div className="welcome-banner">
                    <h1 className="welcome-title">¡Te damos la bienvenida, {user?.name || 'Compañero'}! 👋</h1>
                    <p className="welcome-subtitle">
                        Este es tu panel de control principal. Desde aquí puedes mantenerte informado sobre las últimas novedades 
                        y utilizar el menú superior para navegar a tus herramientas de trabajo.
                    </p>
                </div>
                
                <h2 style={{ fontSize: '1.5rem', fontWeight: '600', marginBottom: '1.5rem', color: '#1f2937' }}>Novedades Recientes</h2>
                
                <div className="news-grid">
                    <div className="news-card">
                        <h3>Lanzamiento del Nuevo Módulo de Órdenes 🚀</h3>
                        <p>Hemos actualizado el sistema para facilitar la visualización del estado de las órdenes. Ahora la barra superior es tu mejor aliada para navegar rápidamente.</p>
                        <span className="news-date">Hoy</span>
                    </div>
                    <div className="news-card">
                        <h3>Atajos Rápidos de Sistema ⚡</h3>
                        <p>Recuerda que si en tu rol cuentas con permisos de Vendedor, la pestaña "Mis Órdenes" está separada de "Generar" para que mantengas tu espacio limpio y ágil.</p>
                        <span className="news-date">Ayer</span>
                    </div>
                    {(user?.rol === 'warehouse-user' || user?.role === 'warehouse-user') && (
                        <div className="news-card">
                            <h3>Aviso sobre Inventarios 📦</h3>
                            <p>Te recordamos revisar la "Recepción de Órdenes" periódicamente. El surtido ahora deduce automáticamente las unidades para evitar quiebres de stock sin avisar.</p>
                            <span className="news-date">Aviso Importante</span>
                        </div>
                    )}
                </div>
            </main>
            <footer>
                <Footer />
            </footer>
        </>
    )
}