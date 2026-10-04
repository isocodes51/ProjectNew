import { useState } from 'react';

/**
 * 81 il listesinden çoklu seçim yapılmasını sağlayan bileşen.
 */
export default function CitySelector({ cities, selected, onChange }) {
  const [filter, setFilter] = useState('');

  const filtered = cities.filter((c) =>
    c.name.toLocaleLowerCase('tr').includes(filter.toLocaleLowerCase('tr'))
  );

  const toggle = (name) => {
    if (selected.includes(name)) {
      onChange(selected.filter((n) => n !== name));
    } else {
      onChange([...selected, name]);
    }
  };

  return (
    <div className="city-selector">
      <input
        type="text"
        placeholder="İl ara..."
        value={filter}
        onChange={(e) => setFilter(e.target.value)}
        className="city-filter"
      />
      <div className="city-list">
        {filtered.map((city) => (
          <label key={city.name} className="city-item">
            <input
              type="checkbox"
              checked={selected.includes(city.name)}
              onChange={() => toggle(city.name)}
            />
            <span>{city.name}</span>
          </label>
        ))}
      </div>
    </div>
  );
}