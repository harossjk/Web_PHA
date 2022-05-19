import axios from 'axios';
import { makeObservable, observable, action, computed, toJS, runInAction } from 'mobx';
import { configure } from 'mobx';
import { IMoldInfo } from '../interface/IDataMonitoring';
import { calIncuiryData2 } from '../utils/Utils';
import { baseURL } from '../utils/Utils';

export const urls = {
  inpSearch: `${baseURL}/api/processState/getInpSearch`,
  allRfid: `${baseURL}/api/processState`,
};

export default class InquiryData {
  //0228 seo
  MoldName: any[] = [];
  MoldInfo: any[] = [];
  InquiryInfo: object = {};
  resultDataInfo: object = {};
  SelectOneMachineInfo: any = {};
  TempInquiry: any = {};
  DefaultChannel: string = '';

  constructor() {
    makeObservable(this, {
      resultDataInfo: observable,
      InquiryInfo: observable,
      MoldName: observable,
      MoldInfo: observable,
      SelectOneMachineInfo: observable,
      TempInquiry: observable,
      DefaultChannel: observable,

      downMoldNameData: action,
      downInquiryData: action,
      downinquiryFirstData: action,
      setChangeChName: action,
      setTempInquiry: action,

      getResultDataInfo: computed,
      getInquiryData: computed,
      getMoldName: computed,
      getMoldInfo: computed,
      getSelectOneMachineInfo: computed,
      getTempInquiry: computed,
    });
  }

  downMoldNameData = async () => {
    let tempArr: any[] = [];
    const response = await axios.get(urls.allRfid);
    response.data.map((el: any) => {
      tempArr.push(`[${el.rfid}] ${el.moldName}`);
    });
    this.MoldInfo = response.data;
    return runInAction(() => {
      this.MoldName = tempArr;
    });
  };

  downInquiryData = async () => {};

  // 조회버튼 클릭시 서버로 보내는 데이터, 처음으로 랜더링되는 response값
  downinquiryFirstData = async (data: any, chName: string) => {
    console.log('프론트에서 서버로 보내는 정보', data);
    try {
      const response = await axios.post(urls.inpSearch, {
        // machineType: data.machineType, //영어
        // machineId: data.machineId, // 16, 26
        workType: data.workType, // day, night, all
        rfid: data.rfid,
        startDt: data.startDt,
        endDt: data.endDt,
        // chName: chName,
      });
      console.log('서버에서 프론트로 돌려받은 값', response.data);
      this.resultDataInfo = response.data;
      const channel = Object.keys(response.data.RfidItemList[0].ChannelItemDic)[0];
      const returnCalValue = calIncuiryData2(response.data, channel);
      return runInAction(() => {
        this.InquiryInfo = returnCalValue;
        this.DefaultChannel = channel;
      });
    } catch (err) {
      alert('조회데이터가 없습니다.');
      return runInAction(() => {
        this.InquiryInfo = { err: err };
      });
    }
  };

  setChangeChName = (chName: string) => {
    try {
      const returnCalValue = calIncuiryData2(this.resultDataInfo, chName);

      return runInAction(() => {
        this.InquiryInfo = returnCalValue;
      });
    } catch (err) {
      console.log(err);
    }
  };

  setTempInquiry = (data: any = {}) => {
    return runInAction(() => {
      this.TempInquiry = data;
    });
  };

  get getResultDataInfo() {
    return this.resultDataInfo;
  }

  get getDefaultChannel() {
    return this.DefaultChannel;
  }
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
  get getTempInquiry() {
    return this.TempInquiry;
  }
}
