import { observer } from 'mobx-react';
import React from 'react';
import Body from '../../components/common/Body';
import Header from '../../components/common/Header';
import myStyle from './Layout.module.scss';


const Layout = () => {
  return (
    <>
      <div className={myStyle.layout}>
        <div className={myStyle.header}>
          <Header />
        </div>
        <div className={myStyle.contents}>
          <Body />
        </div>
      </div>
    </>
  );
};

export default observer(Layout);
