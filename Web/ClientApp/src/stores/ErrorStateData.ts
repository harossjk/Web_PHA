import axios from 'axios';
import { makeObservable, observable, action, computed, toJS, runInAction } from 'mobx';
import { configure } from 'mobx';
import { IMoldInfo } from '../interface/IDataMonitoring';
import { baseURL } from '../utils/Utils';

export const urls = {
  //   inpSearch: `${baseURL}/api/ErrorState/getInpSearch`,
  allError: `${baseURL}/api/ErrorState/getErrSearch`,
};

export default class ErrorStateData {
  inquiryError: Object = {};
  TempInquiry: any = {};

  constructor() {
    makeObservable(this, {
      inquiryError: observable,
      TempInquiry: observable,

      downinquiryErrorData: action,
      setTempInquiry: action,

      getInquiryError: computed,
      getTempInquiry: computed,
    });
  }

  downinquiryErrorData = async (inquiryData: object) => {
    try {
      const response = await axios.post(urls.allError, inquiryData);

      return runInAction(() => {
        console.log('response.data', response.data);
        if (Object.keys(response.data).length === 0) alert('조회데이터가 없습니다.');
        this.inquiryError = response.data;
      });
    } catch (err) {
      console.log(err);
      return runInAction(() => {
        this.inquiryError = [];
      });
    }
  };

  setTempInquiry = (data: any = {}) => {
    return runInAction(() => {
      this.TempInquiry = data;
    });
  };

  get getInquiryError() {
    return this.inquiryError;
  }
  get getTempInquiry() {
    return this.TempInquiry;
  }
}
