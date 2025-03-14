// src/components/Header/UserChangeButton.jsx

import React from 'react';
import './Header.css';

/**
 * UserChangeButton Component
 * Renders a button that triggers a user change action when clicked.
 */
const UserChangeButton = ({ onClick }) => {
  return (
    // Button with an onClick handler to change the user
    <button className="user-change-btn" onClick={onClick}>
      Сменить пользователя
    </button>
  );
};

export default UserChangeButton;
