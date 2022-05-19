/* eslint-disable no-console */
import axios from 'axios';
import { makeObservable, observable, action, computed, toJS } from 'mobx';
import { configure } from 'mobx';

// 주의해서 써야함..
configure({
  enforceActions: 'never',
});

export interface oeeNuph {
  sfactoryname: string; // 공장명(factoryname)
  slinename: string; // 라인명(linename)
  sprocesskey: string; // 공정명(processkey)
  scartype: string; // 품번(cartype)
  noee: number | string; // OEE(oee)
  ntime: number | string; // 시간가동률(time)
  nperformance: number | string; // 성능가동률(performance)
  nok: number | string; // 양품률(ok)
  iplancount: number; // 계획 수량(plancount)
  itargetcount: number; // 목표 수량(targetcount)
  itotalcount: number; // 생산 수량(totalcount)
  iokcount: number; // 양품 수량(okcount)
  ingcount: number; // 불량 수량(ngcount)
  npercent: string; // 달성률(percent)

  scyclestart: string; // 취출(cyclestart)
  scycleend: string; // 병목(cycleend)
  ilinecriteriact: number; // 기준 C/T(linecriteriact)
  ilinemeanct: number; // 평균 C/T(linemeanct)
  ilinebeforect: number; // 현재 C/T(ilinebeforect)
  ilinemaxct: number; // 최대 C/T(linemaxct)
  ilineminct: number; // 최소 C/T (lineminct)
  ilineuph: number; // 평균 UPH(lineuph)
  ilinemaxuph: number; // 최대 UPH(linemaxuph)
  ilineminuph: number; // 최소 UPH(lineminuph)
  ilinemeanlob: number; // 평균 LOB(linemeanlob)
  ilinemaxlob: number; // 최대 LOB(linemaxlob)
  ilineminlob: number; // 최소 LOB(lineminlob)
  ioperatingtime: number; // 부하시간(operatingime)
  iprocesstime: number; // 가동시간(processtime)
  ierrortime: number; // 이상시간(errortime)
  ierrorcount: number; // 이상건수(errorcount)
  dttime: string; // 업데이트 시간(ttime)
  sshotcount: number; // 라인 타발수
  smold: string;
}
const TS1stbaseURL = 'http://tsmes.iptime.org:4003';
const baseURL = TS1stbaseURL;

export const urls = {
  master: `${baseURL}/masterinfo`,
  oee: `${baseURL}/oee`,
  uph: `${baseURL}/uph`,
  oeeNuph: `${baseURL}/oeeNuph`,
  factory: `${baseURL}/realtimecustom`,
  error: `${baseURL}/realtimeerror`,
  monthSummary: `${baseURL}/monthSummary`,
  eqpStatus: `${baseURL}/viewer/eqpstatus`,
  dmAssemblystatus: `${baseURL}/viewer/assemblystatus`,
};

export default class TeasanFacTotalData {
  timerId: any;

  oeeNuphData: oeeNuph[] = [];

  constructor() {
    makeObservable(this, {
      oeeNuphData: observable,

      getOeeNUph: action,
    });
  }

  // 실시간 데이터를 계속 받기위한 setTimeOut
  initStartLiveData = async () => {
    // clearTimeout(this.timerId);
    await this.getOeeNUph();
    // this.timerId = setTimeout(this.initStartLiveData, 5000);
    // await this.getData(); // 받을 데이터 axios로 호출
    // this.clickedData = await this.realtimeData[0]; // 최초 clikedData에 downRealtimeData에서 가져온 realTimeData[0]을 넣어놓음. realtimeData.No === 1
    // this.setClickedData();
  };

  // getData = async () => {
  //   await this.getOeeNUph();
  //   // await this.getUph();
  //   // const error = await this.getError();
  //   // const eqpStatus = await this.getEqpStatus();
  //   // // let monthSummary = await this.DBManager.getMonthSummary(company === 'TS1st' ? '1공장' : company === 'TS' ? '2공장' : '3공장');
  //   // const monthSummary = await this.getMonthSummary('1공장');
  // };

  // ---------------axios---------------------------------------------------
  getOeeNUph = async () => {
    try {
      const response = await axios.get(urls.oeeNuph);
      console.log('axios에서 가져온 데이터', response.data);
      this.oeeNuphData = this.mappingOeeNUph(response.data);
      console.log('매핑한 데이터', toJS(this.oeeNuphData));
    } catch (err) {
      console.log(err);
    }
  };

