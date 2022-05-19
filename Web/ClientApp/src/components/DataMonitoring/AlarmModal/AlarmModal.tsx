import { toJS } from 'mobx';
import React, { useState } from 'react';
import myStyle from './AlarmModal.module.scss'
import moreDot from '../../../asset/img/more.svg'

interface props {
  alarmModalData: any;
  alarmModalClose: (boolean: boolean) => void;
}

const AlarmModal = ({ alarmModalData, alarmModalClose }: props) => {

  return (
    <div className={myStyle.modalBackgroud}>
      <div className={myStyle.modal}>
        <div className={myStyle.modalContents}>
          <div className={myStyle.modalContents}>이상발생 : {alarmModalData.collectDt}</div>
          <div className={myStyle.modalContents}>사출기: {alarmModalData.machineId}</div>
          <div className={myStyle.modalContents}>금형명 : {alarmModalData.moldName}</div>
          <div className={myStyle.modalContents}>RFID : {alarmModalData.rfid}</div>
          <div className={myStyle.modalContents}>상세내용 : {alarmModalData.message}</div>
        </div>
        <div onClick={() => alarmModalClose(false)} className={myStyle.modalClose}>닫기</div>
      </div>
    </div>
  )
};

export default AlarmModal;
