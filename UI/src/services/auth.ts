const TOKEN_KEY = 'id_token'

type LoginRequest = {
  email: string
  password: string
}

type LoginResponse = {
  token: string
  expiresIn?: number
}

const apiBase = import.meta.env.VITE_API_BASE_URL || ''

export async function login(payload: LoginRequest) {
  const res = await fetch(`${apiBase}/api/v1/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  })

  if (!res.ok) {
    const errText = await res.text()
    throw new Error(errText || 'Login failed')
  }

  const data: LoginResponse = await res.json()
  if (!data || !data.token) throw new Error('Invalid login response')
  localStorage.setItem(TOKEN_KEY, data.token)
  return data
}

export function logout() {
  localStorage.removeItem(TOKEN_KEY)
}

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function isAuthenticated(): boolean {
  return !!getToken()
}

export async function fetchWithAuth(input: RequestInfo, init?: RequestInit) {
  const token = getToken()
  const headers = new Headers(init?.headers || {})
  if (token) headers.set('Authorization', `Bearer ${token}`)
  const res = await fetch(input, { ...init, headers })
  return res
}

export default { login, logout, getToken, isAuthenticated, fetchWithAuth }
