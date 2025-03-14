// ModalImage Component
// Displays an image in a modal overlay with a close button.

import React from 'react';
import './ModalImage.css';

const ModalImage = ({ src, alt, onClose }) => {
  // Return nothing if no source is provided
  if (!src) return null;
  return (
    // Overlay that closes the modal when clicked
    <div className="modal-overlay" onClick={onClose}>
      {/* Content container that prevents click events from propagating */}
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        {/* Button to close the modal */}
        <button className="modal-close" onClick={onClose}>✖</button>
        {/* Render the image */}
        <img src={src} alt={alt} className="modal-image" />
      </div>
    </div>
  );
};

export default ModalImage;
