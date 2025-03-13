// src/components/UserCard/UserCard.jsx
import React from 'react';
import './UserCard.css';

const UserCard = ({ user, active, onDelete }) => {
  return (
    <div className={`user-card ${active ? 'active' : ''}`}>
      <div>{user.guid}</div>
      <button className="delete-btn" onClick={() => onDelete(user.guid)}>
        ❌
      </button>
    </div>
  );
};

export default UserCard;
