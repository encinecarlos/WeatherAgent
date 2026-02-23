const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || ''

export async function getWeatherSuggestion(location) {
  try {
    const response = await fetch(
      `${API_BASE_URL}/api/concierge?location=${encodeURIComponent(location)}`
    )

    if (!response.ok) {
      return {
        agentResponse: null,
        isSuccess: false,
        errorMessage: `Network error: Unable to connect to the server. Please try again later.`
      }
    }

    const result = await response.json()
    
    // Novo formato de resposta do back-end usando Result pattern
    if (result.success === false) {
      return {
        agentResponse: null,
        isSuccess: false,
        errorMessage: result.error || 'An unexpected error occurred'
      }
    }

    return {
      agentResponse: result.agentResponse,
      isSuccess: true,
      errorMessage: null
    }
  } catch (error) {
    return {
      agentResponse: null,
      isSuccess: false,
      errorMessage: `Connection error: Unable to reach the server. Please check your internet connection and try again.`
    }
  }
}
