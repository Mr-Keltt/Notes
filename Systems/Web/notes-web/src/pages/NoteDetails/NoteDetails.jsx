// src/pages/NoteDetails/NoteDetails.jsx

import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import ModalImage from '../../components/ModalImage/ModalImage';
import { deleteNote } from '../../helpers/deleteNote';
import './NoteDetails.css';

// Helper function to format a date string as dd.mm.yyyy
const formatDate = (dateString) => {
  const date = new Date(dateString);
  const dd = String(date.getDate()).padStart(2, '0');
  const mm = String(date.getMonth() + 1).padStart(2, '0');
  const yyyy = date.getFullYear();
  return `${dd}.${mm}.${yyyy}`;
};

/**
 * NoteDetails Component
 * Displays the details of a single note including title, text, images, and actions to edit or delete the note.
 */
const NoteDetails = () => {
  const { noteId } = useParams();
  const navigate = useNavigate();

  // Local state for note data, loading, error, and image modal
  const [note, setNote] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [modalImage, setModalImage] = useState(null);

  // Fetch note details when noteId changes
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

  // Open the modal to view an image
  const openImageModal = (src) => {
    setModalImage(src);
  };

  // Close the image modal
  const closeImageModal = () => {
    setModalImage(null);
  };

  // Delete the note and navigate back to home after confirmation
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
      {/* Header with back button */}
      <Header showBack={true} />
      <div className="note-details-container">
        <div className="note-header">
          <div className="note-title-section">
            <h1 className="note-title">{note.title}</h1>
            <span className="note-date">{formatDate(note.dateChange)}</span>
          </div>
          <div className="note-header-actions">
            {/* Button to edit the note */}
            <button 
              className="edit-btn" 
              onClick={() => navigate(`/note/edit/${note.uid}`)}
            >
              ✏️
            </button>
            {/* Button to delete the note */}
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
      {/* Modal for displaying the clicked image */}
      <ModalImage src={modalImage} alt="Просмотр изображения" onClose={closeImageModal} />
    </>
  );
};

export default NoteDetails;
