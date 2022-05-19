import React from 'react';
import { Link, Route, Routes } from 'react-router-dom';
import myStyle from './Body.module.scss';
import Sidebar from '../Sidebar';
import MainContents from '../MainContents';

const Body = () => {
  return (
    <div className={myStyle.BodyLayout}>
      <div className={myStyle.Sidebar}>
        <Sidebar />
      </div>
      <div className={myStyle.Contents}>
        <MainContents />
      </div>
    </div>
  );
};

export default Body;
