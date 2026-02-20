import { useState } from 'react'
import { Link, Outlet, useLocation } from 'react-router-dom'
import '../styles/layout.css'

function Layout() {
  const [isMenuOpen, setIsMenuOpen] = useState(false)
  const location = useLocation()

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen)
  }

  const isActive = (path) => {
    if (path === '/') return location.pathname === '/'
    return location.pathname.startsWith(path)
  }

  return (
    <div className="layout">
      <nav className="navbar">
        <div className="navbar-container">
          <Link to="/" className="navbar-brand">
            <span className="brand-icon">W</span>
            WeatherTaste
          </Link>
          <button className="navbar-toggler" onClick={toggleMenu} aria-label="Toggle navigation">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              {isMenuOpen ? (
                <>
                  <line x1="18" y1="6" x2="6" y2="18" />
                  <line x1="6" y1="6" x2="18" y2="18" />
                </>
              ) : (
                <>
                  <line x1="3" y1="6" x2="21" y2="6" />
                  <line x1="3" y1="12" x2="21" y2="12" />
                  <line x1="3" y1="18" x2="21" y2="18" />
                </>
              )}
            </svg>
          </button>
        </div>
        <div className={`nav-menu ${isMenuOpen ? 'open' : ''}`}>
          <Link
            to="/"
            className={`nav-link ${isActive('/') ? 'active' : ''}`}
            onClick={() => setIsMenuOpen(false)}
          >
            Home
          </Link>
          <Link
            to="/weather"
            className={`nav-link ${isActive('/weather') ? 'active' : ''}`}
            onClick={() => setIsMenuOpen(false)}
          >
            Weather Suggestions
          </Link>
        </div>
      </nav>
      <main className="content">
        <Outlet />
      </main>
    </div>
  )
}

export default Layout
