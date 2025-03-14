// src/components/AddButton/AddButton.jsx

// Import React to use JSX and create components
import React from 'react';
// Import CSS styles for the AddButton component
import './AddButton.css';

/**
 * AddButton Component
 * --------------------
 * This functional component renders a button with a "+" sign.
 * It accepts an onClick prop, which is a function to be executed when the button is clicked.
 */
const AddButton = ({ onClick }) => {
  return (
    // Render a button element with the "add-btn" class and attach the onClick event handler
    <button className="add-btn" onClick={onClick}>
      +
    </button>
  );
};

// Export the AddButton component as the default export of this module
export default AddButton;
