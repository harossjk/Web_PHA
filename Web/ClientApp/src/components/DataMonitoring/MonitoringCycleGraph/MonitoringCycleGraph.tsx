import { toJS } from 'mobx';
import React, { useEffect, useMemo, useState } from 'react';
import { ComposedChart, XAxis, YAxis, Tooltip, Legend, CartesianGrid, Bar, Line, ReferenceLine, LineChart } from 'recharts'
import ChannelSetting from '../ChannelSetting';
import myStyle from './MonitoringCycleGraph.module.scss'
import searchIcon from '../../../asset/img/icon_search.svg' // 돋보기로 수정예정 
import CycleModal from '../CycleModal';
interface props {
    monitoringData: any;
    handleClicked: (data: boolean) => void;
    selectMold: number;

}

const MonitoringCycleGraph = ({ monitoringData, handleClicked, selectMold }: props) => {

    // console.log("monitoringData ", toJS(monitoringData));


    const dataKeyArray = monitoringData !== undefined && monitoringData.data[0] !== undefined && Object.keys(monitoringData.data[0]);
    // console.log("dataKeyArray", toJS(dataKeyArray));

    const [openSettingModal, setOpenSettingModal] = useState<Boolean>(false)
    const [isClicked, setIsClicked] = useState<boolean>(false);


    const handleColor = (key: string) => {
        let a: any;
        monitoringData.colorS.map((el: object, idx: number) => {
            if (Object.keys(el)[0] === key) {
                a = Object.values(el)[0].color
            }
        })
        return a;
    }

    //Cycle 버튼클릭
    const openSearchInfo = () => {
        if (monitoringData !== undefined && monitoringData.data.length !== 0) {
            setOpenSettingModal(true)
            setIsClicked(!isClicked);
        } else {
            return handleNonMonitoringData();
        }

    }

    //애니메이션 속도
    const duration = 3500;


    //최대 최소
    const limitMaxTemp = monitoringData !== undefined && monitoringData.tUpperLimit !== null ? Number(monitoringData.tUpperLimit) + 50 : 200;
    // const limitMinTemp = 0;
    const limitMinTemp = monitoringData !== undefined && monitoringData.tLowerLimit !== null ? Number(monitoringData.tLowerLimit) + -50 : 0;
    const limitMaxPressure = monitoringData !== undefined && monitoringData.pUpperLimit !== null ? Number(monitoringData.pUpperLimit) + 100 : 800;
    const limitMinPressure = monitoringData !== undefined && monitoringData.pLowerLimit !== null ? Number(monitoringData.pLowerLimit) - 100 : 0;
    // const limitMinPressure = 0;

    const handleNonMonitoringData = () => {
        alert("실시간 수집정보가 없습니다.");
        setOpenSettingModal(false);
        setIsClicked(!isClicked);
    }
    return (
        <div className={myStyle.container}>
            <div className={myStyle.header}>
                <span>실시간 모니터링 그래프</span>
                <img onClick={() => openSearchInfo()} src={searchIcon} alt='' className={myStyle.searchInfoIcon} />
                {openSettingModal && <CycleModal setOpenSettingModal={setOpenSettingModal} monitoringData={monitoringData} />}
                {/* {openSettingModal && monitoringData !== undefined
                    ? <CycleModal setOpenSettingModal={setOpenSettingModal} monitoringData={monitoringData} />
                    : handleNonMonitoringData()
                } */}
            </div>
            <div className={myStyle.contents}>
                <div className={myStyle.contentsRight}>
                    <LineChart width={490} height={495} data={monitoringData !== undefined ? monitoringData.data : []}>
                        <XAxis
                            dataKey="x"
                            label={{
                                position: "insideBottomRight",
                                value: '시간',
                                fontSize: 14,
                                fontWeight: 700,
                                fill: "white",
                            }}
                            tick={false}
                            axisLine={{ stroke: '#707070' }}
                        />
                        <YAxis
                            yAxisId="left-axis"
                            domain={[!limitMinTemp ? 0 : limitMinTemp, !limitMaxTemp ? 200 : limitMaxTemp]}
                            // domain={[0,200]}
                            axisLine={{ stroke: '#707070' }}

                            tick={false}
                            label={{
                                position: "insideTopRight",
                                value: '온도(°C)',
                                fontSize: 14,
                                fontWeight: 700,
                                fill: "#e33030",
                            }} />

                        <YAxis
                            yAxisId="right-axis"
                            domain={[!limitMinPressure ? 0 : limitMinPressure, !limitMaxPressure ? 800 : limitMaxPressure]}
                            // domain={[0, 800]}
                            orientation="right"
                            tick={false}
                            axisLine={{ stroke: '#707070' }}

                            label={{
                                position: "insideTopLeft",
                                value: '압력(bar)',
                                fontSize: 14,
                                fontWeight: 700,
                                fill: "#ffd919",
                            }}
                        />
                        <Tooltip contentStyle={{ backgroundColor: "#1f2328" }} />
                        {
                            dataKeyArray !== undefined && dataKeyArray && dataKeyArray.map((item1, idx) => (
                                <Line key={idx} animationDuration={duration} id={item1} yAxisId={item1.charAt(0) === 'T' ? "left-axis" : "right-axis"} type="monotone" dot={false} dataKey={item1} stroke={handleColor(item1)!!} />
                            ))
                        }
                        {
                            dataKeyArray !== undefined && dataKeyArray && dataKeyArray.map((item, idx) => {
                                if (item.charAt(0).includes('T')) {
                                    return (
                                        <React.Fragment key={idx}>
                                            <ReferenceLine
                                                label={{ value: '100°C', position: 'left', fill: "#c42b2b" }}
                                                y={100}
                                                stroke="#c42b2b"
                                                yAxisId={"left-axis"}
                                                strokeDasharray="3 3"
                                            />
                                        </React.Fragment>
                                    )
                                }
                            })
                        }
                        {
                            dataKeyArray !== undefined && dataKeyArray && dataKeyArray.map((item, idx) => {
                                if (item.charAt(0).includes('P')) {
                                    return (
                                        <React.Fragment key={idx}>
                                            <ReferenceLine
                                                label={{ value: '0', position: 'right', fill: "#ffd919" }}
                                                y={0}
                                                stroke="#806432"
                                                yAxisId={"right-axis"}
                                                strokeDasharray="3 3"
                                            />
                                            <ReferenceLine
                                                label={{ value: '200', position: 'right', fill: "#ffd919" }}
                                                y={200}
                                                stroke="#806432"
                                                yAxisId={"right-axis"}
                                                strokeDasharray="3 3"
                                            />
                                            <ReferenceLine
                                                label={{ value: '400', position: 'right', fill: "#ffd919" }}
                                                y={400}
                                                stroke="#806432"
                                                yAxisId={"right-axis"}
                                                strokeDasharray="3 3"
                                            />
                                            <ReferenceLine
                                                label={{ value: '600', position: 'right', fill: "#ffd919" }}
                                                y={600}
                                                stroke="#806432"
                                                yAxisId={"right-axis"}
                                                strokeDasharray="3 3"
                                            />
                                        </React.Fragment>
                                    )
                                }
                            })
                        }
                    </LineChart>
                </div>
                <div className={myStyle.contentsLeft}>
                    {
                        <ChannelSetting
                            selectMold={selectMold}
                            clickedSetting={handleClicked}
                            monitoringData={monitoringData !== undefined && monitoringData.length !== 0 && monitoringData}
                        />
                    }
                </div>
            </div>
        </div>
    );
}

