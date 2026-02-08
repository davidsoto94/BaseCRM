export function fetchWithLanguage(input: RequestInfo, init?: RequestInit): Promise<Response> {
  const lang = localStorage.getItem('lang') || 'en';
    const config = init ? { ...init } : {};
    config.credentials = 'include';
    config.headers = {
      ...config.headers,
      'Accept-Language': lang,
    };
    return fetch(input, config);
}