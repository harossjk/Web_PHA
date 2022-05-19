import { toJS } from 'mobx';
import React, { useEffect } from 'react';
import { ComposedChart, XAxis, YAxis, Tooltip, Legend, CartesianGrid, Bar, Line, ReferenceLine, LineChart } from 'recharts'
import myStyle from './DetailCycleGraph.module.scss'
interface props {
    cycleDetailOneGraph: any;
}

const DetailCycleGraph = ({ cycleDetailOneGraph }: props) => {

    const dataKeyArray = cycleDetailOneGraph !== undefined && cycleDetailOneGraph.data[0] !== undefined && Object.keys(cycleDetailOneGraph.data[0]);
    useEffect(() => {

    }, [dataKeyArray]);

    const handleColor = (key: string) => {
        let a: any;
        cycleDetailOneGraph.colorS.map((el: object, idx: number) => {
            if (Object.keys(el)[0] === key) {
                a = Object.values(el)[0].color
            }
        })
        return a;
    }



    //최대 최소
    const limitMaxTemp = cycleDetailOneGraph !== undefined && cycleDetailOneGraph.tUpperLimit !== null ? Number(cycleDetailOneGraph.tUpperLimit) + 50 : 200;
    // const limitMinTemp = 0;
    const limitMinTemp = cycleDetailOneGraph !== undefined && cycleDetailOneGraph.tLowerLimit !== null ? Number(cycleDetailOneGraph.tLowerLimit) + -50 : 0;
    const limitMaxPressure = cycleDetailOneGraph !== undefined && cycleDetailOneGraph.pUpperLimit !== null ? Number(cycleDetailOneGraph.pUpperLimit) + 100 : 800;
    const limitMinPressure = cycleDetailOneGraph !== undefined && cycleDetailOneGraph.pLowerLimit !== null ? Number(cycleDetailOneGraph.pLowerLimit) - 100 : 0;
    // const limitMinPressure = 0;

    return (
        <div className={myStyle.container}>
            <div className={myStyle.header}>
                <span>{`그래프 & 수집정보 - ${cycleDetailOneGraph.machineName} `}</span>
                <span>{`Cycle No. ${cycleDetailOneGraph.cycleNo}`}</span>
            </div>
            <div className={myStyle.contents}>
                <div className={myStyle.contentsRight}>
                    <LineChart width={500} height={490} data={cycleDetailOneGraph !== undefined && cycleDetailOneGraph.data}>
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
                                <Line key={idx} id={item1} yAxisId={item1.charAt(0) === 'T' ? "left-axis" : "right-axis"} type="monotone" dot={false} dataKey={item1} stroke={handleColor(item1)!!} />
                            ))
                        }
                        {
                            dataKeyArray !== undefined && dataKeyArray && dataKeyArray.map((item, idx) => {
                                console.log("item", item);

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
            </div>
        </div>
    );
}

export default DetailCycleGraph;

