import React from 'react';
import { useNavigate } from 'react-router-dom';
import './AddNoteButton.css';

const AddNoteButton = () => {
  const navigate = useNavigate();
  const handleClick = () => {
    navigate('/note/new');
  };

  return (
    <button className="add-note-btn" onClick={handleClick}>
      +
    </button>
  );
};

export default AddNoteButton;
