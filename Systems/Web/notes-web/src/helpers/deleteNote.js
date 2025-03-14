// src/helpers/deleteNote.js

// Helper function to delete a note by its ID.
// Sends a DELETE request to the server API and throws an error if the deletion fails.
export const deleteNote = async (noteId) => {
  const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
  const response = await fetch(`${baseUrl}/api/Notes/${noteId}`, {
    method: 'DELETE',
  });
  if (response.status !== 204) {
    throw new Error('Ошибка при удалении заметки');
  }
};
