import { toJS } from 'mobx';
import React, { useEffect, useState } from 'react';
import { ComposedChart, XAxis, YAxis, Tooltip, Legend, CartesianGrid, Bar, Line, ReferenceLine } from 'recharts'
import DropDown from '../../common/DropDown';
import GraphSetting from '../GraphSetting';
import myStyle from './PressureIntegralGraph.module.scss'
import useStore from '../../../stores';
import { dateToString } from '../../../utils/Utils';
interface props {
    piData: any;
    monitoringData: any;
    selectMold: number;
    cycleDetaildata: any;
}

const PressureIntegralGraph = ({ cycleDetaildata, piData, monitoringData, selectMold }: props) => {

    const { MSMDStore } = useStore();
    const [update, setUpdate] = useState(true);
    const data = piData !== undefined && piData.values;

    // const chNameAllArray = monitoringData !== undefined && monitoringData.chName !== null ? monitoringData.chName.split(',') : [''];
    // const chNamePressureArray = monitoringData !== undefined && chNameAllArray.length !== 0 && chNameAllArray.filter((el: any) => {
    //     return el.charAt(0) === 'P'
    // });
    // const initchName: any[] = [];

    const handleChNameChange = (data: string) => {
        MSMDStore.downPIData(selectMold, data)
        setUpdate(!update);
    };


    console.log("data!!!!!", toJS(data));


    useEffect(() => {

    }, [update, monitoringData]);

    //커스텀툴팁
    const CustomTooltip = ({ active, payload, label }: any) => {

        // console.log("payload[0]", payload[0]);

        if (active && payload && payload.length) {
            return (
                <div className={myStyle.customTooltip} style={{ backgroundColor: "none" }}>
                    <span >{`Cycle번호 : ${label}`}</span ><br />
                    <span  >{`압력적분값 : ${payload[0].value}`}</span ><br />
                    <span  >{`수집시간 : ${dateToString(new Date(payload[0].payload?.collectDt), "MM월 DD일 HH시 mm분 ss초")}`}</span >
                </div>
            );
        }

        return null;
    };

    const defalutDropDownValue = cycleDetaildata !== undefined && Object.keys(cycleDetaildata).length !== 0 && cycleDetaildata.Values.map((el: any) => {
        if (el.ChName.charAt(0) === 'P' && el.MachineId !== undefined) {
            return el.ChName
        }
    }).filter((el: any) => el !== undefined)

    const offset = 500;
    let upper = Number(piData.upperLimit) + offset;
    let lower = Number(piData.lowerLimit) - offset;
    const limitArray = [lower, upper]

    return (
        <div className={myStyle.container}>

            <div className={myStyle.header}>
                <span>실시간 모니터링 그래프</span>
                <DropDown defaultValue={cycleDetaildata !== undefined && Object.keys(cycleDetaildata).length !== 0 && defalutDropDownValue[0]} options={cycleDetaildata !== undefined && Object.keys(cycleDetaildata).length !== 0 && defalutDropDownValue} onValueChange={handleChNameChange} boxType="small" />

            </div>
            <div className={myStyle.labelBox}>
                <span>UCL : {piData.upperLimit}</span>
                <span>LCL : {piData.lowerLimit}</span>
            </div>
            <div className={myStyle.contents}>
                <ComposedChart width={500} height={262} data={data} >
                    <XAxis
                        dataKey="cycleNo"
                        // label={{
                        //     value: 'Cycle',
                        //     position: "insideBottomRight",
                        //     fontSize: 14,
                        //     fontWeight: 700,
                        //     fill: "white",
                        // }}
                        domain={[0, 20]}
                        minTickGap={-100}
                        tick={false}
                        axisLine={{ stroke: '#707070' }}

                    />
                    <YAxis
                        yAxisId={1}
                        type="number"
                        // domain={limitArray}
                        domain={[0, upper + 1000]}
                        label={{
                            position: "insideTopRight",
                            value: "압력적분",
                            fontSize: 14,
                            fontWeight: 700,
                            fill: "white",
                        }}
                        tick={false}
                        axisLine={{ stroke: '#707070' }}

                    />
                    <Tooltip content={<CustomTooltip />} />
                    {/* <Legend /> */}
                    {/* <CartesianGrid stroke="#f5f5f5" /> */}


                    <Line yAxisId={1} type="monotone" dataKey="inp" stroke="#44ff3b" dot={false} />
                    <ReferenceLine
                        y={0}
                        stroke="none"
                        yAxisId={1}
                        label={{ value: '0', position: 'left', fill: "#f5f5f5" }}
                    />
                    <ReferenceLine
                        y={piData.upperLimit}
                        stroke="#FF0000"
                        yAxisId={1}
                        strokeDasharray="3 3"
                        label={{ value: 'UCL', position: 'left', fill: "#f5f5f5" }}
                    />
                    <ReferenceLine
                        y={piData.lowerLimit}
                        stroke="#FF0000"
                        yAxisId={1}
                        strokeDasharray="3 3"
                        label={{ value: 'LCL', position: 'left', fill: "#f5f5f5" }}
                    />
                </ComposedChart>
            </div>
            <div className={myStyle.labelBox2}>
                <span>Cycle No.{monitoringData !== undefined && monitoringData.cycleNo !== undefined ? monitoringData.cycleNo : ""}</span>
            </div>
        </div>
    );
};

export default PressureIntegralGraph;
