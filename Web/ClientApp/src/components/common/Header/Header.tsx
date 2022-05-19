import React from 'react';
import { Link } from 'react-router-dom';
import myStyle from './Header.module.scss';
import logo from '../../../asset/img/phc.png';

const Header = () => {
  return (
    <>
      <div className={myStyle.titleimg}><img src={logo} alt="no img"></img></div>
      <div className={myStyle.title}>
        <div className={myStyle.top}>Monitoring System</div>
        <div className={myStyle.bottom}>금형센서 & 위치 모니터링</div>
      </div>
    </>
  );
};

export default Header;
