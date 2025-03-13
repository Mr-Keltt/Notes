import React from 'react';
import { useNavigate } from 'react-router-dom';
import './NoteCard.css';

const NoteCard = ({ note, onCardClick }) => {
  const navigate = useNavigate();

  const handleEdit = (e) => {
    e.stopPropagation();
    navigate(`/note/edit/${note.uid}`);
  };

  const handleDelete = (e) => {
    e.stopPropagation();
    alert('Удалить заметку');
  };

  const handleCardClick = () => {
    navigate(`/note/${note.uid}`);
  };

  return (
    <div className="note-card" onClick={handleCardClick}>
      <div className="note-actions">
        <button onClick={handleEdit}>✏️</button>
        <button onClick={handleDelete}>❌</button>
      </div>
      <div className="note-title">{note.title}</div>
      <div className="note-text">{note.text}</div>
    </div>
  );
};

export default NoteCard;
