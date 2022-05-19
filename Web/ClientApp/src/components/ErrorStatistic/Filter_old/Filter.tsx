import React, { useEffect, useState } from 'react'
import DropDown from '../../common/DropDown'
import TimePicker from '../../common/TimePicker'
import myStyle from './Filter.module.scss'
import useStore from '../../../stores';
import { toJS } from 'mobx';
import { observer } from 'mobx-react';
const Filter = () => {



    const { MSMDStore } = useStore();



    const [filterData, setFilterData] = useState<Array<any>>([]);
    const [machineList, setMachineList] = useState<Array<any>>([]);
    const [channelList, setChannelList] = useState<Array<any>>([]);

    useEffect(() => {
        MSMDStore.downMoldData();
    }, []);

    const handleTypeChange = (data: String) => {
        setFilterData([])

        console.log("타입 클릭됨.", data);
        setMachineList([]);  // 누적방지 초기화
        setChannelList([]);  // 누적방지 초기화
        MSMDStore.getMoldInfo.map((el, idx) => {
            if (el.machineName.substring(0, 2) === data) {
                setMachineList(machineList => [...machineList, el.machineName])
            }
        })
    };

    const handleMachineChange = (data: String) => {
        setChannelList([]);
        MSMDStore.getMoldInfo.map((el, idx) => {
            if (el.machineName === data) {
                setChannelList(el.chName)
            }
        })
        const filter = filterData.filter((el) => !el.machineId);
        setFilterData([...filter, { "machineId": data }])
    };


    const handleChannelChange = (data: String) => {
        const filter = filterData.filter((el) => !el.chName);
        setFilterData([...filter, { chName: data }])
    };


    const handleWorkChange = (data: String) => {
        console.log("Work 올라옴.", data);

    };

    const handleStartDtChange = (data: String) => {
        const filter = filterData.filter((el) => !el.startDt);
        setFilterData([...filter, { startDt: data }])
    };

    const handleEndDtChange = (data: String) => {
        const filter = filterData.filter((el) => !el.endDt);
        setFilterData([...filter, { endDt: data }])
    };


    console.log("filterData", filterData);

    return (
        <div className={myStyle.container}>
            <div className={myStyle.filter}>
                <div className={myStyle.dropDown}>
                    <DropDown title="타입" options={type} onValueChange={handleTypeChange} />
                    <DropDown title="설비" options={machineList} onValueChange={handleMachineChange} />
                    <TimePicker title="시작일" onTimeChange={handleStartDtChange} />
                    <TimePicker title="종료일" onTimeChange={handleEndDtChange} />
                </div>
            </div>
            <div className={myStyle.bottonBox}>
                <div className={myStyle.expert}>EXPORT</div>
                <div className={myStyle.search}>조회</div>
            </div>
        </div>
    )
}

export default observer(Filter)

const type = ["수직", "수평"];
const workList = ["주긴", "야간", "종일"];