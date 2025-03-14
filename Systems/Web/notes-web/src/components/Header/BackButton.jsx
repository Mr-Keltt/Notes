// src/components/Header/BackButton.jsx

import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Header.css';

/**
 * BackButton Component
 * Renders a button that navigates the user back to the homepage when clicked.
 */
const BackButton = () => {
  // Hook for programmatic navigation
  const navigate = useNavigate();
  return (
    // Button with a click handler that navigates to the homepage
    <button className="user-change-btn" onClick={() => navigate('/')}>
      Назад
    </button>
  );
};

export default BackButton;
