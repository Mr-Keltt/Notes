// src/index.jsx

import React from 'react';
import { createRoot } from 'react-dom/client';
import App from './App';

// Get the root element from the DOM and create a React root
const container = document.getElementById('root');
const root = createRoot(container);

// Render the main App component into the root element
root.render(<App />);
