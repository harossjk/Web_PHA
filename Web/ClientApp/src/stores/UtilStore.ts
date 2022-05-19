import axios from 'axios';
import { makeObservable, observable, action, computed, toJS, runInAction } from 'mobx';
import { dateToString } from '../utils/Utils';
import { baseURL } from '../utils/Utils';
import FileSaver from 'file-saver';

export const urls = {
  export: `${baseURL}/api/export`,
  download: `${baseURL}/api/export/download`,
};

export default class UtilStore {
  //0228 seo
  RawData: any[] = [];
  TempInquiry: any = {};

  exportClicked: boolean = false;

  constructor() {
    makeObservable(this, {
      RawData: observable,
      TempInquiry: observable,
      exportClicked: observable,

      setExportExelData: action,
      setExportBtnClick: action,

      getExportBtnClick: computed,
    });
  }

  setExportExelData = async (data: any) => {
    // console.log('프론트에서 서버로 올리는 값', data);

    const stringStartDt = dateToString(data.startDt, 'yyyy-MM-DD');
    const strinEndDt = dateToString(data.endDt, 'yyyy-MM-DD');

    try {
      window.location.assign(
        `${urls.download}?rfid=${data.rfid}&moldName=${data.moldName}&workType=${data.workType}&startDt=${stringStartDt}&endDt=${strinEndDt}&pageName=${data.pageName}`,
      );
    } catch (err) {
      console.log(err);
    }
  };

  setExportBtnClick = (boolean: boolean) => {
    return runInAction(() => {
      this.exportClicked = boolean;
    });
  };

  get getExportBtnClick() {
    return this.exportClicked;
  }
}
