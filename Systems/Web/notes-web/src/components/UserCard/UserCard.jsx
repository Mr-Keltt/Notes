// src/components/UserCard/UserCard.jsx
import React from 'react';
import './UserCard.css';

const UserCard = ({ user, active, onClick, onDelete }) => {
  return (
    <div 
      className={`user-card ${active ? 'active' : ''}`}
      onClick={() => { 
        if (!active) onClick(user.guid); 
      }}
      style={{ cursor: 'pointer' }}
    >
      <div>{user.guid}</div>
      <button 
        className="delete-btn" 
        onClick={(e) => { 
          e.stopPropagation();
          onDelete(user.guid);
        }}
      >
        ❌
      </button>
    </div>
  );
};

export default UserCard;
