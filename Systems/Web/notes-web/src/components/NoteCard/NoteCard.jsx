// NoteCard component: displays a note with actions for mark, edit, and delete.

import React from 'react';
import { useNavigate } from 'react-router-dom';
import { deleteNote } from '../../helpers/deleteNote';
import './NoteCard.css';

const NoteCard = ({ note, onCardClick, refreshNotes, onDelete }) => {
  const navigate = useNavigate();

  // Navigate to edit the note
  const handleEdit = (e) => {
    e.stopPropagation();
    navigate(`/note/edit/${note.uid}`);
  };

  // Delete the note after confirmation
  const handleDelete = async (e) => {
    e.stopPropagation();
    if (window.confirm('Удалить заметку?')) {
      try {
        await deleteNote(note.uid);
        if (onDelete) onDelete(note.uid);
        if (refreshNotes) refreshNotes();
      } catch (error) {
        console.error(error);
        alert(error.message);
      }
    }
  };

  // Toggle the note's marked status
  const handleToggleMark = async (e) => {
    e.stopPropagation();
    const newMarked = !note.marked;
    try {
      const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
      const response = await fetch(`${baseUrl}/api/Notes/${note.uid}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: note.title,
          text: note.text,
          marked: newMarked
        })
      });
      if (!response.ok) {
        throw new Error('Ошибка при обновлении статуса избранного');
      }
      if (refreshNotes) refreshNotes();
    } catch (error) {
      console.error(error);
      alert(error.message);
    }
  };

  // Open note details or execute custom card click handler
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
        {/* Toggle favorite status */}
        <button 
          onClick={handleToggleMark} 
          className="star-button"
          style={{ color: note.marked ? '#FFD700' : 'gray' }}
          title="Отметить как избранное"
        >
          ★
        </button>
        {/* Edit the note */}
        <button onClick={handleEdit}>✏️</button>
        {/* Delete the note */}
        <button onClick={handleDelete}>❌</button>
      </div>
      <div className="note-title">{note.title}</div>
      <div className="note-text">{note.text}</div>
    </div>
  );
};

export default NoteCard;
