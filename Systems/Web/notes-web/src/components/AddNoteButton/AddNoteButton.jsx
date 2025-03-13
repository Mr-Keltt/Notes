// src/components/AddNoteButton/AddNoteButton.jsx
import React from 'react';
import './AddNoteButton.css';

const AddNoteButton = ({ onClick }) => {
  return (
    <button className="add-note-btn" onClick={onClick}>
      +
    </button>
  );
};

export default AddNoteButton;
