import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter } from 'react-router-dom';
import Layout from './pages/layout';


ReactDOM.render(

  <BrowserRouter>
    <Layout />
  </BrowserRouter>,

  document.getElementById('root'),
);

reportWebVitals();
