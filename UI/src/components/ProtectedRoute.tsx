import type { ReactNode } from "react"
import { useEffect, useState } from "react"
import { getToken, isTokenValid, refreshAccessToken } from "../services/auth"
import { Navigate, Outlet, useLocation } from "react-router-dom"

export function ProtectedRoute({ children }: { children?: ReactNode }) {
  const location = useLocation()
  const [authStatus, setAuthStatus] = useState<'loading' | 'authenticated' | 'unauthenticated'>('loading')

  useEffect(() => {
    async function checkAuth() {
      const token = getToken()
      
      if (token && isTokenValid(token)) {
        // Token is valid, stay authenticated
        setAuthStatus('authenticated')
        return
      }

      // Token is invalid or missing, try to refresh it
      const refreshed = await refreshAccessToken()
      
      if (refreshed) {
        setAuthStatus('authenticated')
      } else {
        setAuthStatus('unauthenticated')
      }
    }

    checkAuth()
  }, [])

  if (authStatus === 'loading') {
    return null // or a loading spinner component
  }

  if (authStatus === 'unauthenticated') {
    return <Navigate to="/login" replace state={{ from: location }} />
  }

  return children ?? <Outlet />
}
