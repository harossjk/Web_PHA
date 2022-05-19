import { toJS } from 'mobx';
import React, { useState } from 'react';
import { ComposedChart, XAxis, YAxis, Tooltip, Legend, CartesianGrid, Bar, Line, ReferenceLine } from 'recharts'
import myStyle from './PressureIntegralGraph.module.scss'
import { dateToString } from '../../../utils/Utils'
import DropDown from '../../common/DropDown';
import useStore from '../../../stores';
import { observer } from 'mobx-react';
interface props {
    resInquiryData?: any;
    loading?: boolean;
    handlePropsLoading: (data: string) => void;
    chName: string;
}

const PressureIntegralGraph = ({ chName, handlePropsLoading, loading, resInquiryData }: props) => {
    const { ProcessAbilityStore } = useStore();
    const resultDataInfo = ProcessAbilityStore.getResultDataInfo;
    const [isClicked, setIsClicked] = useState<Boolean>(false);

    // const handleChNameChange = (data: string) => {
    //     handlePropsLoading(data);
    // };

    //커스텀툴팁
    const CustomTooltip = ({ active, payload, label }: any) => {

        if (active && payload && payload.length) {
            return (
                <div className={myStyle.customTooltip} style={{ backgroundColor: "#1f2328" }}>
                    <div className={myStyle.customTooltipItem}>{`Cycle No. ${payload[0].payload?.CycleNo}`}</div>
                    <div className={myStyle.customTooltipItem}>{`사출기 번호 : ${payload[0].payload?.MachineId}호기`}</div>
                    <div className={myStyle.customTooltipItem}>{`압력적분값 : ${payload[0].value}`}</div>
                    <div className={myStyle.customTooltipItem}>{`수집일 :  ${dateToString(new Date(payload[0].payload?.CollectDt), "yyyy년 MM월 DD일")}`}</div>
                    <div className={myStyle.customTooltipItem}>{`수집시간 : ${dateToString(new Date(payload[0].payload?.CollectDt), "HH시 mm분 ss초")}`}</div>
                </div>
            );
        }

        return null;
    };

    console.log("resInquiryData", toJS(resInquiryData));


    const offset = 1000;
    let upper = Number(resInquiryData.UpperLimit) + offset;
    let lower = Number(resInquiryData.LowerLimit) - offset;
    return (
        <div className={myStyle.container}>

            <div className={myStyle.header}>
                <span>압력적분 - Cycle 차트</span>
                <DropDown defaultValue={chName} options={resInquiryData !== undefined ? resInquiryData.uniqueChName : null} onValueChange={handlePropsLoading} boxType="small" />
            </div>
            {/* 0517 SEO UCL/LCL추가 */}
            {resInquiryData !== undefined && Object.keys(resInquiryData).length !== 0 &&
                <div className={myStyle.labelBox}>
                    <span>UCL : {resInquiryData.UpperLimit}</span>
                    <span>LCL : {resInquiryData.LowerLimit}</span>
                </div>
            }
            {resInquiryData !== undefined &&
                <div className={myStyle.contents}>
                    <ComposedChart width={550} height={310}
                        data={
                            loading !== undefined
                                ? loading === false
                                    ? resInquiryData.TotalData
                                    : null
                                : resInquiryData.TotalData
                        }
                    >
                        <XAxis
                            domain={[0, 20]}
                            minTickGap={-100}
                            tick={false}
                            axisLine={{ stroke: '#707070' }}
                            label={{
                                position: "insideBottomRight",
                                value: "수집시간",
                                fontSize: 14,
                                fontWeight: 700,
                                fill: "white",
                            }}
                        />
                        <YAxis
                            yAxisId={1}
                            type="number"
                            domain={[0, upper]}
                            label={{
                                position: "insideTopRight",
                                value: "압력적분",
                                fontSize: 14,
                                fontWeight: 700,
                                fill: "white",
                            }}
                            tick={false}

                            stroke="#f5f5f5" />
                        <Tooltip content={<CustomTooltip />} />
                        <Line yAxisId={1} type="monotone" dataKey="Inp" stroke="#44ff3b" dot={false} />
                        <ReferenceLine
                            y={0}
                            stroke="none"
                            yAxisId={1}
                            label={{ value: '0', position: 'left', fill: "#f5f5f5" }}
                        />
                        <ReferenceLine
                            y={resInquiryData.UpperLimit}
                            stroke="#FF0000"
                            yAxisId={1}
                            strokeDasharray="3 3"
                            label={{ value: 'UCL', position: 'left', fill: "#f5f5f5" }}
                        />
                        <ReferenceLine
                            y={resInquiryData.LowerLimit}
                            stroke="#FF0000"
                            yAxisId={1}
                            strokeDasharray="3 3"
                            label={{ value: 'LCL', position: 'left', fill: "#f5f5f5" }}
                        />
                    </ComposedChart>
                </div>
                // {/* <div className={myStyle.labelBox}>
                //     <span>UCL : 20 bar*sec</span>
                //     <span>LCL <span style={{ paddingLeft: "3px" }}>:</span> 10<span style={{ paddingLeft: "3px" }}/> bar*sec</span>
                // </div> */}
            }
        </div>
    );
};

export default observer(PressureIntegralGraph);
