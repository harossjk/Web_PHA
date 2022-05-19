import { toJS } from 'mobx';
import React, { useState } from 'react';
import myStyle from './Alarm.module.scss'
import moreDot from '../../../asset/img/more.svg'

interface props {
  allErrorData: any[];
  handleClickedErrorInfo: (index: number) => void;
}

const Alarm = ({ allErrorData, handleClickedErrorInfo }: props) => {

  const [myIndex, setMyIndex] = useState<number | boolean>();

  const handelMouseOver = (idx: number) => {
    setMyIndex(idx);
  }

  const handleMouseLeave = (boolean: boolean) => {
    setMyIndex(boolean);
  }
  return (
    <div className={myStyle.container}>
      <div className={myStyle.list}>
        {allErrorData.length !== 0 ?
          allErrorData.map((el, idx) => {
            return (
              <div key={idx} onClick={() => handleClickedErrorInfo(idx)} onMouseOver={() => handelMouseOver(idx)} onMouseLeave={() => handleMouseLeave(false)} className={myIndex === idx ? myStyle.itemhover : myStyle.item}>
                <div className={myStyle.leftLine} />
                <div className={myStyle.contents}>
                  <div className={myStyle.title}>{`Collected_Alarm : ${el.dataType} ${el.customMessage} `}</div>
                  <div className={myStyle.time}>{el.customCollectInfo}</div>
                </div>
                {/* <img src={moreDot} alt='' className={myStyle.moreDot} /> */}
              </div>
            )
          })
          : <div className={myStyle.nonValue}>금일 알람 없음</div>
        }
      </div>
    </div>
  )
};

export default Alarm;
