import React from 'react';
import MoldSensorMonitoringData from './MoldSensorMonitoringData';
import InquiryData from './InquiryData';
import RackData from './RackData';
import RawData from './RawData';
import ErrorStateData from './ErrorStateData';
import UtilStore from './UtilStore';

const MSMDStore: MoldSensorMonitoringData = new MoldSensorMonitoringData();
const ProcessAbilityStore: InquiryData = new InquiryData();
const RackStore: RackData = new RackData();
const RawDataStore: RawData = new RawData();
const ErrorStateDataStore: ErrorStateData = new ErrorStateData();
const UtilDataStore: UtilStore = new UtilStore();

const useStore = () => {
  return { MSMDStore, ProcessAbilityStore, RackStore, RawDataStore, ErrorStateDataStore, UtilDataStore };
};
export default useStore;
