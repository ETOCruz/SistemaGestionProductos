import { useState } from "react"
import { useNavigate } from "react-router-dom"
import "@pages/Autentication.css"

import { useLoader } from "@hooks/useLoader"
import { AvatarIcon } from "@icons/AvatarIcon"
import { useAuth } from "@hooks/useAuth"
import { validateUser } from "@services/users-autentication"

export default function Autentication() {

    const [error, setError] = useState('')
    const [showpassword, setShowpassword] = useState(false)
    const { login } = useAuth()
    const { showLoader, hideLoader } = useLoader()

    const navigate = useNavigate()

    const handleSubmit = async (e) => {
        e.preventDefault()
        setError('')
        showLoader()

        const formData = new FormData(e.target)
        const username = formData.get('username')
        const password = formData.get('password')

        try {
            const user = await validateUser(username, password)
            login(user)
            hideLoader()
            navigate('/')
        } catch (error) {
            setError(error.message)
            hideLoader()
        }
    }

    return (
        <>
            <div className="auth">
                <div className="auth_container">
                    <div className="auth_icon_wrapper">
                        <AvatarIcon />
                    </div>

                    <h1 className="auth_title">
                        Bienvenido de nuevo
                    </h1>
                    <p className="auth_description">
                        Ingresa tus credenciales para acceder autenticarte
                    </p>

                    <form onSubmit={handleSubmit}>
                        <div className="auth_form_group">
                            <label className="auth_label">
                                Usuario
                            </label>
                            <input
                                name="username"
                                type="username"
                                placeholder="Nickname"
                                className="auth_input"
                                required
                            />

                            <label className="auth_label">
                                Contraseña
                            </label>
                            <input
                                name="password"
                                type={showpassword ? 'text' : 'password'}
                                placeholder="••••"
                                className="auth_input"
                                required
                            />
                            <div className="auth_show_password">
                                <label className="auth_checkbox_label">
                                    <input
                                        type="checkbox"
                                        checked={showpassword}
                                        onChange={() => setShowpassword(!showpassword)}
                                    />
                                    Mostrar password
                                </label>
                            </div>
                        </div>

                        {error && <p style={{ color: '#ef4444', fontSize: '0.875rem', marginTop: '0.5rem', textAlign: 'center' }}>{error}</p>}

                        <button type="submit" className="auth_button">
                            Iniciar Sesión
                        </button>
                    </form>

                </div>
            </div>
        </>
    )
}
