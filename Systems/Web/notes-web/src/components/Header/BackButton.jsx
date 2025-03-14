// src/components/Header/BackButton.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Header.css';

const BackButton = () => {
  const navigate = useNavigate();
  return (
    <button className="user-change-btn" onClick={() => navigate('/')}>
      Назад
    </button>
  );
};

export default BackButton;
