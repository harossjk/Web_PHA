import React, { useEffect, useState } from 'react'
import myStyle from './MoldLocationPage.module.scss'
import plant from '../../asset/img/plant_final.svg';
import CageA1x2 from '../../asset/img/CageA1x2.svg';
import CageA1x3 from '../../asset/img/CageA1x3.svg';
import CageA1x4 from '../../asset/img/CageA1x4.svg';
import useStore from '../../stores';
import { observer } from 'mobx-react';
import MoldSearchBox from '../../components/MoldLocation/MoldSearchBox';
import MoldSearchResult from '../../components/MoldLocation/MoldSearchResult';
import Rack from '../../components/MoldLocation/Rack';
import { BiRectangle } from 'react-icons/bi'
import { toJS } from 'mobx';
import { Oval } from 'react-loader-spinner'
import Rack3 from '../../components/MoldLocation/Rack3';
import { makeStyles } from '@material-ui/core/styles';
import Tooltip from '@mui/material/Tooltip';
import Rack4 from '../../components/MoldLocation/Rack4';

const useStyles = makeStyles({
    tooltip: {
        fontSize: "14px",
    },
});

const MoldLocationPage = () => {

    const classes = useStyles();
    const { ProcessAbilityStore, RackStore } = useStore();
    const [btnPosition, setBtnPosition] = useState<string>("")
    const [filterData, setFilterData] = useState<any>({});
    const [rackId, setRackId] = useState<string>("");
    const [colRack, setColRack] = useState<number>(0);
    const [rackIconClicked, setRackIconClicked] = useState<boolean>(false);
    const [moldNameClicked, setMoldNameClicked] = useState<string>("");
    const [matchingRack, setMatchingRack] = useState<Array<any>>([]);
    const [rackFloor, setRackFloor] = useState<string>("");

    const moldName = ProcessAbilityStore.getMoldName;
    const rackMoldInfo = RackStore.getRackMoldInfo;
    const machineStatus = RackStore.MachineStatus;
    const rackList = RackStore.getRackList;
    const machineLocationInfo = RackStore.getMachineLocationInfo;
    const sidebarClickRackId = RackStore.getMyRackPosition;
    const trigger = RackStore.getClickedTrigger;
    const sidebarClickRackFloor = RackStore.getMyRackFloor;
    const [loading, setLoading] = useState(true);
    // console.log("matchingRack", matchingRack);
    // console.log("colRack", colRack);
    // console.log("matchingRack", matchingRack);
    // console.log("sidebarClickRackFloor", sidebarClickRackFloor);

    useEffect(() => {
        setFilterData({});
    }, [])

    useEffect(() => {
        RackStore.downRackMoldData();
        ProcessAbilityStore.downMoldNameData();
        RackStore.downMachineStatus();
        RackStore.downRackListData();
        RackStore.downMachineLocationInfo();
    }, [loading, rackIconClicked]);


    useEffect(() => {
        setMatchingRack([]);
        //사이드바에서 클릭시
        if (sidebarClickRackId === false) {
            setRackIconClicked(false);
        }


        if (sidebarClickRackId) {
            // setMatchingRack([]);
            setRackId(sidebarClickRackId);
            setRackIconClicked(true);  //사이드바에서 클릭한것을 랙아이콘에서 클릭한것처럼 동작
            setRackFloor(sidebarClickRackFloor);
            const modifyArray: any[] = [];
            rackMoldInfo.map((el, idx) => {
                if (el.rackBarcode !== null && el.rackBarcode.substr(0, 3) === sidebarClickRackId) {
                    modifyArray.push({ moldName: el.moldName, locationName: el.locationName, x: el.rackBarcode.substr(2, 1), y: el.rackBarcode.substr(4, 1) })
                }
            })
            if (modifyArray.length !== 0) {
                setMatchingRack(modifyArray);
                setLoading(false);
            }
        }
    }, [sidebarClickRackId, trigger])


    //금형명 클릭시
    const handleMoldNameChange = (data: string) => {
        // console.log("data", data);

        RackStore.setMyRackPosition("");
        if (rackIconClicked) setLoading(true);
        setRackIconClicked(false);
        setMoldNameClicked("");
        setColRack(0);

        // console.log("rackMoldInfo", toJS(rackMoldInfo));

        rackMoldInfo.map((el, idx) => {
            if (el.rfid === data) setFilterData(rackMoldInfo[idx])
        });
    };


    //Rack아이콘 클릭시
    const handleRackIconClick = (myPosition: string, id: string) => {

        //moldPosition으로 rackType 값 넣기
        rackList.map((el) => {
            if (el.key === id) el.value.map((item: any) => { setRackFloor(item.floor); })
        })
        let rackMoldInfoFilteringData: any[] = [];

        rackMoldInfo.map((el, idx) => {
            const charAtRackBarcode = el.rackBarcode !== null && el.rackBarcode.charAt(0);
            if (charAtRackBarcode === id) rackMoldInfoFilteringData.push(toJS(el));
        });

        if (myPosition !== undefined && myPosition[0] === id) {

            //선택 금형 위치
            setLoading(true);
            setRackIconClicked(true);
            const rackSplit = myPosition.split('-');

            setRackId(`${rackSplit[0]}-${rackSplit[1]}`);
            setColRack(Number(rackSplit[2]));

            //나머지 금형 위치
            const modifyArray: any[] = [];
            rackMoldInfoFilteringData.map((el, idx) => {
                if (el.rackBarcode.substr(0, 3) === `${rackSplit[0]}-${rackSplit[1]}`) modifyArray.push({ moldName: el.moldName, locationName: el.locationName, x: el.rackBarcode.substr(2, 1), y: el.rackBarcode.substr(4, 1) });
            })

            setMatchingRack(modifyArray);
        }
        else if (myPosition !== undefined || myPosition[0] !== id) {
            setLoading(true);
            setRackIconClicked(true);
            setRackId(`${id}-1`);

            const modifyArray: any[] = [];
            rackMoldInfoFilteringData.map((el, idx) => {
                modifyArray.push({ moldName: el.moldName, locationName: el.locationName, x: el.rackBarcode.substr(2, 1), y: el.rackBarcode.substr(4, 1) });
            })
            setMatchingRack(modifyArray);
        }
    }


    const machineliveStatus = (viewStauts: string = 'Disconnect') => {
        switch (viewStauts) {
            case 'Disconnect':
                return myStyle.Disconnect
            case 'Wait':
                return myStyle.Wait
            case 'Manual':
                return myStyle.Manual
            case 'Auto':
                return myStyle.Auto
            case 'Alarm':
                return myStyle.Alarm
            case 'Preheating':
                return myStyle.Preheating
            case 'Changemold':
                return myStyle.Changemold
            default:
                return myStyle.Disconnect
        }
    }

    //툴팁
    const [title, setTitle] = useState<any>("");

    const handleMachineInMold = (data: string) => {
        // console.log("Data", data);
        // console.log("rackMoldInfo", toJS(rackMoldInfo));

        setTitle("금형없음")
        const result = rackMoldInfo.length !== 0 && rackMoldInfo.filter((el => el.locationBarcode === data))
        // console.log("result", result);

        if (result && result.length !== 0) {
            if (result.length > 1) {
                let tempArr: any = [];
                result.map((el: any) => {
                    tempArr.push(el.moldName);
                })
                let tempStr = tempArr.join('\n');
                return setTitle(tempStr);

            }
            return setTitle(result[0].moldName)
        }
    }

    // console.log("filterData", toJS(filterData));
    // console.log("machineStatus", toJS(machineStatus));
    // console.log("rackMoldInfo", toJS(rackMoldInfo));
    // console.log("machineLocationInfo", toJS(machineLocationInfo));
    // console.log("moldNameClicked", toJS(moldNameClicked));


    //금형아이콘 클릭시 및 위치
    const iconPosition = () => {
        let array = [];
        for (let y = 0; y < 130; y++) {
            array.push(
                <div key={y} className={myStyle.tableCol}>
                    {machineLocationInfo !== undefined && machineStatus.length !== 0 && [...Array(300)].map((n, x) => {
                        return (
                            // <div key={x} className={myStyle.tableRow} onClick={() => handlePosition(x, y)}>
                            <div key={x} className={myStyle.tableRow}>
                                {/* 수평사출기 */}
                                {x === 73 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_15.locationLegend}<br />{title}</span>} placement="top"><span id="hor_15" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_15.barcode)} className={filterData.locationName === machineStatus.mId_h_15.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_15.viewStatus)} /></Tooltip>}
                                {x === 80 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_16.locationLegend}<br />{title}</span>} placement="top"><span id="hor_16" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_16.barcode)} className={filterData.locationName === machineStatus.mId_h_16.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_16.viewStatus)} /></Tooltip>}
                                {x === 87 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_17.locationLegend}<br />{title}</span>} placement="top"><span id="hor_17" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_17.barcode)} className={filterData.locationName === machineStatus.mId_h_17.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_17.viewStatus)} /></Tooltip>}
                                {x === 94 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_18.locationLegend}<br />{title}</span>} placement="top"><span id="hor_18" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_18.barcode)} className={filterData.locationName === machineStatus.mId_h_18.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_18.viewStatus)} /></Tooltip>}
                                {x === 101 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_19.locationLegend}<br />{title}</span>} placement="top"><span id="hor_19" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_19.barcode)} className={filterData.locationName === machineStatus.mId_h_19.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_19.viewStatus)} /></Tooltip>}
                                {x === 107 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_20.locationLegend}<br />{title}</span>} placement="top"><span id="hor_20" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_20.barcode)} className={filterData.locationName === machineStatus.mId_h_20.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_20.viewStatus)} /></Tooltip>}
                                {x === 114 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_21.locationLegend}<br />{title}</span>} placement="top"><span id="hor_21" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_21.barcode)} className={filterData.locationName === machineStatus.mId_h_21.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_21.viewStatus)} /></Tooltip>}
                                {x === 121 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_22.locationLegend}<br />{title}</span>} placement="top"><span id="hor_22" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_22.barcode)} className={filterData.locationName === machineStatus.mId_h_22.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_22.viewStatus)} /></Tooltip>}
                                {x === 128 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_23.locationLegend}<br />{title}</span>} placement="top"><span id="hor_23" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_23.barcode)} className={filterData.locationName === machineStatus.mId_h_23.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_23.viewStatus)} /></Tooltip>}
                                {x === 135 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_24.locationLegend}<br />{title}</span>} placement="top"><span id="hor_24" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_24.barcode)} className={filterData.locationName === machineStatus.mId_h_24.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_24.viewStatus)} /></Tooltip>}

                                {x === 158 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_25.locationLegend}<br />{title}</span>} placement="top"><span id="hor_25" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_25.barcode)} className={filterData.locationName === machineStatus.mId_h_25.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_25.viewStatus)} /></Tooltip>}
                                {x === 165 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_26.locationLegend}<br />{title}</span>} placement="top"><span id="hor_26" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_26.barcode)} className={filterData.locationName === machineStatus.mId_h_26.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_26.viewStatus)} /></Tooltip>}
                                {x === 172 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_27.locationLegend}<br />{title}</span>} placement="top"><span id="hor_27" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_27.barcode)} className={filterData.locationName === machineStatus.mId_h_27.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_27.viewStatus)} /></Tooltip>}
                                {x === 179 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_28.locationLegend}<br />{title}</span>} placement="top"><span id="hor_28" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_28.barcode)} className={filterData.locationName === machineStatus.mId_h_28.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_28.viewStatus)} /></Tooltip>}
                                {x === 186 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_29.locationLegend}<br />{title}</span>} placement="top"><span id="hor_29" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_29.barcode)} className={filterData.locationName === machineStatus.mId_h_29.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_29.viewStatus)} /></Tooltip>}
                                {x === 192 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_30.locationLegend}<br />{title}</span>} placement="top"><span id="hor_30" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_30.barcode)} className={filterData.locationName === machineStatus.mId_h_30.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_30.viewStatus)} /></Tooltip>}
                                {x === 199 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_31.locationLegend}<br />{title}</span>} placement="top"><span id="hor_31" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_31.barcode)} className={filterData.locationName === machineStatus.mId_h_31.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_31.viewStatus)} /></Tooltip>}
                                {x === 205 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_32.locationLegend}<br />{title}</span>} placement="top"><span id="hor_32" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_32.barcode)} className={filterData.locationName === machineStatus.mId_h_32.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_32.viewStatus)} /></Tooltip>}
                                {x === 212 && y === 53 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_33.locationLegend}<br />{title}</span>} placement="top"><span id="hor_33" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_33.barcode)} className={filterData.locationName === machineStatus.mId_h_33.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_33.viewStatus)} /></Tooltip>}

                                {x === 70 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_1.locationLegend}<br />{title}</span>} placement="top"><span id="hor_1" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_1.barcode)} className={filterData.locationName === machineStatus.mId_h_1.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_1.viewStatus)} /></Tooltip>}
                                {x === 78 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_2.locationLegend}<br />{title}</span>} placement="top"><span id="hor_2" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_2.barcode)} className={filterData.locationName === machineStatus.mId_h_2.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_2.viewStatus)} /></Tooltip>}
                                {x === 87 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_3.locationLegend}<br />{title}</span>} placement="top"><span id="hor_3" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_3.barcode)} className={filterData.locationName === machineStatus.mId_h_3.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_3.viewStatus)} /></Tooltip>}
                                {x === 96 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_h_4.locationLegend}<br />{title}</span>} placement="top"><span id="hor_4" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_4.barcode)} className={filterData.locationName === machineStatus.mId_h_4.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_4.viewStatus)} /></Tooltip>}
                                {x === 104 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_5.locationLegend}<br />{title}</span>} placement="top"><span id="hor_5" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_5.barcode)} className={filterData.locationName === machineStatus.mId_h_5.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_5.viewStatus)} /></Tooltip>}
                                {x === 113 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_6.locationLegend}<br />{title}</span>} placement="top"><span id="hor_6" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_6.barcode)} className={filterData.locationName === machineStatus.mId_h_6.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_6.viewStatus)} /></Tooltip>}
                                {x === 122 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_7.locationLegend}<br />{title}</span>} placement="top"><span id="hor_7" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_7.barcode)} className={filterData.locationName === machineStatus.mId_h_7.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_7.viewStatus)} /></Tooltip>}
                                {x === 130 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_8.locationLegend}<br />{title}</span>} placement="top"><span id="hor_8" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_8.barcode)} className={filterData.locationName === machineStatus.mId_h_8.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_8.viewStatus)} /></Tooltip>}
                                {x === 158 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_9.locationLegend}<br />{title}</span>} placement="top"><span id="hor_9" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_9.barcode)} className={filterData.locationName === machineStatus.mId_h_9.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_9.viewStatus)} /></Tooltip>}
                                {x === 166 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_10.locationLegend}<br />{title}</span>} placement="top"><span id="hor_10" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_10.barcode)} className={filterData.locationName === machineStatus.mId_h_10.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_10.viewStatus)} /></Tooltip>}
                                {x === 175 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_11.locationLegend}<br />{title}</span>} placement="top"><span id="hor_11" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_11.barcode)} className={filterData.locationName === machineStatus.mId_h_11.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_11.viewStatus)} /></Tooltip>}
                                {x === 184 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_12.locationLegend}<br />{title}</span>} placement="top"><span id="hor_12" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_12.barcode)} className={filterData.locationName === machineStatus.mId_h_12.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_12.viewStatus)} /></Tooltip>}
                                {x === 192 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_13.locationLegend}<br />{title}</span>} placement="top"><span id="hor_13" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_13.barcode)} className={filterData.locationName === machineStatus.mId_h_13.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_13.viewStatus)} /></Tooltip>}
                                {x === 200 && y === 65 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_h_14.locationLegend}<br />{title}</span>} placement="top"><span id="hor_14" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_h_14.barcode)} className={filterData.locationName === machineStatus.mId_h_14.machineLegend ? myStyle.Blink : machineliveStatus(machineStatus.mId_h_14.viewStatus)} /></Tooltip>}


                                {/* 수직사출기 */}
                                {x === 66 && y === 81 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_v_1.locationLegend}<br />{title}</span>} placement="top"><span id="ver_1" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_1.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 82 && y === 81 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_v_2.locationLegend}<br />{title}</span>} placement="top"><span id="ver_2" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_2.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 97 && y === 81 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_v_3.locationLegend}<br />{title}</span>} placement="top"><span id="ver_3" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_3.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 113 && y === 81 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_v_4.locationLegend}<br />{title}</span>} placement="top"><span id="ver_4" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_4.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 128 && y === 81 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_v_5.locationLegend}<br />{title}</span>} placement="top"><span id="ver_5" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_5.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 64 && y === 97 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_v_6.locationLegend}<br />{title}</span>} placement="top"><span id="ver_6" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_6.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 83 && y === 97 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span> {machineLocationInfo.mId_v_7.locationLegend}<br />{title}</span>} placement="top"><span id="ver_7" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_7.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 102 && y === 97 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_v_8.locationLegend}<br />{title}</span>} placement="top"><span id="ver_8" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_8.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 121 && y === 97 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_v_9.locationLegend}<br />{title}</span>} placement="top"><span id="ver_9" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_9.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 160 && y === 97 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_v_10.locationLegend}<br />{title}</span>} placement="top"><span id="ver_10" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_10.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 179 && y === 97 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.mId_v_11.locationLegend}<br />{title}</span>} placement="top"><span id="ver_11" onMouseOver={() => handleMachineInMold(machineLocationInfo.mId_v_11.barcode)} className={machineliveStatus('Disconnect')} /></Tooltip>}

                                {/* RACK */}
                                {x === 206 && y === 72 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-I" placement="top"><span id="I" onClick={() => handleRackIconClick(moldNameClicked, 'I')} className={checkedRack(moldNameClicked) === 'I' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 211 && y === 72 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-C" placement="top"><span id="C" onClick={() => handleRackIconClick(moldNameClicked, 'C')} className={checkedRack(moldNameClicked) === 'C' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 216 && y === 72 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-B" placement="top"><span id="B" onClick={() => handleRackIconClick(moldNameClicked, 'B')} className={checkedRack(moldNameClicked) === 'B' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 221 && y === 72 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-A" placement="top"><span id="A" onClick={() => handleRackIconClick(moldNameClicked, 'A')} className={checkedRack(moldNameClicked) === 'A' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 202 && y === 87 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-H" placement="top"><span id="H" onClick={() => handleRackIconClick(moldNameClicked, 'H')} className={checkedRack(moldNameClicked) === 'H' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 208 && y === 87 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-G" placement="top"><span id="G" onClick={() => handleRackIconClick(moldNameClicked, 'G')} className={checkedRack(moldNameClicked) === 'G' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 213 && y === 87 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-F" placement="top"><span id="F" onClick={() => handleRackIconClick(moldNameClicked, 'F')} className={checkedRack(moldNameClicked) === 'F' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 219 && y === 87 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-E" placement="top"><span id="E" onClick={() => handleRackIconClick(moldNameClicked, 'E')} className={checkedRack(moldNameClicked) === 'E' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                                {x === 224 && y === 87 && <Tooltip classes={{ tooltip: classes.tooltip }} title="Rack-D" placement="top"><span id="D" onClick={() => handleRackIconClick(moldNameClicked, 'D')} className={checkedRack(moldNameClicked) === 'D' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}


                                {/* 금형반 */}
                                {/* {x === 258 && y === 105 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.MOLD.locationLegend}<br />{moldBantitle.length !==0 && moldBantitle.map((el:any)=>`${el}</br>`)}</span>} placement="top"><span id="D" onMouseOver={() => handleMachineInMold(machineLocationInfo.MOLD.barcode)} className={checkedRack(moldNameClicked) === 'D' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>} */}
                                {x === 258 && y === 105 && <Tooltip classes={{ tooltip: classes.tooltip }} title={<span>{machineLocationInfo.MOLD.locationLegend}<br />{title.split('\n').map((line: any) => { return <span>{line}<br /></span> })}</span>} placement="top"><span id="MOLD" onMouseOver={() => handleMachineInMold(machineLocationInfo.MOLD.barcode)} className={filterData.locationBarcode === 'MOLD' ? myStyle.Blink : machineliveStatus('Disconnect')} /></Tooltip>}
                            </div>
                        )
                    })}
                </div>
            );
        }
        return array;
    }



    const handleMoldClicked = (machineId: string) => {
        setMoldNameClicked(machineId)
    }

    const checkedRack = (checkData: string) => {
        if (checkData !== undefined) return checkData.charAt(0);
    }

    return (
        <div className={myStyle.container} >
            {machineStatus !== undefined && Object.keys(machineStatus).length !== 0 &&
                <>
                    <div className={myStyle.contentsLeft}>
                        <div className={myStyle.contentsTitle}>
                            {!rackIconClicked ? <div>라인조감도 {`${btnPosition}`}</div> : <div>{`Rack ${rackId}`}</div>}
                            {!rackIconClicked &&
                                <div className={myStyle.injectionType}>
                                    <div>
                                        <span><BiRectangle className={myStyle.icon0} />선택금형</span>
                                        <span><BiRectangle className={myStyle.icon1} />계획휴지</span>
                                        <span><BiRectangle className={myStyle.icon2} />설비예열</span>
                                        <span><BiRectangle className={myStyle.icon3} />작업준비</span>
                                        <span><BiRectangle className={myStyle.icon4} />금형교체</span>
                                        <span><BiRectangle className={myStyle.icon5} />자동</span>
                                        <span><BiRectangle className={myStyle.icon6} />이상</span>
                                    </div>
                                </div>
                            }
                        </div>
                        {loading
                            ?
                            <div className={myStyle.loaderPosition}><Oval color="#00BFFF" height={80} width={80} /></div>
                            : <></>

                        }
                        <div className={myStyle.contentMain} >
                            < img onLoad={() => setLoading(false)} src={!rackIconClicked
                                ? plant
                                : rackFloor === "2"
                                    ? CageA1x2
                                    : rackFloor === "3"
                                        ? CageA1x3
                                        : CageA1x4
                            } alt="" className={!rackIconClicked ? myStyle.contentBackground : myStyle.contentRackBackground} />
                            {
                                !rackIconClicked && !loading
                                    ? <div className={myStyle.gridTable}>{iconPosition()}</div>
                                    : <Rack4 rackId={rackId} rackFloor={rackFloor} filterData={filterData} moldList={matchingRack} selectY={colRack} rackIconClicked={rackIconClicked} loading={loading} />
                            }
                        </div>
                    </div>

                    <div className={myStyle.contentsRight}>
                        <div className={myStyle.contentTop}><MoldSearchBox title="금형명" options={moldName} onValueChange={handleMoldNameChange} /></div>
                        <div className={myStyle.contentBottom}><MoldSearchResult filterData={filterData} onclickMold={handleMoldClicked} /></div>
                    </div>

                </>
            }
        </div >
    )
}

export default observer(MoldLocationPage)