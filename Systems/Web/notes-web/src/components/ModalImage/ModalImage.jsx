import React from 'react';
import './ModalImage.css';

const ModalImage = ({ src, alt, onClose }) => {
  if (!src) return null;
  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={e => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>✖</button>
        <img src={src} alt={alt} className="modal-image" />
      </div>
    </div>
  );
};

export default ModalImage;
