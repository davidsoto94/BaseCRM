import auth from '../services/auth'
import { useNavigate } from 'react-router-dom'

export default function Home() {
  const navigate = useNavigate()

  function handleLogout() {
    auth.logout()
    navigate('/login')
  }

  return (
    <div style={{ padding: 24 }}>
      <h1>Welcome</h1>
      <p>You are signed in.</p>
      <button onClick={handleLogout} style={{ padding: '8px 12px' }}>
        Sign out
      </button>
    </div>
  )
}
