import React, { useEffect, useState } from 'react'
import DropDown from '../../common/DropDown'
import TimePicker from '../../common/TimePicker'
import myStyle from './DetailCycleInfo.module.scss'
import useStore from '../../../stores';
import { toJS } from 'mobx';
import { observer } from 'mobx-react';
import DetailCycleGraph from '../DetailCycleGraph';
import InputBox from '../../common/InputBox';

interface props {
    initFilterData: any;
    onChangeInquiry: (data: any) => void;
    setOpenSettingModal: (data: any) => void;
}

const DetailCycleInfo = ({ initFilterData, onChangeInquiry, setOpenSettingModal }: props) => {

    const { MSMDStore } = useStore();
    const [filterData, setFilterData] = useState<Object>({});
    const [machineList, setMachineList] = useState<Array<any>>([]);
    const defaultMachineType = initFilterData.machineType === 'Horizontal' ? '수평' : '수직';
    const defaultMachineName = initFilterData.machineName;
    const defaultCycleNo = initFilterData.cycleNo;

    useEffect(() => {
        handleTypeChange(defaultMachineType)
        setFilterData({
            ...filterData,
            machineType: initFilterData.machineType,
            machineId: initFilterData.machineId,
            cycleNo: initFilterData.cycleNo
        })
    }, [])


    const handleTypeChange = (data: string) => {
        setMachineList([])
        MSMDStore.getMoldInfo.map((el, idx) => {
            if (el.machineName.substring(0, 2) === data) {
                setMachineList(machineList => [...machineList, el.machineName])
            }
        })
        if (data === "수직") data = "Vertical"
        if (data === "수평") data = "Horizontal"
        setFilterData({ ...filterData, machineType: data })
    };


    const handleMachineChange = (data: string) => {

        const machineNameToNumber = data.split(" ")[1]
        setFilterData({ ...filterData, machineId: machineNameToNumber })
    };
    const handleCycleChange = (data: string) => {
        setFilterData({ ...filterData, cycleNo: data })
    };

    const handleInquiry = (filterData: any) => {

        console.log("filterData", filterData);

        //입력 유효성체크
        if (filterData.machineType === undefined) return alert("타입을 선택해주세요.")
        if (filterData.machineId === undefined) return alert("설비을 선택해주세요.")
        if (filterData.cycleNo === undefined) return alert(" Cycle을 확인해 주세요.")

        onChangeInquiry(filterData)
    }

    const handleClose = () => {
        setOpenSettingModal(false);
    }

    return (
        <div className={myStyle.container}>

            <div className={myStyle.filter}>
                <div className={myStyle.dropDown}>
                    <DropDown title="타입" options={type} onValueChange={handleTypeChange} defaultValue={defaultMachineType} />
                    <DropDown title="설비" options={machineList} onValueChange={handleMachineChange} defaultValue={defaultMachineName} />
                    <InputBox title="Cycle" onValueChange={handleCycleChange} defaultValue={defaultCycleNo }/>
                </div>
                <div className={myStyle.bottonBox}>
                    <div className={myStyle.expert} onClick={() => handleClose()}>닫기</div>
                    <div className={myStyle.search} onClick={() => handleInquiry(filterData)}>조회</div>
                </div>
            </div>
        </div>
    )
}

export default observer(DetailCycleInfo)

const type = ["수직", "수평"];