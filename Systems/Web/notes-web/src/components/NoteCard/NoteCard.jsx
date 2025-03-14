// src/components/NoteCard/NoteCard.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { deleteNote } from '../../helpers/deleteNote';
import './NoteCard.css';

const NoteCard = ({ note, onCardClick, refreshNotes, onDelete }) => {
  const navigate = useNavigate();

  const handleEdit = (e) => {
    e.stopPropagation();
    navigate(`/note/edit/${note.uid}`);
  };

  const handleDelete = async (e) => {
    e.stopPropagation();
    if (window.confirm('Удалить заметку?')) {
      try {
        await deleteNote(note.uid);
        // Если передан callback, обновляем список заметок
        if (onDelete) onDelete(note.uid);
        if (refreshNotes) refreshNotes();
      } catch (error) {
        console.error(error);
        alert(error.message);
      }
    }
  };

  const handleCardClick = () => {
    if (onCardClick) {
      onCardClick(note.uid);
    } else {
      navigate(`/note/${note.uid}`);
    }
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
