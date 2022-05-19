import { toJS } from 'mobx';
import Moment from 'moment';

//url export
export const baseURL = `http://localhost:6500`;
// export const baseURL = `http://172.18.10.163:6500`;

/**
 * Date타입을 String타입으로 변환
 * @param date
 * @param format String으로 변환할 날짜 포멧
 * @returns
 */
export const dateToString = (date: Date | undefined, format: string): string | undefined => {
  if (date === undefined || date === null) return undefined;
  return Moment(date).format(format);
};

/**
 * String타입을 Data타입으로 변환
 * @param dateText
 * @param format dateText의 날짜 포멧
 * @returns
 */
export const stringToDate = (dateText: string | undefined, format: string): Date | undefined => {
  if (dateText === undefined || dateText === null) return undefined;
  return Moment(dateText, format).toDate();
};

export const dateCal = (date: string | undefined | Date) => {
  if (date === undefined) return dateToString(new Date() as any as Date, 'YYYYMMDD');
  if (typeof date === 'string') return dateToString(new Date() as any as Date, 'YYYYMMDD');
  return dateToString(date as any as Date, 'YYYYMMDD');
};

export const dayCal = (date: any) => {
  if (Number(dateToString(date, 'hh')) < 13) return '오전';
  else return '오후';
};

//dataMonitoring Alarm
export const calErrorData = (allErrorData: Array<any>) => {
  let sortCollectDtData = [...allErrorData].sort((prev: any, next: any) => {
    prev = new Date(prev.collectDt);
    next = new Date(next.collectDt);
    return next - prev;
  });
  // console.log('sortCollectDtData', sortCollectDtData);
  const calErrorArray: any[] = [];
  sortCollectDtData.map((el) => {
    const a = new Date(el.collectDt);
    const resTime = dateToString(a, `MM/DD, ${dayCal(a)} hh:mm:ss`);
    const resTime2 = dateToString(a, `YYYY년 MM월 DD일 ${dayCal(a)} hh시 mm분 ss초`);

    switch (el.dataType) {
      case 'Integral_Pressure':
        el.dataType = '압력 적분';
        break;
      case 'Maximum_Temperature':
        el.dataType = '온도';
        break;
      case 'Maximum_Pressure':
        el.dataType = '압력';
        break;
      case 'MoldTemp_Temperature':
        el.dataType = '금형 온도';
        break;
      default:
        return el.dataType;
    }

    calErrorArray.push({
      machineId: `수평 ${el.machineId}호기`,
      rfid: el.rfid,
      dataType: el.dataType,
      message: el.message,
      moldName: el.moldName,
      collectDt: resTime2,
      customCollectInfo: `${resTime}, 수평 ${el.machineId}호기 ${el.moldName}, 채널 : ${el.chName}`,
      customMessage: `${el.message.split('(')[0]}`,
      value: el.value,
      limitValue: el.limitValue,
      channel: el.chName,
    });
  });
  // console.log('calErrorArray', calErrorArray);

  // return calErrorArray.slice(0, 20); 최신 20개
  return calErrorArray;
};

export const calIncuiryData2 = (data: object, chName: string) => {
  let totalCount: number = 0;
  let normalCount: number = 0;
  let overLowerLimitCount: number = 0;
  let overLowerLimitValue: any[] = [];
  let overUpperLimitCount: number = 0;
  let overUpperLimitValue: any[] = [];
  let overLowerLimitPercent: string = '';
  let overUpperLimitPercent: string = '';
  let normalPercent: string = '';
  let lowerTop5: any[] = [];
  let upperTop5: any[] = [];

  // console.log('Object.entries(data)', toJS(Object.entries(data)[0][1][0].ChannelItemDic));
  const allChannelData: any[] = Object.entries(Object.entries(data)[0][1][0].ChannelItemDic);
  // console.log('allChannelData', toJS(allChannelData));
  const myChannelData = allChannelData.filter((myChannel: any) => myChannel[0] === chName);
  // console.log('myChannelData', toJS(myChannelData[0][1]));
  const resultChannelData: any = myChannelData[0][1] as object;

  const uniqueChName: any[] = [];
  allChannelData.map((el: any) => {
    return uniqueChName.push(el[0]);
  });

  totalCount = Number(resultChannelData.TotalData.length);
  overLowerLimitCount = Number(resultChannelData.LowerErrorDataKeyS.length);
  overUpperLimitCount = Number(resultChannelData.UpperErrorDataKeyS.length);
  normalCount = totalCount - (overLowerLimitCount + overUpperLimitCount);

  overLowerLimitPercent = ((overLowerLimitCount / totalCount) * 100).toFixed(4);
  overUpperLimitPercent = ((overUpperLimitCount / totalCount) * 100).toFixed(4);
  normalPercent = String(100 - Number(overLowerLimitPercent) + Number(overUpperLimitPercent));

  overLowerLimitValue = resultChannelData.TotalData.filter((item: any) =>
    resultChannelData.LowerErrorDataKeyS.includes(item.Index),
  );
  overUpperLimitValue = resultChannelData.TotalData.filter((item: any) =>
    resultChannelData.UpperErrorDataKeyS.includes(item.Index),
  );

  lowerTop5 = resultChannelData.TotalData.filter((item: any) =>
    resultChannelData.LowerTop5DataKeyS.includes(item.Index),
  );
  //오차율 계산
  lowerTop5 = lowerTop5.map((el: any, idx: number) => {
    return {
      CycleNo: el.CycleNo,
      Inp: el.Inp,
      CollectDt: el.CollectDt,
      MachineId: el.MachineId,
      DefaultLowerLimit: resultChannelData.LowerLimit,
      errorRateLower: (
        (Math.abs(Number(resultChannelData.LowerLimit) - Number(el.Inp)) / Number(resultChannelData.LowerLimit)) *
        100
      ).toFixed(2),
    };
  });
  lowerTop5 = [...lowerTop5].sort((prev: any, next: any) => Number(prev.Inp) - Number(next.Inp)).slice(0, 5);

  upperTop5 = resultChannelData.TotalData.filter((item: any) =>
    resultChannelData.UpperTop5DataKeyS.includes(item.Index),
  );
  //오차율 계산
  upperTop5 = upperTop5.map((el: any, idx: number) => {
    return {
      CycleNo: el.CycleNo,
      Inp: el.Inp,
      CollectDt: el.CollectDt,
      MachineId: el.MachineId,
      DefaultUpperLimit: resultChannelData.UpperLimit,
      errorRateUpper: (
        (Math.abs(Number(resultChannelData.UpperLimit) - Number(el.Inp)) / Number(resultChannelData.UpperLimit)) *
        100
      ).toFixed(2),
    };
  });
  upperTop5 = [...upperTop5].sort((prev: any, next: any) => Number(next.Inp) - Number(prev.Inp)).slice(0, 5);

  return {
    ...resultChannelData,
    totalCount: totalCount,
    overLowerLimitCount: overLowerLimitCount,
    overUpperLimitCount: overUpperLimitCount,
    normalCount: normalCount,
    overLowerLimitPercent: overLowerLimitPercent,
    overUpperLimitPercent: overUpperLimitPercent,
    normalPercent: normalPercent,
    overLowerLimitValue: overLowerLimitValue,
    overUpperLimitValue: overUpperLimitValue,
    lowerTop5: lowerTop5,
    upperTop5: upperTop5,
    uniqueChName: uniqueChName,
  };
};

export const initColor = (colorS: any[]) => {
  console.log('colorS', colorS);
};
