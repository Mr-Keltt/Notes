// src/components/AddButton/AddButton.jsx
import React from 'react';
import './AddButton.css';

const AddButton = ({ onClick }) => {
  return (
    <button className="add-btn" onClick={onClick}>
      +
    </button>
  );
};

export default AddButton;
