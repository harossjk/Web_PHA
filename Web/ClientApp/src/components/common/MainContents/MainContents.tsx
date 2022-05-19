import React, { useEffect } from 'react';
import { Link, Route, Routes } from 'react-router-dom';
import myStyle from './MainContents.module.scss';
import DataMonitoringPage from '../../../pages/DataMonitoringPage';
import ProcessAbilityPage from '../../../pages/ProcessAbilityPage';
import ErrorStatisticPage from '../../../pages/ErrorStatisticPage';
import MoldLocationPage from '../../../pages/MoldLocationPage';
import RawDataPage from '../../../pages/RawDataPage';
import useStore from '../../../stores';

const MainContents = () => {

  //최초 랙데이터 로딩
  const { RackStore } = useStore();
  useEffect(() => {
    RackStore.downRackMoldData();
  }, []);

  return (
    <>
      <Routes>
        <Route path="/" element={<DataMonitoringPage />} />
        <Route path="/viewer/dataMonitoring" element={<DataMonitoringPage />} />
        <Route path="/viewer/processAbility" element={<ProcessAbilityPage />} />
        <Route path="/viewer/errorStatistic" element={<ErrorStatisticPage />} />
        <Route path="/viewer/rawData" element={<RawDataPage />} />
        <Route path="/viewer/moldLocation" element={<MoldLocationPage />} />
      </Routes>
    </>
  );
};

export default MainContents;
