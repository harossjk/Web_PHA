import axios from 'axios';
import { log } from 'console';
import { makeObservable, observable, action, computed, toJS, runInAction } from 'mobx';
import { configure } from 'mobx';
import { IMoldInfo } from '../interface/IDataMonitoring';
import { calErrorData, initColor } from '../utils/Utils';
import { baseURL } from '../utils/Utils';
export const urls = {
  selectMonitoringData: `${baseURL}/api/datamonitor/selectone`, //그래프형식
  integralPressure: `${baseURL}/api/datamonitor/getInp`, //그래프형식
  dataMonitoringForTable: `${baseURL}/dataMonitoringForTable`, //테이블형식
  moldData: `${baseURL}/api/datamonitor/molddata`, //테이블형식
  colorUpdate: `${baseURL}/api/datamonitor/`, //테이블형식
  detailData: `${baseURL}/api/datamonitor/getdetail`, //테이블형식
  oneDetailData: `${baseURL}/api/datamonitor/getonedetail`, //테이블형식
  allError: `${baseURL}/api/datamonitor/alarm`, //테이블형식
};

export default class MoldSensorMonitoringData {
  MonitoringData: any[] = [];
  PreMonitoringData: any[] = [];
  PIData: any[] = [];
  MoldInfo: IMoldInfo[] = [
    {
      machineName: '',
      moldName: '',
      machineType: '',
      machineId: '',
      viewStatus: '',
      chName: '',
    },
  ];
  UpdateGraphInfo: any[] = [];
  StatusArray: any[] = [];
  InitCount: number = 0;
  DetailInfo: object = {};
  DetailInfoOneCycle = { Values: [], cycle: '' };
  DetailInfoOneGraph: any[] = [];
  chNamePressureArray: any[] = [];

  //0413 seo
  AllError: any[] = [];

  //0505 seo 압력적분그래프 수정
  chNameCount = 0;

  constructor() {
    makeObservable(this, {
      StatusArray: observable,
      InitCount: observable,
      MoldInfo: observable,
      MonitoringData: observable,
      PIData: observable,
      UpdateGraphInfo: observable,
      DetailInfo: observable,
      DetailInfoOneCycle: observable,
      DetailInfoOneGraph: observable,
      AllError: observable,
      chNamePressureArray: observable,

      downMoldData: action,
      downInitData: action,
      downMonitoringData: action,
      downPIData: action,
      changeGraph: action,
      downCycleInfo: action,
      downCycleInfoOne: action,
      downAllErrorData: action,

      getMonitoringData: computed,
      getMoldInfo: computed,
      getPIData: computed,
      getGraphInfo: computed,
      getInitCount: computed,
      getStatusArray: computed,
      getDetailData: computed,
      getDetailInfoOneCycle: computed,
      getDetailInfoOneGraph: computed,
      getAllError: computed,
      getChNamePressureArray: computed,
    });
  }

  initStart = async (data?: any) => {
    await this.downMoldData();
    await this.downMonitoringData(data);
    await this.downPIData(data);
    await this.downCycleInfo(data);
    await this.downAllErrorData();
  };

  downMoldData = async () => {
    try {
      const response = await axios.get(urls.moldData);

      /*사출기 상태 
        viewStatus "Disconnect" : 계획휴지
        viewStatus "Wait"       : 작업준비
        viewStatus "Auto"       : 자동운전 
        viewStatus "Alarm"      : 알람(이상)
        viewStatus "Preheating" : 설비예열
        viewStatus "Changemold" : 금형교체
      */

      const testArray = [...response.data];
      const injectionListCount = 9; // 사출기 리스트 개수 초기값

      for (let i = response.data.length; i < injectionListCount; i++) {
        testArray.push({
          machineType: 'Horizontal',
          machineName: '비가동',
          viewStatus: 'Disconnect',
        });
      }

      //최초 랜더링 하고싶은 viewStatus 선택 viewStatus별
      runInAction(() => {
        this.InitCount = testArray.findIndex((el: any, idx: any) => {
          return (
            el.viewStatus === 'Auto' ||
            el.viewStatus === 'Alarm' ||
            el.viewStatus === 'Preheating' ||
            el.viewStatus === 'Manual'
            // el.sensorIsActive === 'True'
          );
        });

        //위 조건에서 만족하는 viewStatus가 없을때 sensorIsActive센서 작동여부를 찾음
        if (this.InitCount === -1) {
          this.InitCount = testArray.findIndex((el: any, idx: any) => {
            return el.sensorIsActive === 'True';
          });
        }
        // console.log('this.InitCount', this.InitCount);
      });

      //반복 랜더링 하고싶은 viewStatus 선택 viewStatus별
      runInAction(() => {
        this.StatusArray = testArray
          .map((el: any, idx: any) => {
            if (
              el.viewStatus === 'Auto' ||
              el.viewStatus === 'Alarm' ||
              // el.viewStatus === 'Preheating' ||
              el.viewStatus === 'Manual'
            )
              return idx;
          })
          .filter(function (item: any) {
            return item !== undefined;
          });

        //위 조건에서 만족하는 viewStatus가 없을때 sensorIsActive센서 작동여부를 찾음
        if (this.StatusArray.length === 0) {
          this.StatusArray = testArray
            .map((el: any, idx: any) => {
              if (el.sensorIsActive === 'True') return idx;
            })
            .filter(function (item: any) {
              return item !== undefined;
            });
        }
        // console.log('this.StatusArray.length', this.StatusArray.length);
      });

      return runInAction(() => {
        this.MoldInfo = testArray;
      });
    } catch (err) {
      console.log(err);
    }
  };

