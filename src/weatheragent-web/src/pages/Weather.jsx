import { useState } from 'react'
import { getWeatherSuggestion } from '../services/weatherService'
import '../styles/weather.css'

function Weather() {
  const [location, setLocation] = useState('')
  const [agentResponse, setAgentResponse] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isLoading, setIsLoading] = useState(false)

  const handleKeyPress = (e) => {
    if (e.key === 'Enter' && location.trim()) {
      getSuggestion()
    }
  }

  const getSuggestion = async () => {
    if (!location.trim()) {
      setErrorMessage('Please enter a city name')
      return
    }

    setIsLoading(true)
    setErrorMessage('')
    setAgentResponse('')

    try {
      const result = await getWeatherSuggestion(location.trim())

      if (result.isSuccess) {
        setAgentResponse(result.agentResponse || 'No response received')
      } else {
        setErrorMessage(result.errorMessage || 'An unknown error occurred')
      }
    } catch (error) {
      setErrorMessage(`Connection error: ${error.message}`)
    } finally {
      setIsLoading(false)
    }
  }

  const formatResponse = (response) => {
    if (!response) return ''

    const formatted = response
      .replace(/\n\n/g, '</p><p>')
      .replace(/\n/g, '<br/>')
      .replace(/\*\*/g, '')
      .replace(/1\)/g, '<strong>1)</strong>')
      .replace(/2\)/g, '<strong>2)</strong>')
      .replace(/3\)/g, '<strong>3)</strong>')
      .replace(/4\)/g, '<strong>4)</strong>')

    return `<p>${formatted}</p>`
  }

  return (
    <div className="weather-container">
      <div className="weather-header">
        <h1>🌤️ WeatherTaste</h1>
        <p className="subtitle">
          Get personalized clothing and food suggestions based on weather
        </p>
      </div>

      <div className="search-section">
        <div className="input-group">
          <input
            type="text"
            className="form-control search-input"
            placeholder="Enter city name (e.g., São Paulo, New York, London)"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            onKeyPress={handleKeyPress}
            disabled={isLoading}
          />
          <button
            className="btn btn-primary search-btn"
            onClick={getSuggestion}
            disabled={isLoading || !location.trim()}
          >
            {isLoading ? (
              <>
                <span className="spinner"></span>
                <span>Loading...</span>
              </>
            ) : (
              <span>Get Suggestions</span>
            )}
          </button>
        </div>
      </div>

      {errorMessage && (
        <div className="alert alert-danger mt-4" role="alert">
          <strong>Error:</strong> {errorMessage}
        </div>
      )}

      {agentResponse && (
        <div className="response-card mt-4">
          <div
            className="response-content"
            dangerouslySetInnerHTML={{ __html: formatResponse(agentResponse) }}
          />
        </div>
      )}
    </div>
  )
}

export default Weather
