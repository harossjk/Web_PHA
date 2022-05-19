import axios from 'axios';
import { makeObservable, observable, action, computed, toJS, runInAction } from 'mobx';
import { dateToString } from '../utils/Utils';
import { baseURL } from '../utils/Utils';

export const urls = {
  rawDataSearch: `${baseURL}/api/rowdata/search`,
};

export default class RawData {
  //0228 seo
  RawData: any[] = [];
  TempInquiry: any = {};

  constructor() {
    makeObservable(this, {
      RawData: observable,
      TempInquiry: observable,

      downRawData: action,
      setTempInquiry: action,

      getRawData: computed,
      getTempInquiry: computed,
    });
  }

  downRawData = async (data: any, chName?: string) => {
    try {
      const response = await axios.post(urls.rawDataSearch, {
        // machineType: data.machineType, //영어
        // machineId: data.machineId, // 16, 26
        workType: data.workType, // day, night, all
        rfid: data.rfid,
        startDt: data.startDt,
        endDt: data.endDt,
        // chName: chName,
      });
      const modifiedArray = response.data.map((el: any, idx: number) => {
        if (el.MachineType === 'Horizontal') return { ...el, MachineType: '수평', MachineId: `${el.MachineId}호기` };
        if (el.MachineType === 'Vertical') return { ...el, MachineType: '수직', MachineId: `${el.MachineId}호기` };
      });

      return runInAction(() => {
        this.RawData = modifiedArray;
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

  get getRawData() {
    return this.RawData;
  }
  get getTempInquiry() {
    return this.TempInquiry;
  }
}
