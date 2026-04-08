import { lazy, Suspense } from 'react'
import { Routes, Route } from 'react-router-dom'
import ProtectedRoute from '@components/ProtectedRoute'
import { useLoader } from '@hooks/useLoader'
import { Loader } from '@components/Loader'

const LazyHomePage = lazy(() => import('@pages/Home.jsx'))
const LazyAutentication = lazy(() => import('@pages/Autentication.jsx'))

function App() {

  const { isLoading } = useLoader()

  return (
    <>
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
        </Routes>
      </Suspense>
    </>
  )
}

export default App
