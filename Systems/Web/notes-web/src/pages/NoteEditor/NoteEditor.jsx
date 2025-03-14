// src/pages/NoteEditor/NoteEditor.jsx

import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import { useActiveUserContext } from '../../context/ActiveUserContext';
import './NoteEditor.css';

/**
 * NoteEditor Component
 * Renders a form for creating or editing a note.
 * It fetches note data in edit mode and saves updates or new notes.
 */
const NoteEditor = () => {
  const { noteId } = useParams();
  const navigate = useNavigate();
  const isEditMode = Boolean(noteId);
  const { activeUser } = useActiveUserContext();

  const [title, setTitle] = useState('');
  const [text, setText] = useState('');
  const [marked, setMarked] = useState(false);
  const [images, setImages] = useState([]);

  // Fetch note data if in edit mode
  useEffect(() => {
    if (isEditMode) {
      const fetchNote = async () => {
        try {
          const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
          const response = await fetch(`${baseUrl}/api/Notes/${noteId}`);
          if (!response.ok) {
            throw new Error('Ошибка при загрузке заметки');
          }
          const note = await response.json();
          setTitle(note.title);
          setText(note.text);
          setMarked(note.marked);
          setImages(note.photos || []);
        } catch (error) {
          console.error(error);
          alert('Ошибка загрузки заметки');
        }
      };
      fetchNote();
    }
  }, [isEditMode, noteId]);

  // Handle attach photo action
  const handleAttachPhoto = () => {
    alert('Будет скоро!');
  };

  // Remove an image from the list by uid
  const handleDeleteImage = (uid) => {
    setImages(prev => prev.filter(img => img.uid !== uid));
  };

  // Save or update note data
  const handleSaveNote = async () => {
    if (title.trim().length === 0) {
      alert('Название заметки не может быть пустым');
      return;
    }

    try {
      const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
      if (isEditMode) {
        const response = await fetch(`${baseUrl}/api/Notes/${noteId}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ title, text, marked }),
        });
        if (response.status !== 204) {
          throw new Error('Ошибка обновления заметки');
        }
      } else {
        if (!activeUser) {
          alert('Не выбран активный пользователь');
          return;
        }
        const response = await fetch(`${baseUrl}/api/Notes`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ title, text, marked, userId: activeUser }),
        });
        if (response.status !== 201) {
          throw new Error('Ошибка создания заметки');
        }
      }

      navigate('/');
    } catch (error) {
      console.error(error);
      alert(error.message);
    }
  };

  return (
    <>
      <Header showBack={true} />
      <div className="note-editor-container">
        <div className="note-editor-panel">
          <label htmlFor="title">Название</label>
          <input 
            id="title"
            type="text" 
            value={title} 
            onChange={(e) => setTitle(e.target.value)}
          />
          <label htmlFor="text">Текст</label>
          <textarea 
            id="text"
            value={text} 
            onChange={(e) => setText(e.target.value)}
          ></textarea>
          <button className="attach-photo-btn" onClick={handleAttachPhoto}>
            Прикрепить фото
          </button>
          <div className="note-images-grid">
            {images.map(image => (
              <div className="image-wrapper" key={image.uid}>
                <img 
                  src={image.url} 
                  alt=""
                />
                <button 
                  className="delete-image-btn" 
                  onClick={() => handleDeleteImage(image.uid)}
                >
                  ✖
                </button>
              </div>
            ))}
          </div>
          <button className="save-note-btn" onClick={handleSaveNote}>
            Сохранить заметку
          </button>
        </div>
      </div>
    </>
  );
};

export default NoteEditor;
