import React, { useEffect, useState } from 'react'
import DropDown from '../../common/DropDown'
import TimePicker from '../../common/TimePicker'
import myStyle from './Filter.module.scss'
import useStore from '../../../stores';
import { toJS } from 'mobx';
import { observer } from 'mobx-react';
import DropDownMultiSelect from '../../common/DropDownMultiSelect';
import { stringToDate } from '../../../utils/Utils';
import ComboBox from '../../common/ComboBox'
interface props {
    setInquiryData: any;
    moldName: any;
    onChangeInquiry: (data: any) => void;
}

const Filter = ({ setInquiryData, moldName, onChangeInquiry }: props) => {

    const { MSMDStore } = useStore();
    const [filterData, setFilterData] = useState<Object>({});
    const [machineList, setMachineList] = useState<Array<any>>([]);
    const [channelList, setChannelList] = useState<Array<any>>([]);

    useEffect(() => {
        MSMDStore.downMoldData();
    }, []);



    const handleMoldNameChange = (data: string) => {

        setFilterData({ ...filterData, rfid: data })
    };

    const handleTypeChange = (data: string) => {
        setMachineList([]);
        MSMDStore.getMoldInfo.map((el, idx) => {
            if (el.machineName.substring(0, 2) === data) {
                setMachineList(machineList => [...machineList, el.machineName])
            }
        })
        if (data === "수직") data = "vertical"
        if (data === "수평") data = "horizontal"
        setFilterData({ ...filterData, machineType: data })
    };

    const handleMachineChange = (data: any) => {

        MSMDStore.getMoldInfo.map((el, idx) => {
            if (el.machineName === data) {
                setChannelList(el.chName)
            }
        })
        const machineNameToNumber = data.map((el: any, idx: any) => {
            return el.substr(3, 2)
        })
        console.log("machineNameToNumber", machineNameToNumber);

        setFilterData({ ...filterData, machineId: machineNameToNumber })
    };

    const handleChannelChange = (data: string) => {

    }


    const handleWorkChange = (data: string) => {
        if (data === "주간") data = "day"
        if (data === "야간") data = "night"
        if (data === "종일") data = "all"
        setFilterData({ ...filterData, workType: data })
    };

    const handleStartDtChange = (data: string) => {
        const startDt = stringToDate(data, "YYYYMMDD") //   const test = new Date(data)와 값 같이 나옴, 사용하고 싶은것 사용.    
        setFilterData({ ...filterData, startDt: startDt })
    };

    const handleEndDtChange = (data: string) => {
        const endDt = stringToDate(data, "YYYYMMDD")
        setFilterData({ ...filterData, endDt: endDt })
    };

    const handleInquiry = (filterData: any) => {
        // console.log("filterData", filterData);

        //입력 유효성체크
        if (filterData.rfid === undefined || !filterData.rfid) return alert("금형을 선택해주세요.")
        if (filterData.machineType === undefined) return alert("타입을 선택해주세요.")
        if (filterData.machineId === undefined) return alert("설비을 선택해주세요.")
        if (filterData.workType === undefined) return alert("근무타입을 선택해주세요.")
        if (filterData.startDt === undefined) return alert("시작일를 선택해주세요.")
        if (filterData.endDt === undefined) return alert("종료일를 선택해주세요.")
        if (filterData.endDt < filterData.startDt) return alert("종료일이 시작일 보다 앞에 있습니다. ")
        setInquiryData(filterData)
        onChangeInquiry(filterData)

    }

    return (
        <div className={myStyle.container}>
            <div className={myStyle.filter}>
                <div className={myStyle.dropDown}>
                    <ComboBox title="금형명" options={moldName} onValueChange={handleMoldNameChange} />
                    <DropDown title="타입" options={type} onValueChange={handleTypeChange} />
                    <DropDownMultiSelect title="설비" options={machineList} onValueChange={handleMachineChange} />
                    <DropDown title="근무타입" options={workList} onValueChange={handleWorkChange} />


                </div>
                <div className={myStyle.timePicker}>
                    {/* <DropDown title="근무타입" options={workList} onValueChange={handleWorkChange} /> */}
                    <TimePicker title="시작일" onTimeChange={handleStartDtChange} />
                    <TimePicker title="종료일" onTimeChange={handleEndDtChange} />
                </div>

            </div>
            <div className={myStyle.bottonBox}>
                <div className={myStyle.expert}>EXPORT</div>
                <div className={myStyle.search} onClick={() => handleInquiry(filterData)}>조회</div>
            </div>
        </div>
    )
}

export default observer(Filter)

const type = ["수직", "수평"];
const workList = ["주간", "야간", "종일"];