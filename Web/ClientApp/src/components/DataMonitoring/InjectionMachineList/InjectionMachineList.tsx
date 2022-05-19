import React from 'react';
import myStyle from './InjectionMachineList.module.scss'
import { BiRectangle } from 'react-icons/bi'
import injection_gray from '../../../asset/img/injection_gray.svg';
import injection_blue from '../../../asset/img/injection_blue.svg';
import injection_yellow from '../../../asset/img/injection_yellow.svg';
import injection_purple from '../../../asset/img/injection_purple.svg';
import injection_green from '../../../asset/img/injection_green.svg';
import injection_red from '../../../asset/img/injection_red.svg';
import { toJS } from 'mobx';

interface props {
  handleClicked: (data: number) => void;
  moldInfo: any[];
  selectMold: number;
  monitoringData: any[];
}

const InjectionMachineList = ({ handleClicked, moldInfo, selectMold, monitoringData }: props) => {

  // console.log("selectMold", toJS(selectMold));
  // console.log("monitoringData", toJS(monitoringData));
  // console.log("moldInfo", toJS(moldInfo));



  return (
    <div className={myStyle.container}>
      <div className={myStyle.header}>
        <span>사출기</span>
      </div>
      <div className={myStyle.contents}>
        {
          moldInfo !== undefined && moldInfo.map((el, idx) => {
            return (
              <div key={idx} className={
                selectMold === 0 && selectMold === idx
                  ? myStyle.selectedItem
                  : selectMold === idx
                    ? myStyle.selectedItem
                    : myStyle.nonSelectedItem
              } onClick={() => handleClicked(idx)}>
                {(() => {
                  switch (el.viewStatus) {
                    case 'Disconnect':
                      return <img src={injection_gray} alt='' className={myStyle.injectionIcon} />
                    case 'Preheating':
                      return <img src={injection_blue} alt='' className={myStyle.injectionIcon} />
                    case 'Wait':
                      return <img src={injection_yellow} alt='' className={myStyle.injectionIcon} />
                    case 'Changemold':
                      return <img src={injection_purple} alt='' className={myStyle.injectionIcon} />
                    case 'Auto':
                      return <img src={injection_green} alt='' className={myStyle.injectionIcon} />
                    case 'Alarm':
                      return <img src={injection_red} alt='' className={myStyle.injectionIcon} />
                    case 'Manual':
                      return <img src={injection_yellow} alt='' className={myStyle.injectionIcon} />
                    default:
                      return null
                  }
                })()}
                <div key={idx} className={myStyle.value}>

                  {/* <span>{el.machineName}</span><span>&ensp;{el.curMoldfileName}</span>
                  {el.sensorIsActive === 'false' && <span>&ensp;[수집대기]</span>}
                  {el.sensorIsActive === 'true' && <span>&ensp;[수집중]</span>}
                  {el.viewStatus === 'Manual' && el.machineName !== '비가동' && <span>&ensp;[수동 운전]</span>} */}

                  <span>{el.machineName}</span>
                  <br/>
                  <span>{el.curMoldfileName}</span>
                </div>
              </div>
            )
          })
        }
      </div>
      <div className={myStyle.injectionType}>
        <div >
          <span><BiRectangle className={myStyle.icon1} />계획휴지</span>
          <span><BiRectangle className={myStyle.icon2} />설비예열</span>
          <span><BiRectangle className={myStyle.icon3} />작업준비</span>
          <span><BiRectangle className={myStyle.icon4} />금형교체</span>
          <span><BiRectangle className={myStyle.icon5} />자동</span>
          <span><BiRectangle className={myStyle.icon6} />이상</span>
        </div>
      </div>
    </div>
  )
};

export default InjectionMachineList;

/*사출기 상태
            viewStatus "Disconnect" : 계획휴지
            viewStatus "Wait"       : 작업준비
            viewStatus "Auto"       : 자동운전 
            viewStatus "Alarm"      : 알람(이상)
            viewStatus "Preheating" : 설비예열
            viewStatus "Changemold" : 금형교체
          */