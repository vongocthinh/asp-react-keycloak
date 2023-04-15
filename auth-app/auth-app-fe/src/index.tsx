import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import KeyCloakService from './services/KeycloakService';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

const renderApp = () =>
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  )

KeyCloakService.CallLogin(renderApp);
