import { Link } from 'react-router-dom'
import '../styles/home.css'

function Home() {
  return (
    <div className="home-container">
      <div className="hero-section">
        <h1>
          <span className="hero-title-highlight">WeatherTaste</span>
        </h1>
        <p className="tagline">Your AI-powered lifestyle assistant</p>
        <p className="description">
          Get personalized recommendations for <strong>what to wear</strong> and{' '}
          <strong>what to eat</strong> based on the current weather in your city.
        </p>
        <Link to="/weather" className="btn btn-primary btn-lg start-btn">
          Get Started
        </Link>
      </div>

      <div className="features-section">
        <div className="feature-card">
          <div className="feature-icon">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M20.38 3.46 16 2 12 3.46 8 2 3.62 3.46a2 2 0 0 0-1.34 1.89v13.3a2 2 0 0 0 2.26 1.98L8 20l4-1.46L16 20l3.46 1.63a2 2 0 0 0 2.26-1.98V5.35a2 2 0 0 0-1.34-1.89z" />
            </svg>
          </div>
          <h3>Clothing Suggestions</h3>
          <p>
            Smart recommendations based on temperature, apparent temperature, and rain
            probability.
          </p>
        </div>
        <div className="feature-card">
          <div className="feature-icon">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M12 2a10 10 0 1 0 10 10 4 4 0 0 1-5-5 4 4 0 0 1-5-5" />
              <path d="M8.5 8.5v.01" />
              <path d="M16 15.5v.01" />
              <path d="M12 12v.01" />
              <path d="M11 17v.01" />
              <path d="M7 14v.01" />
            </svg>
          </div>
          <h3>Food & Drink Ideas</h3>
          <p>
            Comfort food for cold days, light meals for hot weather, and everything in
            between.
          </p>
        </div>
        <div className="feature-card">
          <div className="feature-icon">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M12 8V4H8" />
              <rect width="16" height="12" x="4" y="8" rx="2" />
              <path d="M2 14h2" />
              <path d="M20 14h2" />
              <path d="M15 13v2" />
              <path d="M9 13v2" />
            </svg>
          </div>
          <h3>AI-Powered</h3>
          <p>
            Powered by Azure OpenAI and the Microsoft Agent Framework for intelligent
            responses.
          </p>
        </div>
      </div>
    </div>
  )
}

export default Home
