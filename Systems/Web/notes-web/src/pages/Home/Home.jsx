// Home component: displays notes for the active user with options to filter, view, add, or delete notes.

import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import NoteCard from '../../components/NoteCard/NoteCard';
import AddButton from '../../components/AddButton/AddButton';
import { useActiveUserContext } from '../../context/ActiveUserContext';
import './Home.css';

const Home = () => {
  const navigate = useNavigate();
  const { activeUser } = useActiveUserContext();
  const [notes, setNotes] = useState([]);
  const [showOnlyMarked, setShowOnlyMarked] = useState(false);

  // Fetch notes for the active user when activeUser changes
  useEffect(() => {
    if (activeUser) {
      const fetchNotes = async () => {
        try {
          const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
          const response = await fetch(`${baseUrl}/api/Notes?userId=${activeUser}`);
          if (!response.ok) {
            throw new Error('Ошибка при загрузке заметок');
          }
          const data = await response.json();
          setNotes(data);
        } catch (error) {
          console.error(error);
        }
      };
      fetchNotes();
    }
  }, [activeUser]);

  // Navigate to the note creation page
  const handleAddNote = () => {
    navigate('/note/new');
  };

  // Navigate to the note details page on card click
  const handleCardClick = (noteId) => {
    navigate(`/note/${noteId}`);
  };

  // Remove the deleted note from the state
  const handleDeleteNote = (deletedNoteId) => {
    setNotes(prevNotes => prevNotes.filter(note => note.uid !== deletedNoteId));
  };

  // Filter notes based on the marked status
  const filteredNotes = showOnlyMarked
    ? notes.filter(note => note.marked)
    : notes;

  return (
    <>
      <Header />
      <div className="main-content">
        <div className="container">
          {/* Checkbox to filter only marked notes */}
          <div className="filter-container">
            <label className="filter-label">
              <input
                type="checkbox"
                checked={showOnlyMarked}
                onChange={() => setShowOnlyMarked(prev => !prev)}
              />
              {' '}Показывать только избраные
            </label>
          </div>
          {filteredNotes.length > 0 ? (
            <div className="notes-grid">
              {filteredNotes.map(note => (
                <NoteCard 
                  key={note.uid} 
                  note={note} 
                  onCardClick={handleCardClick}
                  onDelete={handleDeleteNote}
                  refreshNotes={() => {
                    // Toggle the marked state of the note in the local state
                    setNotes(prevNotes =>
                      prevNotes.map(n => n.uid === note.uid ? { ...n, marked: !n.marked } : n)
                    );
                  }}
                />
              ))}
            </div>
          ) : (
            <div className="no-notes">Заметки отсутствуют</div>
          )}
        </div>
      </div>
      <AddButton onClick={handleAddNote} />
    </>
  );
};

export default Home;
