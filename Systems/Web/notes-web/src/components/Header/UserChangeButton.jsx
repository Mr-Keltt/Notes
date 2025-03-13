// src/components/Header/UserChangeButton.jsx
import React from 'react';
import './Header.css';

const UserChangeButton = ({ onClick }) => {
  return (
    <button className="user-change-btn" onClick={onClick}>
      Сменить пользователя
    </button>
  );
};

export default UserChangeButton;
