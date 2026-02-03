export function fecthWithLanguage(input: RequestInfo, init?: RequestInit): Promise<Response> {
  const lang = localStorage.getItem('lang') || 'en';
    const config = init ? { ...init } : {};
    config.headers = {
      ...config.headers,
      'Accept-Language': lang,
    };
    return fetch(input, config);
}