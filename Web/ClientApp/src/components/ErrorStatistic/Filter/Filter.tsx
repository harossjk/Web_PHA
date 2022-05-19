import React, { useEffect, useState } from 'react'
import DropDown from '../../common/DropDown'
import TimePicker from '../../common/TimePicker'
import myStyle from './Filter.module.scss'
import useStore from '../../../stores';
import { toJS } from 'mobx';
import { observer } from 'mobx-react';
import DropDownMultiSelect from '../../common/DropDownMultiSelect';
import { dateToString, stringToDate } from '../../../utils/Utils';
import ComboBox from '../../common/ComboBox'
interface props {
    setInquiryData: any;
    moldName: any;
    onChangeInquiry: (data: any, reportExport?: boolean) => void;
}

const Filter = ({ setInquiryData, moldName, onChangeInquiry }: props) => {

    const { MSMDStore, ErrorStateDataStore } = useStore();
    const [filterData, setFilterData] = useState<Object>({});
    const today = new Date();
    const defaultEndDt = new Date(); //금일
    const defaultStartDt = new Date(today.setDate(today.getDate() - 7)); //일주일전
    const workList = ["주간", "야간", "종일"];
    const TempInquiry = ErrorStateDataStore.getTempInquiry;
    // console.log("TempInquiry", toJS(TempInquiry));
    // console.log("filterData", toJS(filterData));

    useEffect(() => {
        MSMDStore.downMoldData();
        if (Object.keys(TempInquiry).length === 0) {
            setFilterData({
                ...filterData,
                workType: "day",//초기값 주간
                startDt: defaultStartDt,
                endDt: defaultEndDt,

                //TempInquiry 초기값
                tempWorkType: workList[0],
                tempDefaultEndDt: dateToString(defaultEndDt, 'YYYY-MM-DD'),
                tempDefaultStartDt: dateToString(defaultStartDt, 'YYYY-MM-DD'),

            })
        }
        if (Object.keys(TempInquiry).length !== 0) {
            setFilterData({
                ...TempInquiry,
            })
        }
    }, []);



    const handleMoldNameChange = (data: string) => {
        const rfid = data !== null && data.split(" ")[0].substr(1, data.split(" ")[0].length - 2)
        const moldName = data !== null && data.split("] ")[1]
        const transMoldName = moldName && moldName.replace("#", "-");

        setFilterData({ ...filterData, rfid: rfid, tempMoldName: data, moldName: transMoldName })
    };


    const handleWorkChange = (data: string) => {
        let transData = "";
        if (data === "주간") transData = "day"
        if (data === "야간") transData = "night"
        if (data === "종일") transData = "all"
        setFilterData({ ...filterData, workType: transData, tempWorkType: data === null ? "day" : data })
    };

    const handleStartDtChange = (data: string) => {
        const startDt = new Date(data) //   const test = new Date(data)와 값 같이 나옴, 사용하고 싶은것 사용.    
        setFilterData({ ...filterData, startDt: startDt, tempDefaultStartDt: data })
    };

    const handleEndDtChange = (data: string) => {
        const endDt = new Date(data)
        setFilterData({ ...filterData, endDt: endDt, tempDefaultEndDt: data })
    };


    const handleInquiry = (filterData: any, reportExport: boolean = false) => {
        //입력 유효성체크
        if (filterData.rfid === undefined || !filterData.rfid) return alert("금형을 선택해주세요.")
        if (filterData.workType === undefined) return alert("근무타입을 선택해주세요.")
        if (filterData.startDt === undefined) return alert("시작일를 선택해주세요.")
        if (filterData.endDt === undefined) return alert("종료일를 선택해주세요.")
        if (filterData.endDt < filterData.startDt) return alert("종료일이 시작일 보다 앞에 있습니다. ")

        //export
        if (reportExport) {
            onChangeInquiry(filterData, reportExport)
        }

        setInquiryData(filterData)
        onChangeInquiry(filterData)

    }

    // console.log("filterData", filterData);

    return (
        <div className={myStyle.container}>
            <div className={myStyle.filter}>
                <div className={myStyle.dropDown}>
                    <ComboBox title="금형명" options={moldName} onValueChange={handleMoldNameChange} defaultValue={TempInquiry.tempMoldName} placeholder="금형명을 선택해주세요." />
                    <DropDown title="근무타입" options={workList} onValueChange={handleWorkChange} defaultValue={Object.keys(TempInquiry).length !== 0 ? TempInquiry.tempWorkType : "주간"} />
                    <TimePicker title="시작일" onTimeChange={handleStartDtChange} defaultValue={Object.keys(TempInquiry).length !== 0 ? TempInquiry.tempDefaultStartDt : dateToString(defaultStartDt, 'YYYY-MM-DD')} />
                    <TimePicker title="종료일" onTimeChange={handleEndDtChange} defaultValue={Object.keys(TempInquiry).length !== 0 ? TempInquiry.tempDefaultEndDt : dateToString(defaultEndDt, 'YYYY-MM-DD')} />
                </div>
                <div className={myStyle.timePicker}>
                </div>

            </div>
            <div className={myStyle.bottonBox}>
                <div className={myStyle.expert} onClick={() => handleInquiry(filterData, true)}>EXPORT</div>
                <div className={myStyle.search} onClick={() => handleInquiry(filterData)}>조회</div>
            </div>
        </div>
    )
}

export default observer(Filter)