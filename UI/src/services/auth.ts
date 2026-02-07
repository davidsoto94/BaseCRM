const TOKEN_KEY = 'accessToken'

type LoginRequest = {
  email: string
  password: string
}

type LoginResponse = {
  accessToken: string
}

type JwtPayload = {
  name?: string;
  given_name?: string;
  family_name?: string;
  email?: string;
  picture?: string;
  permissions?: string;
};

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
  if (!data || !data.accessToken) throw new Error('Invalid login response')
  sessionStorage.setItem(TOKEN_KEY, data.accessToken)
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
  headers.set("Accept-Language", localStorage.getItem('lang') || 'en')
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

/**
 * Decode a JWT token and return its payload
 */
export function decodeJwt(token: string): JwtPayload | null {
  try {
    const payload = token.split('.')[1]
    if (!payload) return null
    const json = atob(payload.replace(/-/g, '+').replace(/_/g, '/'))
    return JSON.parse(json)
  } catch {
    return null
  }
}

/**
 * Get the current user's JWT payload
 */
export function getCurrentUserPayload(): JwtPayload | null {
  const token = getToken()
  if (!token) return null
  return decodeJwt(token)
}

const auth = {
  login,
  logout,
  getToken,
  isAuthenticated,
  fetchWithAuth,
}

export default auth
