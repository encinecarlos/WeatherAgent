import { Link } from 'react-router-dom'
import '../styles/home.css'

function Home() {
  return (
    <div className="home-container">
      <div className="hero-section">
        <h1>??? WeatherTaste</h1>
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
          <div className="feature-icon">??</div>
          <h3>Clothing Suggestions</h3>
          <p>
            Smart recommendations based on temperature, apparent temperature, and rain
            probability.
          </p>
        </div>
        <div className="feature-card">
          <div className="feature-icon">???</div>
          <h3>Food & Drink Ideas</h3>
          <p>
            Comfort food for cold days, light meals for hot weather, and everything in
            between.
          </p>
        </div>
        <div className="feature-card">
          <div className="feature-icon">??</div>
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
