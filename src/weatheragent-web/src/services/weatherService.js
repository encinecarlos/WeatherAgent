const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || ''

export async function getWeatherSuggestion(location) {
  try {
    const response = await fetch(
      `${API_BASE_URL}/api/concierge?location=${encodeURIComponent(location)}`
    )

    if (!response.ok) {
      const errorContent = await response.text()
      return {
        agentResponse: null,
        isSuccess: false,
        errorMessage: `Error: ${response.status} - ${errorContent}`
      }
    }

    const result = await response.json()
    return {
      agentResponse: result.agentResponse,
      isSuccess: true,
      errorMessage: null
    }
  } catch (error) {
    return {
      agentResponse: null,
      isSuccess: false,
      errorMessage: `Connection error: ${error.message}`
    }
  }
}
