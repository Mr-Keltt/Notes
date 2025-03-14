// src/pages/Home/Home.jsx
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

  const handleAddNote = () => {
    navigate('/note/new');
  };

  const handleCardClick = (noteId) => {
    navigate(`/note/${noteId}`);
  };

  const handleDeleteNote = (deletedNoteId) => {
    setNotes(prevNotes => prevNotes.filter(note => note.uid !== deletedNoteId));
  };

  return (
    <>
      <Header />
      <div className="main-content">
        <div className="container">
          {notes.length > 0 ? (
            <div className="notes-grid">
              {notes.map(note => (
                <NoteCard 
                  key={note.uid} 
                  note={note} 
                  onCardClick={handleCardClick}
                  onDelete={handleDeleteNote}
                />
              ))}
            </div>
          ) : (
            <div className="no-users">Заметки отсутствуют</div>
          )}
        </div>
      </div>
      <AddButton onClick={handleAddNote} />
    </>
  );
};

export default Home;
