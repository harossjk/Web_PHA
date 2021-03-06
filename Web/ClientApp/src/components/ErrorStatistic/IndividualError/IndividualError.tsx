import { toJS } from 'mobx';
import React, { useState, useEffect } from 'react'
import DropDown from '../../common/DropDown'
import IndividualBarGraph from '../IndividualBarGraph';
import myStyle from './IndividualError.module.scss'

interface props {
    selectOneMachineError: any;
    calArray: any;
    onValueChange: (data: number) => void;
    initErrorData: any[];
}

const IndividualError = ({ initErrorData, calArray, onValueChange, selectOneMachineError }: props) => {

    const initialValue = 0;
    let machineNameArray: any[] = [];
    const [graphData, setGraphData] = useState<any[]>([])
    const [allErrorCount, setAllErrorCount] = useState(0)

    useEffect(() => {
        if (selectOneMachineError !== undefined && selectOneMachineError.length === 0) {
            setGraphData([]);
            setAllErrorCount(0)
        }

        else if (selectOneMachineError !== undefined && selectOneMachineError.length !== 0) {
            selectOneMachineError.map((el: any) => {
                initErrorData.map((error: any, idx: number) => initErrorData[idx].value = 0)
            })
            selectOneMachineError.map((el: any) => {
                initErrorData.map((error: any, idx: number) => {
                    if (el.ErrorType === initErrorData[idx].id) initErrorData[idx].value = initErrorData[idx].value + 1;
                })
            })
            setGraphData(initErrorData);
            setAllErrorCount(initErrorData.reduce(
                (accumulator: any, currentValue: any) => accumulator + currentValue.value
                , initialValue
            ))
        }

    }, [selectOneMachineError, initErrorData])


    if (calArray !== undefined && calArray.length !== 0) {
        calArray.map((el: any) => {
            machineNameArray.push(el.id);
        })
    }
    const handleTypeChange = (machineName: string) => {
        const myIndex = machineNameArray.indexOf(machineName);
        onValueChange(myIndex)
    };

    return (
        <div className={myStyle.container}>
            <div className={myStyle.filter}>
                <DropDown defaultValue={machineNameArray[0]} title="??????" options={machineNameArray} onValueChange={handleTypeChange} />
                <div className={myStyle.errorCount}>
                    ????????????
                    <div className={myStyle.errorCountValue}>{allErrorCount}</div>
                    <span style={{ marginLeft: "5px" }}>???</span>
                </div>
            </div>
            <div>
                <IndividualBarGraph graphData={graphData.length === 0 && allErrorCount === 0 ? resinitErrorData : graphData} />
            </div>
        </div>
    )
}
export default IndividualError

const resinitErrorData = [
    {
        "id": "Integral_Pressure/UpperError",
        "label": "???????????? ????????????",
        "value": 0,
    },
    {
        "id": "Integral_Pressure/LowerError",
        "label": "???????????? ????????????",
        "value": 0,
    },
    {
        "id": "Maximum_Pressure/UpperError",
        "label": "???????????? ????????????",
        "value": 0,
    },
    {
        "id": "Maximum_Pressure/LowerError",
        "label": "???????????? ????????????",
        "value": 0,
    },
    {
        "id": "MoldTemp_Temperature/UpperError",
        "label": "???????????? ????????????",
        "value": 0,
    },
    {
        "id": "MoldTemp_Temperature/LowerError",
        "label": "???????????? ????????????",
        "value": 0,
    },
    {
        "id": "Maximum_Temperature/UpperError",
        "label": "???????????? ????????????",
        "value": 0,
    },
    {
        "id": "Maximum_Temperature/LowerError",
        "label": "???????????? ????????????",
        "value": 0,
    },
]