// src/components/NoteCard/NoteCard.jsx
import React from 'react';
import './NoteCard.css';

const NoteCard = ({ note }) => {
  const { title, text } = note;
  return (
    <div className="note-card">
      <div className="note-actions">
        <button onClick={() => alert('Редактировать заметку')}>✏️</button>
        <button onClick={() => alert('Удалить заметку')}>❌</button>
      </div>
      <div className="note-title">{title}</div>
      <div className="note-text">{text}</div>
    </div>
  );
};

export default NoteCard;
