const TOKEN_KEY = 'id_token'

type LoginRequest = {
  email: string
  password: string
}

type LoginResponse = {
  token: string
  expiresIn?: number
}

export const apiBase = import.meta.env.VITE_API_BASE_URL || ''

export async function login(payload: LoginRequest) {
  const res = await fetch(`${apiBase}/api/v1/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json',
      'Accept-Language' : localStorage.getItem('lang') || 'en'
     },
    body: JSON.stringify(payload),
  })

  if (!res.ok) {
    const errText = await res.text()
    throw new Error(errText || 'Login failed')
  }

  const data: LoginResponse = await res.json()
  if (!data || !data.token) throw new Error('Invalid login response')
  sessionStorage.setItem(TOKEN_KEY, data.token)
  return data
}

export function logout() {
  sessionStorage.removeItem(TOKEN_KEY)
}

export function getToken(): string | null {
  return sessionStorage.getItem(TOKEN_KEY)
}

export function isAuthenticated(): boolean {
  return !!getToken()
}

export async function fetchWithAuth(input: RequestInfo | URL, init: RequestInit = {}) {
  const token = getToken()
  const headers = new Headers(init.headers ?? undefined)
  headers.set("Content-Language", localStorage.getItem('lang') || 'en')
  if (token && !headers.has("Authorization")) {
    headers.set("Authorization", `Bearer ${token}`)
  }
  return fetch(input, { ...init, headers })
}

export function isTokenValid(token: string) {
  try {
    const [, payload] = token.split(".")
    if (!payload) return false
    const decoded = JSON.parse(atob(payload.replace(/-/g, "+").replace(/_/g, "/")))
    if (!decoded?.exp) return true
    const now = Math.floor(Date.now() / 1000)
    return decoded.exp > now
  } catch {
    return false
  }
}

const auth = {
  login,
  logout,
  getToken,
  isAuthenticated,
  fetchWithAuth,
}

export default auth
