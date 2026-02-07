import { Routes, Route } from 'react-router-dom'
import Login from './pages/Login'
import Home from './pages/Home'
import Register from './pages/Register'
import ForgotPassword from './pages/ForgotPassword'
import ResetPassword from './pages/ResetPassword'
import ConfirmEmail from './pages/ConfirmEmail'
import Users from './pages/Users'
import MFASetup from './pages/MFASetup'
import MFAVerify from './pages/MFAVerify'
import { ProtectedRoute } from './components/ProtectedRoute'
import { ThemeProvider } from './style/ThemeProvider'



function App() {
  return (
    <ThemeProvider>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/mfa-setup" element={<MFASetup />} />
        <Route path="/mfa-verify" element={<MFAVerify />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/reset-password" element={<ResetPassword />} />
        <Route path="/confirm-email" element={<ConfirmEmail />} />
        <Route element={<ProtectedRoute />}>
          <Route path="/" element={<Home />} />
          <Route path="/register" element={<Register />} />
          <Route path="/users" element={<Users />} />
        </Route>
      </Routes>
    </ThemeProvider>
  )
}

export default App
