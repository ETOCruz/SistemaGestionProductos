
import { Navbar } from "@components/layout/Navbar"
import { Footer } from "@components/Footer"

import { useAuth } from "@hooks/useAuth"

export default function Home() {
    const { user, logout } = useAuth()

    //console.log(user)

    return (
        <>
            <header>
                <Navbar name={user?.name} onLogout={logout} />
            </header>
            <main>
                <p>Home</p>
            </main>
            <footer>
                <Footer />
            </footer>
        </>
    )
}