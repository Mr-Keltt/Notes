// src/components/UserCard/UserCard.jsx

import React from 'react';
import './UserCard.css';

/**
 * UserCard Component
 * Renders a card displaying a user's identifier and a delete button.
 * Clicking the card (if not active) triggers the onClick handler.
 * Clicking the delete button triggers the onDelete handler.
 */
const UserCard = ({ user, active, onClick, onDelete }) => {
  return (
    // Card container with conditional active styling and click handler
    <div 
      className={`user-card ${active ? 'active' : ''}`}
      onClick={() => { 
        if (!active) onClick(user.guid); 
      }}
      style={{ cursor: 'pointer' }}
    >
      {/* Display user's unique identifier */}
      <div>{user.guid}</div>
      {/* Delete button with click event to remove the user */}
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
