import axios from 'axios';

const api = axios.create({
  baseURL: '/api',
});

export async function fetchCities() {
  const { data } = await api.get('/weather/cities');
  return data;
}

export async function fetchForecast(cityName, days = 7) {
  const { data } = await api.get(`/weather/${encodeURIComponent(cityName)}`, {
    params: { days },
  });
  return data;
}

export async function fetchBatchForecasts(cityNames, days = 7) {
  const { data } = await api.get('/weather/batch', {
    params: { cities: cityNames.join(','), days },
  });
  // Backend { results: [...], errors: [...] } döndürür
  return Array.isArray(data) ? { results: data, errors: [] } : data;
}