  mappingOeeNUph = (data: any) => {
    const res: any = [];
    try {
      if (data !== undefined && data.length > 0) {
        data.map((d: any, idx: number) => {
          res.push({
            MasterName: d.sfactoryname, // 공장명(factoryname)
            LineName: d.slinename, // 라인명(linename)
            ProcessKey: d.sprocesskey, // 공정명(processkey)
            CodeType: d.scartype, // 품번(cartype)

            AchievementRate: parseFloat(d.noee).toFixed(1), // OEE(oee)
            TimeOperationRate: parseFloat(d.ntime).toFixed(1), // 시간가동률(time)
            PerformanceRate: parseFloat(d.nperformance).toFixed(1), // 성능가동률(performance)
            YieldRate: parseFloat(d.nok).toFixed(1), // 양품률(ok)
            PlannedQty: d.iplancount, // 계획 수량(plancount)
            TargetQty: d.itargetcount, // 목표 수량(targetcount)
            ProductionQty: d.itotalcount, // 생산 수량(totalcount)
            OkQty: d.iokcount, // 양품 수량(okcount)
            NgQty: d.ingcount, // 불량 수량(ngcount)
            Percent: d.npercent, // 달성률(percent)

            CycleStart: d.scyclestart, // 취출(cyclestart)
            CycleEnd: d.scycleend, // 병목(cycleend)

            ReferenceCycleTime: this.isValid(d.ilinecriteriact, 'number'), // 기준 C/T(linecriteriact)
            AverageCycleTime: this.isValid(d.ilinemeanct, 'number'), // 평균 C/T(linemeanct)
            CurrentCycleTime: this.isValid(d.ilinebeforect, 'number'), // 현재 C/T(ilinebeforect)
            MaxCycleTime: this.isValid(d.ilinemaxct, 'number'), // 최대 C/T(linemaxct)
            MinCycleTimne: this.isValid(d.ilineminct, 'number'), // 최소 C/T (lineminct)

            AverageUPH: this.isValid(d.ilineuph, 'number'), // 평균 UPH(lineuph)
            MaxUPH: this.isValid(d.ilinemaxuph, 'number'), // 최대 UPH(linemaxuph)
            MinUPH: this.isValid(d.ilineminuph, 'number'), // 최소 UPH(lineminuph)

            AverageLOB: this.isValid(d.ilinemeanlob, 'number'), // 평균 LOB(linemeanlob)
            MaxLOB: this.isValid(d.ilinemaxlob, 'number'), // 최대 LOB(linemaxlob)
            MinLOB: this.isValid(d.ilineminlob, 'number'), // 최소 LOB(lineminlob)

            Operatingtime: this.isValid(d.ioperatingtime, 'number'), // 부하시간(operatingime)
            Processtime: this.isValid(d.iprocesstime, 'number'), // 가동시간(processtime)
            ErrorTime: this.isValid(d.ierrortime, 'number'), // 이상시간(errortime)
            ErrorCount: this.isValid(d.ierrorcount, 'number'), // 이상건수(errorcount)
            UpdateTime: d.dttime, // 업데이트 시간(ttime)

            ShotCount: this.isValid(d.sshotcount, 'number'), // 라인 타발수
            MoldName: this.isValid(d.smold, 'string'),
          });

          return d;
        });
      }
    } catch (e) {
      console.error(e);
    }
    return res;
  };

  isValid = (data: any, type: any) => {
    let res;
    try {
      switch (type) {
        case 'string':
        default:
          res = data !== undefined && data !== null && data !== 'null' ? data : '';
          break;

        case 'number':
          res = data !== undefined && data !== null && data !== 'null' ? Number(data) : 0;
          break;

        case 'array':
          res = data !== undefined && data !== null && data !== 'null' && data.length > 0 ? data : [];
          break;

        case 'check':
          res = !!(data !== undefined && data !== null && data !== 'null');
          break;

        case 'arrayCheck':
          res = !!(data !== undefined && data !== null && data !== 'null' && data.length > 0);
          break;

        case 'stringCheck':
          res = !!(data !== undefined && data !== null && data !== 'null' && data === '');
          break;
      }
    } catch (e) {
      console.error(e);
    }
    return res;
  };

  // getUph = async () => {
  //   const res = undefined;
  //   try {
  //     let rawData = await axios.get(urls.uph);
  //     if (rawData.data !== undefined)
  //       res = mappingData(dataTypes.uph, rawData.data);
  //   } catch (e) {
  //     console.log(e);
  //   }
  //   return res;
  // };

  // getError = async (config: any) => {
  //   const res = undefined;
  //   try {
  //     config = config === undefined ? {} : config;
  //     let rawData = await axios.get(urls.error, { params: config });
  //     if (rawData.data !== undefined) res = rawData.data;
  //   } catch (e) {
  //     console.error(e);
  //   }
  //   return res;
  // };

  // getEqpStatus = async (config) => {
  //   const res = undefined;
  //   try {
  //     const rawData = await axios.get(urls.eqpStatus);
  //     if (rawData.data !== undefined) res = rawData.data;
  //   } catch (e) {
  //     console.error(e);
  //   }
  //   return res;
  // };

  // getMonthSummary = async (config) => {
  //   const res = undefined;
  //   try {
  //     config = config === undefined ? {} : config;
  //     let rawData = await axios.get(urls.monthSummary, {
  //       params: { sfactory: config },
  //     });
  //     console.log(rawData);
  //     // console.log(sfactory);
  //     console.log(config);
  //     if (rawData.data !== undefined) {
  //       res = rawData.data;
  //     }
  //   } catch (e) {
  //     console.error("File : DBManager.js, Method : getMonthSummary");
  //     console.error(e);
  //   }
  //   return res;
  // };
}
