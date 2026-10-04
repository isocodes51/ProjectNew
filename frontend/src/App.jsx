import { useEffect, useState } from 'react';
import CitySelector from './components/CitySelector';
import ForecastGrid from './components/ForecastGrid';
import { fetchCities, fetchBatchForecasts } from './services/api';
import './App.css';

export default function App() {
  const [cities, setCities] = useState([]);
  const [selected, setSelected] = useState([]);
  const [forecasts, setForecasts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchCities().then(setCities).catch(() => setError('İl listesi yüklenemedi.'));
  }, []);

  const loadForecasts = async () => {
    if (selected.length === 0) return;
    setLoading(true);
    setError(null);
    try {
      const { results = [], errors = [] } = await fetchBatchForecasts(selected, 7);
      setForecasts(results);
      if (results.length === 0) {
        setError(errors.length > 0 ? `Hiç veri alınamadı: ${errors.join(' | ')}` : 'Sonuç bulunamadı.');
      } else if (errors.length > 0) {
        setError(`Bazı iller alınamadı: ${errors.join(' | ')}`);
      }
    } catch (e) {
      const detail = e?.response?.data?.message;
      setError(detail ? `Hata: ${detail}` : 'Hava durumu verisi alınamadı. Backend çalışıyor mu?');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="app">
      <header className="app-header">
        <h1>🌤️ Türkiye Hava Durumu</h1>
        <p>Seçtiğiniz iller için önümüzdeki 7 günlük tahmin</p>
      </header>

      <main className="app-main">
        <aside className="sidebar">
          <h3>İller ({selected.length} seçili)</h3>
          <CitySelector cities={cities} selected={selected} onChange={setSelected} />
          <button
            className="btn-primary"
            onClick={loadForecasts}
            disabled={loading || selected.length === 0}
          >
            {loading ? 'Yükleniyor...' : '🔍 Tahminleri Getir'}
          </button>
          {selected.length > 0 && (
            <button className="btn-secondary" onClick={() => setSelected([])}>
              Seçimi Temizle
            </button>
          )}
        </aside>

        <section className="results">
          {error && <div className="error-box">⚠️ {error}</div>}
          {!error && forecasts.length === 0 && !loading && (
            <div className="empty-state">
              Soldaki listeden illeri seçip <b>Tahminleri Getir</b>'e tıklayın.
            </div>
          )}
          {forecasts.map((f) => (
            <ForecastGrid key={f.city} forecast={f} />
          ))}
        </section>
      </main>
    </div>
  );
}