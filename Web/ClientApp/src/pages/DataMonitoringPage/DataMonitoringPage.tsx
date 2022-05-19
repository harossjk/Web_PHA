import { observer } from 'mobx-react';
import React, { useEffect, useMemo, useState } from 'react'
import MonitoringCycleGraph from '../../components/DataMonitoring/MonitoringCycleGraph';
import { toJS } from 'mobx';
import useStore from '../../stores';
import InjectionMachine from '../../components/DataMonitoring/InjectionMachineList';
import myStyle from './DataMonitoringPage.module.scss'
import PressureIntegralGraph from '../../components/DataMonitoring/PressureIntegralGraph';
import _ from 'lodash';
import ChannelSetting from '../../components/DataMonitoring/ChannelSetting';
import GraphInfo from '../../components/DataMonitoring/GraphInfo';
import Alarm from '../../components/DataMonitoring/Alarm';
import AlarmModal from '../../components/DataMonitoring/AlarmModal';

const DataMonitoringPage = () => {

  const { MSMDStore } = useStore();
  let monitoringData = MSMDStore.getMonitoringData;
  // let settingData = MSMDStore.getMonitoringData;
  const moldInfo = MSMDStore.getMoldInfo;
  const piData = MSMDStore.getPIData;
  const newGraphInfo = MSMDStore.getGraphInfo;
  const StatusArray = MSMDStore.getStatusArray;
  const initCount = MSMDStore.getInitCount;
  const cycleDetaildata = MSMDStore.getDetailData;
  const allErrorData = MSMDStore.getAllError;

  const [update, setUpdate] = useState(true);
  const [renderingDataNum, setRenderingDataNum] = useState(initCount); // 0~8번 
  const [setting, setSetting] = useState(false);
  const [alarmModalOpen, setAlarmModalOpen] = useState(false);
  const [alarmModalData, setAlarmModalData] = useState<any>();
  const [cnt, setCnt] = useState(1)

  useEffect(() => {
    if (!setting) {

      MSMDStore.initStart(
        renderingDataNum !== undefined && renderingDataNum !== -1
          ? renderingDataNum
          : initCount !== -1
            ? initCount
            : 0
      );

      if (StatusArray.length !== 0 && cnt >= StatusArray.length) { //한바퀴 돌았을때
        const timeout = renderingControl(StatusArray.length === 0 ? cnt : StatusArray[0]);
        setCnt(1);
        return () => {
          clearTimeout(timeout);
        };
      }

      if (StatusArray.length !== 0) {
        setCnt(cnt + 1);
        const timeout = renderingControl(StatusArray[cnt]);
        return () => {
          clearTimeout(timeout);
        };
      }

      if (StatusArray.length === 0) {
        // console.log("처음 랜더링시 또는 모든 사출기가 계획휴지시");
        // const timeout = renderingControl(StatusArray.length !== 0 ? StatusArray[cnt] : initCount);
        const timeout = renderingControl(0);
        // setRenderingDataNum(0);
        return () => {
          clearTimeout(timeout);
        };
      }
    }

  }, [update, setting]);


  //사출기 클릭이벤트
  const ClickedList = (data: any) => {
    setRenderingDataNum(data)
    setUpdate(!update);
  }

  //그래프 재랜더링 컨드롤
  const renderingControl = (cnt: number) => {
    // console.log("랜더링할 값:", cnt);

    if (setting) {
      return setTimeout(() => {
      }, 25000);
    }
    return setTimeout(() => {
      setRenderingDataNum(cnt === undefined ? StatusArray[0] : cnt);
      setUpdate(!update);
    }, 25000);
  }

  //설정 클릭이벤트
  const clickedSetting = (boolean: boolean) => {
    setSetting(boolean)
  }

  // 배열1 : monitoringData , 배열2 : newGraphInfo[0]
  // setting 시 새 그래프를 그려서 내려줌.  0126 seo
  if (newGraphInfo.length !== 0 && renderingDataNum === newGraphInfo[1]) {

    let monitoringDataStr = JSON.stringify(monitoringData[0].data);
    let dupObject = newGraphInfo[0].filter((x: any) => {
      return monitoringDataStr.includes(JSON.stringify(x))
    });
    let props = dupObject;
    let resultArray = monitoringData[0].data.filter((key1: any) => {
      return newGraphInfo[0].some((key2: any) => {
        return key1.id === key2.id;
      });
    }).map((sameObj: any) => {
      return props.reduce((newObj: any, value: any) => {
        newObj[value] = sameObj[value];
        return newObj;
      }, {});
    });

    monitoringData[0].data = resultArray;
  }


  const handleClickedErrorInfo = (index: number) => {
    setAlarmModalOpen(true)
    allErrorData.map((el, idx) => {
      if (index === idx) setAlarmModalData(el);
    })
  }

  console.log("alarmModalData", toJS(alarmModalData));

  return (

    <>
      <div className={myStyle.monitoringPageContainer}>
        {alarmModalOpen && alarmModalData !== undefined &&
          <div className={myStyle.modalBackgroud}>
            <div className={myStyle.modal}>
              <div className={myStyle.modalContents}>
                <div className={myStyle.modalContents}>이상발생 : {alarmModalData.collectDt}</div>
                <div className={myStyle.modalContents}>사출기: {alarmModalData.machineId}</div>
                <div className={myStyle.modalContents}>금형명 : {alarmModalData.moldName}</div>
                <div className={myStyle.modalContents}>RFID : {alarmModalData.rfid}</div>
                <div className={myStyle.modalContents}>채널 : {alarmModalData.channel}</div>
                <div className={myStyle.modalContents}>상세내용 : {alarmModalData.dataType}(이)가 {alarmModalData.message}</div>
                <div className={myStyle.modalContents}>Value : {alarmModalData.value}</div>
                <div className={myStyle.modalContents}>Limit Value : {alarmModalData.limitValue}</div>
              </div>
              <div onClick={() => setAlarmModalOpen(false)} className={myStyle.modalClose}>닫기</div>
            </div>
          </div>
        }
        <div className={myStyle.graphContainer}>
          <div className={myStyle.contentsLeft}>
            <div className={myStyle.contentsTitle}>온도 & 압력</div>
            <div className={myStyle.contentsLeftTop}>

              <div className={myStyle.contentsLeft_Left}>
                {
                  <MonitoringCycleGraph
                    monitoringData={monitoringData[0]}
                    handleClicked={clickedSetting}
                    selectMold={renderingDataNum}
                  />
                }
              </div>


              <div className={myStyle.contentsLeft_Right}>
                {
                  <GraphInfo
                    monitoringData={monitoringData.length !== 0 ? monitoringData[0] : false}
                    cycleDetaildata={cycleDetaildata !== undefined && cycleDetaildata}
                    moldInfo={moldInfo}
                    renderingDataNum={renderingDataNum}

                  />
                }
              </div>
            </div>
          </div>
          <div className={myStyle.contentsRights}>
            <div className={myStyle.contentsRight_top}>
              <div className={myStyle.contentsTitle}>압력적분 - Cycle</div>
              <div className={myStyle.contentsRightTop}>
                {piData.length !== 0 && <PressureIntegralGraph cycleDetaildata={cycleDetaildata !== undefined && cycleDetaildata} piData={piData} monitoringData={monitoringData[0]} selectMold={renderingDataNum} />}
              </div>
            </div>
            <div className={myStyle.contentsRight_bottom}>
              <div className={myStyle.contentsTitle}>알람 현황</div>
              <Alarm allErrorData={allErrorData} handleClickedErrorInfo={handleClickedErrorInfo} />
            </div>
          </div>
        </div>
        <InjectionMachine handleClicked={ClickedList} moldInfo={moldInfo} selectMold={renderingDataNum} monitoringData={monitoringData} />

      </div>

    </>

  )
}

export default observer(DataMonitoringPage)
