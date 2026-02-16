import { useState } from 'react'
import { Link, Outlet } from 'react-router-dom'
import '../styles/layout.css'

function Layout() {
  const [isMenuOpen, setIsMenuOpen] = useState(false)

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen)
  }

  return (
    <div className="layout">
      <nav className="navbar">
        <div className="navbar-container">
          <Link to="/" className="navbar-brand">
            🌤️ WeatherTaste
          </Link>
          <button className="navbar-toggler" onClick={toggleMenu}>
            <span className="navbar-toggler-icon">☰</span>
          </button>
        </div>
        <div className={`nav-menu ${isMenuOpen ? 'open' : ''}`}>
          <Link to="/" className="nav-link" onClick={() => setIsMenuOpen(false)}>
            🏠 Home
          </Link>
          <Link to="/weather" className="nav-link" onClick={() => setIsMenuOpen(false)}>
            🌡️ Weather Suggestions
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
