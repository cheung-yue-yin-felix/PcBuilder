import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/useAuth.ts'
import { Header } from './Header.tsx'

export function AppHeader() {
  const { isReady, isAuthenticated, user, isAdmin, isMember, logout } = useAuth()
  const navigate = useNavigate()

  async function handleLogout() {
    await logout()
    void navigate('/')
  }

  return (
    <header className="app-header">
      <Link to="/" className="brand">
        PC Builder
      </Link>
      <nav className="app-nav" aria-label="Account">
        {!isReady ? null : <Header isAuthenticated={isAuthenticated} user={user} isAdmin={isAdmin} isMember={isMember} handleLogout={handleLogout} />}
      </nav>
    </header>
  )
}
