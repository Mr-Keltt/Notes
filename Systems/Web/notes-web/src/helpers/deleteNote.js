// src/helpers/deleteNote.js
export const deleteNote = async (noteId) => {
  const baseUrl = process.env.Main__PublicUrl || 'http://localhost:10000';
  const response = await fetch(`${baseUrl}/api/Notes/${noteId}`, {
    method: 'DELETE',
  });
  if (response.status !== 204) {
    throw new Error('Ошибка при удалении заметки');
  }
};
  