export default MonitoringCycleGraph;








































{/* <Tooltip contentStyle={{ backgroundColor: "#1f2328" }} />
{
    dataKeyArray !== undefined && dataKeyArray && dataKeyArray.map((item1, idx) => (
        <Line key={idx} animationDuration={duration} id={item1} yAxisId={item1.charAt(0) === 'T' ? "left-axis" : "right-axis"} type="monotone" dot={false} dataKey={item1} stroke={handleColor(item1)!!} />
    ))
} */}
{/* 
<ReferenceLine
    label={{ value: '0°C', position: 'left', fill: "#c42b2b" }}
    y={0}
    stroke="#c42b2b"
    yAxisId={"left-axis"}
    strokeDasharray="3 3"
/> */}
{/* 

<ReferenceLine
    label={{ value: '50', position: 'left', fill: "#f5f5f5" }}  
    y={50}
    stroke="#8a8a8a"
    yAxisId={"left-axis"}
    strokeDasharray="3 3"
/> */}
{/* <ReferenceLine
    label={{ value: '100°C', position: 'left', fill: "#c42b2b" }}
    y={100}
    stroke="#c42b2b"
    yAxisId={"left-axis"}
    strokeDasharray="3 3"
/> */}
{/* <ReferenceLine
    label={{ value: '150', position: 'left', fill: "#f5f5f5" }}
    y={150}
    stroke="#8a8a8a"
    yAxisId={"left-axis"}
    strokeDasharray="3 3"
/> */}





{/* <ReferenceLine
    label={{ value: '0', position: 'right', fill: "#ffd919" }}
    y={0}
    stroke="#806432"
    yAxisId={"right-axis"}
    strokeDasharray="3 3"
/>
<ReferenceLine
    label={{ value: '200', position: 'right', fill: "#ffd919" }}
    y={200}
    stroke="#806432"
    yAxisId={"right-axis"}
    strokeDasharray="3 3"
/>
<ReferenceLine
    label={{ value: '400', position: 'right', fill: "#ffd919" }}
    y={400}
    stroke="#806432"
    yAxisId={"right-axis"}
    strokeDasharray="3 3"
/>
<ReferenceLine
    label={{ value: '600', position: 'right', fill: "#ffd919" }}
    y={600}
    stroke="#806432"
    yAxisId={"right-axis"}
    strokeDasharray="3 3"
/>
</LineChart> */}
