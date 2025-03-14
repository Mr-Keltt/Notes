// WarningBanner component: displays a dismissible warning banner with persistence.

import React, { useState, useEffect } from 'react';
import './WarningBanner.css';

const STORAGE_KEY = 'warningBannerClosed';
const ONE_HOUR = 3600000; // One hour in milliseconds

const WarningBanner = ({ text }) => {
  // Local state to track banner visibility
  const [isVisible, setIsVisible] = useState(true);

  // Check localStorage for previous dismissal on mount
  useEffect(() => {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored) {
      try {
        const data = JSON.parse(stored);
        // If the banner was closed less than an hour ago, hide it
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

  // Close banner and store the dismissal timestamp
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
