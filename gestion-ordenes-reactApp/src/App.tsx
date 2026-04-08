import { lazy, Suspense } from 'react'
import { Routes, Route } from 'react-router-dom'
import ProtectedRoute from '@components/ProtectedRoute'
import { useLoader } from '@hooks/useLoader'
import { Loader } from '@components/Loader'
import { GlobalToast } from '@components/layout/GlobalToast'

const LazyHomePage = lazy(() => import('@pages/Home.jsx'))
const LazyAutentication = lazy(() => import('@pages/Autentication.jsx'))
const LazyGenerarOrdenes = lazy(() => import('@pages/salesUser/GenerarOrdenes.jsx'))
const LazyStadoOrdenes = lazy(() => import('@pages/salesUser/StadoOrdenes.jsx'))
const LazyRecepcionOrdenes = lazy(() => import('@pages/warehouseUser/RecepcionOrdenes.jsx'))
const LazyDespachoOrden = lazy(() => import('@pages/warehouseUser/DespachoOrden.jsx'))
const LazyRecepcionInventario = lazy(() => import('@pages/warehouseUser/RecepcionInventario.jsx'))

function App() {

  const { isLoading } = useLoader()

  return (
    <>
      <GlobalToast />
      <Suspense fallback={<Loader />}>
        {isLoading && <Loader />}
        <Routes>
          <Route path="/login" element={<LazyAutentication />} />
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <LazyHomePage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/ventas/generar-orden"
            element={
              <ProtectedRoute>
                <LazyGenerarOrdenes />
              </ProtectedRoute>
            }
          />
          <Route
            path="/ventas/ordenes"
            element={
              <ProtectedRoute>
                <LazyStadoOrdenes />
              </ProtectedRoute>
            }
          />
          <Route
            path="/bodega/ordenes"
            element={
              <ProtectedRoute>
                <LazyRecepcionOrdenes />
              </ProtectedRoute>
            }
          />
          <Route
            path="/bodega/ordenes/:id"
            element={
              <ProtectedRoute>
                <LazyDespachoOrden />
              </ProtectedRoute>
            }
          />
          <Route
            path="/bodega/inventario"
            element={
              <ProtectedRoute>
                <LazyRecepcionInventario />
              </ProtectedRoute>
            }
          />
        </Routes>
      </Suspense>
    </>
  )
}

export default App
