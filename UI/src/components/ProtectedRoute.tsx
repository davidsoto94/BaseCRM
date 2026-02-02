import type { ReactNode } from "react"
import { getToken, isTokenValid } from "../services/auth"
import { Navigate, Outlet, useLocation } from "react-router-dom"



export function ProtectedRoute({ children }: { children?: ReactNode }) {
  const location = useLocation()
  const token = getToken()
  const hasValidToken = !!(token && isTokenValid(token))
  if (!hasValidToken) {
    return <Navigate to="/login" replace state={{ from: location }} />
  }
  return children ?? <Outlet />
}