  downInitData = async () => {
    runInAction(() => {
      this.InitCount = this.MoldInfo.findIndex((data, idx) => {
        return (
          data.viewStatus === 'Auto' ||
          data.viewStatus === 'Alarm' ||
          // data.viewStatus === 'Preheating' ||
          data.viewStatus === 'Manual'
        );
      });
    });
  };

  downMonitoringData = async (data: any) => {
    // console.log('Data', data);
    // console.log('this.MoldInfo[data]Data', toJS(this.MoldInfo[data]));
    this.chNamePressureArray = [];
    try {
      const response = await axios.get(urls.selectMonitoringData, {
        params: {
          machineType: this.MoldInfo[data].machineType,
          machineId: this.MoldInfo[data].machineId,
        },
      });

      //0516 주석
      // initColor(response.data[0].colorS);
      return runInAction(() => {
        this.MonitoringData = response.data;
        this.PreMonitoringData = response.data;
        const chNameAllArray =
          response.data[0] !== undefined && response.data[0].chName !== null
            ? response.data[0].chName.split(',')
            : [''];

        if (response.data[0] !== undefined && chNameAllArray.length !== 0) {
          this.chNamePressureArray = chNameAllArray.filter((el: any) => {
            return el.charAt(0) === 'P';
          });
        }
      });
    } catch (err) {
      console.log(err);
    }
  };

  downCycleInfo = async (data: any) => {
    try {
      if (this.MoldInfo[data] !== undefined && this.MonitoringData[0]) {
        const response = await axios.get(urls.oneDetailData, {
          params: {
            machineType: this.MonitoringData[0].machineType,
            machineId: this.MonitoringData[0].machineId,
            chName: this.MonitoringData[0].chName,
            cycleNo: this.MonitoringData[0].cycleNo,
          },
        });
        return runInAction(() => {
          // console.log('에러체크response.data', response.data);
          this.DetailInfo = response.data;
        });
      }
      if (this.MonitoringData[0] === undefined) {
        this.DetailInfo = {};
      }
    } catch (err) {
      console.log(err);
    }
  };

  //0307
  downCycleInfoOne = async (data: any) => {
    // console.log('data', toJS(data));

    try {
      const graph_Response = await axios.get(urls.selectMonitoringData, {
        params: {
          machineType: data.machineType,
          machineId: data.machineId,
          cycleNo: data.cycleNo,
        },
      });
      const cycleInfo_Response = await axios.get(urls.oneDetailData, {
        params: {
          machineType: data.machineType,
          machineId: data.machineId,
          cycleNo: data.cycleNo,
        },
      });

      return runInAction(() => {
        this.DetailInfoOneGraph = graph_Response.data;
        this.DetailInfoOneCycle = cycleInfo_Response.data;
      });
    } catch (err) {
      console.log(err);
    }
  };

  //최초는 P1검색,
  downPIData = async (data: any, chName: any = this.chNamePressureArray[0]) => {
    console.log('data', data);
    console.log('chName', chName);

    try {
      if (this.MoldInfo[data] !== undefined && this.MoldInfo[data].machineId !== undefined) {
        const response = await axios.post(urls.integralPressure, {
          machineType: this.MoldInfo[data].machineType,
          machineId: [this.MoldInfo[data].machineId],
          chName: chName,
          top: 20,
        });
        console.log('response.data[0]', response.data[0]);

        return runInAction(() => {
          //0509 seo 압력적분그래프 수정
          if (response.data[0] === undefined && this.chNamePressureArray.length !== 0) {
            this.downPIData(data, this.chNamePressureArray[this.chNameCount + 1]);
          } else if (
            //0516 seo 압력적분그래프 수정
            response.data[0] !== undefined &&
            response.data[0].lowerLimit === '(null)' &&
            response.data[0].upperLimit === '(null)' &&
            this.chNamePressureArray.length !== 0
          )
            this.downPIData(data, this.chNamePressureArray[this.chNameCount + 1]);
          else this.PIData = response.data.length !== 0 && response.data[0];
        });
      }
    } catch (err) {
      console.log(err);
    }
  };

  changeGraph = async (data: any, cnt: number) => {
    this.UpdateGraphInfo = [data, cnt];
  };

  changeColor = async (color: any, monitoringData: any) => {
    try {
      await axios.post(urls.colorUpdate, {
        ...monitoringData,
        colorS: color,
      });
    } catch (err) {
      console.log(err);
    }
  };

  downAllErrorData = async () => {
    // debugger;
    const response = await axios.get(urls.allError);
    const returnCalValue = calErrorData(response.data);
    return runInAction(() => {
      this.AllError = returnCalValue;
    });
  };

  get getMoldInfo() {
    return this.MoldInfo;
  }

  get getMonitoringData() {
    return this.MonitoringData;
  }
  get getPIData() {
    return this.PIData;
  }
  get getGraphInfo() {
    return this.UpdateGraphInfo;
  }
  get getInitCount() {
    return this.InitCount;
  }
  get getStatusArray() {
    return this.StatusArray;
  }
  get getDetailData() {
    return this.DetailInfo;
  }
  get getDetailInfoOneCycle() {
    return this.DetailInfoOneCycle;
  }
  get getDetailInfoOneGraph() {
    return this.DetailInfoOneGraph;
  }
  get getAllError() {
    return this.AllError;
  }
  get getChNamePressureArray() {
    return this.chNamePressureArray;
  }
}
