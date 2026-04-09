import { useState } from 'react'
import { getWeatherSuggestion } from '../services/weatherService'
import '../styles/weather.css'

/**
 * Extracts temperature from the agent response text.
 * Looks for patterns like "25°C", "25 °C", "25°", "77°F", etc.
 * Returns temperature in Celsius.
 */
function extractTemperature(responseText) {
  if (!responseText) return null

  // Match patterns like: 25°C, 25 °C, 25°c, -5°C
  const celsiusMatch = responseText.match(/-?\d+(?:\.\d+)?\s*°\s*[Cc]/)
  if (celsiusMatch) {
    return parseFloat(celsiusMatch[0])
  }

  // Match patterns like: 77°F, 77 °F
  const fahrenheitMatch = responseText.match(/-?\d+(?:\.\d+)?\s*°\s*[Ff]/)
  if (fahrenheitMatch) {
    const f = parseFloat(fahrenheitMatch[0])
    return Math.round((f - 32) * 5 / 9)
  }

  // Match patterns like: temperatura de 25, temperature of 25, temperature: 25
  const tempWordMatch = responseText.match(/[Tt]emperat\w+[:\s]+(?:de\s+|of\s+|is\s+)?(-?\d+(?:\.\d+)?)/i)
  if (tempWordMatch) {
    return parseFloat(tempWordMatch[1])
  }

  return null
}

/**
 * Returns a temperature class and label based on the temperature value.
 */
function getTemperatureInfo(temp) {
  if (temp === null) return { className: '', label: '', icon: '' }
  if (temp <= 0) return { className: 'temp-freezing', label: `${temp}°C - Freezing`, icon: '&#10052;' }
  if (temp <= 10) return { className: 'temp-cold', label: `${temp}°C - Cold`, icon: '&#10052;' }
  if (temp <= 18) return { className: 'temp-cool', label: `${temp}°C - Cool`, icon: '&#9729;' }
  if (temp <= 24) return { className: 'temp-mild', label: `${temp}°C - Mild`, icon: '&#9925;' }
  if (temp <= 30) return { className: 'temp-warm', label: `${temp}°C - Warm`, icon: '&#9728;' }
  if (temp <= 38) return { className: 'temp-hot', label: `${temp}°C - Hot`, icon: '&#9728;' }
  return { className: 'temp-extreme', label: `${temp}°C - Extreme`, icon: '&#128293;' }
}

function Weather() {
  const [location, setLocation] = useState('')
  const [agentResponse, setAgentResponse] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [temperature, setTemperature] = useState(null)

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
    setTemperature(null)

    try {
      const result = await getWeatherSuggestion(location.trim())

      if (result.isSuccess) {
        const response = result.agentResponse || 'No response received'
        setAgentResponse(response)
        const extractedTemp = extractTemperature(response)
        setTemperature(extractedTemp)
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

  const tempInfo = getTemperatureInfo(temperature)

  return (
    <div className="weather-container">
      <div className="weather-header">
        <h1>Weather Suggestions</h1>
        <p className="subtitle">
          Get personalized clothing and food suggestions based on weather
        </p>
      </div>

      <div className="search-section">
        <div className="input-group">
          <input
            type="text"
            className="form-control search-input"
            placeholder="Enter city name (e.g., S\u00e3o Paulo, New York, London)"
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
                <span>Analyzing...</span>
              </>
            ) : (
              <span>
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" style={{ marginRight: '0.5rem', verticalAlign: 'middle' }}>
                  <circle cx="11" cy="11" r="8" />
                  <path d="m21 21-4.3-4.3" />
                </svg>
                Get Suggestions
              </span>
            )}
          </button>
        </div>
      </div>

      {isLoading && (
        <div className="loading-container mt-4">
          <div className="loading-orb"></div>
          <p className="loading-text">Analyzing weather conditions for {location}...</p>
        </div>
      )}

      {errorMessage && (
        <div className="alert alert-danger mt-4" role="alert">
          <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
            <span style={{ fontSize: '1.5rem' }}>⚠️</span>
            <div>
              <strong>Oops!</strong> {errorMessage}
            </div>
          </div>
        </div>
      )}

      {agentResponse && (
        <div className={`response-card mt-4 ${tempInfo.className}`}>
          {temperature !== null && (
            <div className={`temp-badge ${tempInfo.className}`}>
              <span className="temp-icon" dangerouslySetInnerHTML={{ __html: tempInfo.icon }} />
              <span>{tempInfo.label}</span>
            </div>
          )}
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
