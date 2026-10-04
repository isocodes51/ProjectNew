const DAY_NAMES = ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt'];

function formatDate(iso) {
  const d = new Date(iso);
  return `${DAY_NAMES[d.getDay()]} ${d.getDate()}.${d.getMonth() + 1}`;
}

/**
 * Tek bir günün tahmin özetini gösteren kart.
 */
export default function ForecastCard({ day }) {
  return (
    <div className="forecast-card">
      <div className="fc-date">{formatDate(day.date)}</div>
      {day.iconUrl && <img src={day.iconUrl} alt={day.conditionText} className="fc-icon" />}
      <div className="fc-condition">{day.conditionText}</div>
      <div className="fc-temp">
        <span className="fc-max">{Math.round(day.tempMaxC)}°</span>
        <span className="fc-min">{Math.round(day.tempMinC)}°</span>
      </div>
      <div className="fc-meta">
        <span title="Yağış olasılığı">💧 %{day.precipChancePercent} ({day.precipMm.toFixed(1)}mm)</span>
        <span title="Rüzgar">💨 {Math.round(day.windMaxKph)} km/s</span>
        <span title="Nem">🌫️ %{Math.round(day.humidityPercent)}</span>
        <span title="UV indeksi">☀️ UV {day.uvIndex}</span>
        {day.willItSnow && <span title="Kar">❄️ Kar</span>}
      </div>
    </div>
  );
}