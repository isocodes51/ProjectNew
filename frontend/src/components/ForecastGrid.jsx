import ForecastCard from './ForecastCard';

/**
 * Bir şehrin 7 günlük tahminini yatay kart listesi olarak gösterir.
 */
export default function ForecastGrid({ forecast }) {
  return (
    <section className="forecast-grid">
      <header className="fg-header">
        <h2>{forecast.city}{forecast.region ? `, ${forecast.region}` : ''}</h2>
        <span className="fg-tz">{forecast.timezone}</span>
      </header>
      <div className="fg-days">
        {forecast.days.map((day) => (
          <ForecastCard key={day.date} day={day} />
        ))}
      </div>
    </section>
  );
}