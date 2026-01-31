import type { ReactNode } from "react"
import { getToken } from "../services/auth"
import { Navigate } from "react-router-dom"

export function ProtectedRoute({ children }: { children: ReactNode }) {
  const token = getToken()
  if (!token) {
    return <Navigate to="/login" replace />
  }
  return children
}
