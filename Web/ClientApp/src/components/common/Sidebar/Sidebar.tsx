import React, { useEffect, useState } from 'react';
import { Link, Route, Routes, useLocation } from 'react-router-dom';
import myStyle from './Sidebar.module.scss';
import monitor from '../../../asset/img/monitor.svg';
import rawdata from '../../../asset/img/navi_icon_rawdata.svg';
import measure from '../../../asset/img/navi_icon_measure.svg';
import errorImg from '../../../asset/img/navi_icon_err.svg';
import adsClick from '../../../asset/img/ads_click.svg';
import rackGrid from '../../../asset/img/rackGrid.svg';
import circle18 from '../../../asset/img/circle18.svg';
import useStore from '../../../stores';
import { toJS } from 'mobx';
import { observer } from 'mobx-react';

const Sidebar = () => {

  const { RackStore } = useStore();
  const rackList = RackStore.getRackList;
  const [open, setOpen] = useState<boolean>(false);
  const [rackId, setRackId] = useState<string>("");
  const [clickedRackId, setClickedRackId] = useState<any>();
  let location = useLocation();


  useEffect(() => {
    RackStore.downRackListData();
  }, []);

  const handleItemClick = (clickedRackId: string) => {
    setOpen(true)
    setRackId(clickedRackId);
    if (clickedRackId === rackId) {
      setOpen(false);
      setRackId(""); //초기화
    }
  }

  const handleSubRackClicked = (rackId: any, rackData: any) => {
    //store에 clickedRAckId를 넘김
    setClickedRackId(rackId)
    RackStore.setMyRackPosition(rackId);


    //RackFloor 구하기 0408 seo
    if (rackData.length !== 0) {
      rackData.map((el: any) => {
        if (el.barcode.substr(0, 3) === rackId) RackStore.setMyRackFloor(el.floor)
      })
    }
  }

  const handleSideBarClicked = () => {
    //다른 사이드바 리스트 클릭시 초기화
    RackStore.setMyRackPosition("");
  }


  const testFnc = (data: any) => {
    const dupArr: any = data.map((el: any) => {
      return `${el.barcode.split("-")[0]}-${el.barcode.split("-")[1]}`;
    });
    const uniqueArr = dupArr.filter((v: any, i: number, self: any) => self.indexOf(v) === i);

    return uniqueArr.map((el: any, idx: number) => (
      <Link key={idx} to="/viewer/moldLocation" className={clickedRackId === el ? myStyle.clickedContent : myStyle.content} onClick={() => handleSubRackClicked(el, data)}>{el}</Link>
    ))
  }

  return (
    <div className={myStyle.LinkDiv}>
      <div className={myStyle.LinkItem1}>
        <div className={myStyle.header}>
          <div className={myStyle.title}>금형센서 모니터링</div>
        </div>
        <div className={myStyle.contents}>
          <Link to="/viewer/dataMonitoring" className={location.pathname === '/viewer/dataMonitoring' || location.key === "default" ? `${myStyle.content} ${myStyle.clicked}` : myStyle.content} onClick={() => handleSideBarClicked()}>
            <img src={monitor} alt='' className={myStyle.icon} />
            데이터 모니터링
          </Link>
          <Link to="/viewer/processAbility" className={location.pathname === '/viewer/processAbility' ? `${myStyle.content} ${myStyle.clicked}` : myStyle.content} onClick={() => handleSideBarClicked()}>
            <img src={measure} alt='' className={myStyle.icon} />
            공정능력 지수 통계
          </Link>
          <Link to="/viewer/errorStatistic" className={location.pathname === '/viewer/errorStatistic' ? `${myStyle.content} ${myStyle.clicked}` : myStyle.content} onClick={() => handleSideBarClicked()}>
            <img src={errorImg} alt='' className={myStyle.icon} />
            이상 통계
          </Link>
          <Link to="/viewer/rawData" className={location.pathname === '/viewer/rawData' ? `${myStyle.content} ${myStyle.clicked}` : myStyle.content} onClick={() => handleSideBarClicked()}>
            <img src={rawdata} alt='' className={myStyle.icon} />
            Raw Data 이력
          </Link>
        </div>
      </div>
      <div className={myStyle.LinkItem2}>
        <div className={myStyle.header}>
          <div className={myStyle.title}>금형위치 모니터링</div>
        </div>
        <div className={myStyle.contents}>
          <Link to="/viewer/moldLocation" className={location.pathname === '/viewer/moldLocation' ? `${myStyle.content} ${myStyle.clicked}` : myStyle.content} onClick={() => handleSubRackClicked(false, [])}>
            <img src={adsClick} alt='' className={myStyle.icon2} />
            금형 위치 현황
          </Link>
          <div className={open ? `${myStyle.content} ${myStyle.clickedOrange}` : myStyle.content}>  <img src={rackGrid} alt='' className={myStyle.icon2} />보관함 현황</div>

        </div>
        {rackList.length !== 0 && rackList.map((el: any, idx: any) => {
          return (
            <React.Fragment key={idx}>
              {/* <div className={`${myStyle.content} ${myStyle.rack}`} onClick={() => handleItemClick(el.key)}> */}
              <div className={myStyle.RackIconContent} onClick={() => handleItemClick(el.key)}>
                <span>
                  <img src={circle18} alt='' className={myStyle.icon3} />
                  {`${el.key} RACK`}
                </span>
              </div>
              {open && rackId === el.key && el.value.length !== 0 &&
                <div className={`${myStyle.content} ${myStyle.subRack}`}>{testFnc(el.value)}</div>}
            </React.Fragment>
          );
        })}
      </div>
    </div >
  );
};
export default observer(Sidebar);
