
import { Navbar } from "@components/layout/Navbar"
import { Footer } from "@components/Footer"

export default function Home() {

    //console.log(user)

    return (
        <>
            <header>
                <Navbar />
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