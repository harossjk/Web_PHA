import axios from 'axios';
import { makeObservable, observable, action, computed, toJS } from 'mobx';
import { configure } from 'mobx';
import { IMoldInfo } from '../interface/IDataMonitoring';
import { baseURL } from '../utils/Utils';

export const urls = {
  // selectMonitoringData: `${baseURL}/api/datamonitor/selectone`, //그래프형식
  // integralPressure: `${baseURL}/api/datamonitor/getInp`, //그래프형식
  // dataMonitoringForTable: `${baseURL}/dataMonitoringForTable`, //테이블형식
  // moldData: `${baseURL}/api/datamonitor/molddata`, //테이블형식
  // colorUpdate: `${baseURL}/api/datamonitor/`, //테이블형식
  // detailData: `${baseURL}/api/datamonitor/getdetail`, //테이블형식
  inpSearch: `${baseURL}/api/processState/getInpSearch`,
  allRfid: `${baseURL}/api/processState`,
};

export default class MoldInfoData {
  //0228 seo
  MoldName: any[] = [];
  MoldInfo: any[] = [];
  InquiryInfo: any[] = [];
  SelectOneMachineInfo: any = {};

  constructor() {
    makeObservable(this, {
      InquiryInfo: observable,
      MoldName: observable,
      SelectOneMachineInfo: observable,

      downMoldNameData: action,
      downInquiryData: action,
      downSelectMachineData: action,
      downinquiryFirstData: action,

      getInquiryData: computed,
      getMoldName: computed,
      getMoldInfo: computed,
      getSelectOneMachineInfo: computed,
    });
  }

  downMoldNameData = async () => {
    let tempArr: any[] = [];
    const response = await axios.get(urls.allRfid);
    response.data.map((el: any) => {
      tempArr.push(`[${el.rfid}] ${el.moldName}`);
    });
    this.MoldInfo = response.data;
    return (this.MoldName = tempArr);
  };
  downInquiryData = async () => {};

  // 조회버튼 클릭시 서버로 보내는 데이터, 처음으로 랜더링되는 response값
  downinquiryFirstData = async (data: any, chName = 'P1') => {
    console.log('조회데이터 machineId[0]', data.machineId[0]);
    console.log('조회할데이터 ', data);

    // const newData = {
    //   machineType: data.machineType, //영어
    //   machineId: data.machineId[0], // 16, 26
    //   workType: data.workType, // day, night, all
    //   moldName: data.moldName,
    //   startDt: data.startDt,
    //   endDt: data.endDt,
    //   chName: chName,
    // };

    // if (newData.machineId === '16') return (this.SelectOneMachineInfo = Top5_Data);
    // if (newData.machineId === '26') return (this.SelectOneMachineInfo = Top5_Data2);
    try {
      const response = await axios.post(urls.inpSearch, {
        machineType: data.machineType, //영어
        machineId: data.machineId, // 16, 26
        workType: data.workType, // day, night, all
        moldName: data.moldName,
        startDt: data.startDt,
        endDt: data.endDt,
        // chName: chName,
      });

      // calIncuiryData(response.data, chName = 'P1');

      return (this.InquiryInfo = response.data);
    } catch (err) {
      console.log(err);
    }
  };

  // 각각 조회사출기 클릭시 서버호출, 재랜더링
  downSelectMachineData = async (data: any) => {
    console.log('조회데이터 machineId[0]', data.machineId[0]);

    //서버로부터 요청받아야할 top5값;
    if (data.machineId === '16') return (this.SelectOneMachineInfo = Top5_Data);
    if (data.machineId === '26') return (this.SelectOneMachineInfo = Top5_Data2);

    // try {
    //   const response = await axios.get(urls.selectOneData, {data});
    //   return response.data;
    // } catch (err) {
    //   console.log(err);
    // }
  };

  get getInquiryData() {
    return this.InquiryInfo;
  }

  get getSelectOneMachineInfo() {
    return this.SelectOneMachineInfo;
  }

  get getMoldName() {
    return this.MoldName;
  }

  get getMoldInfo() {
    return this.MoldInfo;
  }
}

const moldName = ['금형_A', '금형_B', '금형_C', '금형_D', '금형_E'];

const Top5_Data = {
  upperTop5: [
    { 사이클번호: 1, 오차율: '10%', 압력적분값: 40, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 222, 오차율: '10%', 압력적분값: 44, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 3, 오차율: '10%', 압력적분값: 41, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 23, 오차율: '10%', 압력적분값: 43, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 43, 오차율: '10%', 압력적분값: 37, 기준값: 40, cp: 7.6, cpk: 13.2 },
  ],
  lowerTop5: [
    { 사이클번호: 1, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 222, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 3, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 23, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 43, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
  ],
};

const Top5_Data2 = {
  upperTop5: [
    { 사이클번호: 12, 오차율: '10%', 압력적분값: 40, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 33, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 13, 오차율: '10%', 압력적분값: 41, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 223, 오차율: '10%', 압력적분값: 43, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 343, 오차율: '10%', 압력적분값: 44, 기준값: 40, cp: 7.6, cpk: 13.2 },
  ],
  lowerTop5: [
    { 사이클번호: 31, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 2222, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 43, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 223, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
    { 사이클번호: 243, 오차율: '10%', 압력적분값: 35, 기준값: 40, cp: 7.6, cpk: 13.2 },
  ],
};
