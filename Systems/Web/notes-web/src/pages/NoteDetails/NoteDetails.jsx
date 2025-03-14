// src/pages/NoteDetails/NoteDetails.jsx
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import ModalImage from '../../components/ModalImage/ModalImage';
import { deleteNote } from '../../helpers/deleteNote';
import './NoteDetails.css';

// Форматирование даты в формате dd.mm.yyyy
const formatDate = (dateString) => {
  const date = new Date(dateString);
  const dd = String(date.getDate()).padStart(2, '0');
  const mm = String(date.getMonth() + 1).padStart(2, '0');
  const yyyy = date.getFullYear();
  return `${dd}.${mm}.${yyyy}`;
};

const NoteDetails = () => {
  const { noteId } = useParams();
  const navigate = useNavigate();
  
  const [note, setNote] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [modalImage, setModalImage] = useState(null);

  useEffect(() => {
    const fetchNote = async () => {
      try {
        const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
        const response = await fetch(`${baseUrl}/api/Notes/${noteId}`);
        if (!response.ok) {
          throw new Error('Ошибка при загрузке заметки');
        }
        const data = await response.json();
        setNote(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchNote();
  }, [noteId]);

  const openImageModal = (src) => {
    setModalImage(src);
  };

  const closeImageModal = () => {
    setModalImage(null);
  };

  const handleDeleteNote = async () => {
    if (window.confirm("Вы уверены, что хотите удалить заметку?")) {
      try {
        await deleteNote(noteId);
        navigate('/');
      } catch (error) {
        console.error(error);
        alert(error.message);
      }
    }
  };

  if (loading) return <div>Загрузка...</div>;
  if (error) return <div>Ошибка: {error}</div>;
  if (!note) return <div>Заметка не найдена</div>;

  return (
    <>
      <Header showBack={true} />
      <div className="note-details-container">
        <div className="note-header">
          <div className="note-title-section">
            <h1 className="note-title">{note.title}</h1>
            <span className="note-date">{formatDate(note.dateChange)}</span>
          </div>
          <div className="note-header-actions">
            <button 
              className="edit-btn" 
              onClick={() => navigate(`/note/edit/${note.uid}`)}
            >
              ✏️
            </button>
            <button 
              className="delete-btn" 
              onClick={handleDeleteNote}
            >
              ❌
            </button>
          </div>
        </div>
        <div className="note-content">
          <div className="note-fulltext-panel">
            <p>{note.text}</p>
          </div>
          <div className="note-images-grid">
            {note.photos && note.photos.map((image) => (
              <div className="image-wrapper" key={image.uid}>
                <img
                  src={image.url}
                  alt=""
                  onClick={() => openImageModal(image.url)}
                />
              </div>
            ))}
          </div>
        </div>
      </div>
      <ModalImage src={modalImage} alt="Просмотр изображения" onClose={closeImageModal} />
    </>
  );
};

export default NoteDetails;
