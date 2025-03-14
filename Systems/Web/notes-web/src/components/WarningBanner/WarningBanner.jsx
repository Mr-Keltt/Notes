import React, { useState, useEffect } from 'react';
import './WarningBanner.css';

const STORAGE_KEY = 'warningBannerClosed';
const ONE_HOUR = 3600000; // 1 час в миллисекундах

const WarningBanner = ({ text }) => {
  const [isVisible, setIsVisible] = useState(true);

  useEffect(() => {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored) {
      try {
        const data = JSON.parse(stored);
        // Если плашка закрыта и сохранённое время меньше часа назад – скрываем её
        if (data.closed && Date.now() - data.timestamp < ONE_HOUR) {
          setIsVisible(false);
        } else {
          localStorage.removeItem(STORAGE_KEY);
        }
      } catch (e) {
        console.error('Ошибка чтения localStorage', e);
      }
    }
  }, []);

  const handleClose = () => {
    setIsVisible(false);
    localStorage.setItem(
      STORAGE_KEY,
      JSON.stringify({ closed: true, timestamp: Date.now() })
    );
  };

  if (!isVisible) return null;

  return (
    <div className="warning-banner">
      <span className="warning-text">{text}</span>
      <button className="warning-close" onClick={handleClose}>✖</button>
    </div>
  );
};

export default WarningBanner;
