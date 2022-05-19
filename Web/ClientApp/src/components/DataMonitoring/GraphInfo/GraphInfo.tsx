import { toJS } from 'mobx';
import React, { useEffect, useState } from 'react';
import myStyle from './GraphInfo.module.scss'
import useStore from '../../../stores';

interface props {
    monitoringData: any;
    cycleDetaildata: any;
    moldInfo: any[];
    renderingDataNum: number;
}

const GraphInfo = ({ monitoringData, cycleDetaildata, moldInfo, renderingDataNum }: props) => {

    // console.log("monitoringDatamonitoringDatamonitoringDatamonitoringDatamonitoringDatamonitoringData", toJS(monitoringData));

    const [cycleValue, setCycleValue] = useState<Array<any>>()

    useEffect(() => {
        setCycleValue(cycleDetaildata.Values);
        machineStateFnc(monitoringData.viewStatus, moldInfo, renderingDataNum)
        sensorCollectionFnc(cycleDetaildata, monitoringData.viewStatus, monitoringData.sensorIsActive, moldInfo, renderingDataNum)
    }, [cycleDetaildata, renderingDataNum]);

    const [machineState, setMachineState] = useState("")
    const [sensorState, setSensorState] = useState("")

    const machineStateFnc = (viewStatus: any, moldInfo: any, renderingDataNum: number) => {


        /*사출기 상태 
          viewStatus "Disconnect" : 계획휴지
          viewStatus "Wait"       : 작업준비
          viewStatus "Auto"       : 자동운전 
          viewStatus "Alarm"      : 알람(이상)
          viewStatus "Preheating" : 설비예열
          viewStatus "Changemold" : 금형교체
        */

        moldInfo.map((el: any, idx: number) => {
            if (idx === renderingDataNum) {
                switch (el.viewStatus) {
                    case "Disconnect":
                        return setMachineState("계획휴지")
                    case "Wait":
                        return setMachineState("작업준비")
                    case "Auto":
                        return setMachineState("자동")
                    case "Alarm":
                        return setMachineState("이상")
                    case "Preheating":
                        return setMachineState("설비예열")
                    case "Changemold":
                        return setMachineState("금형교체")
                    case "Manual":
                        return setMachineState("수동운전")

                    default:
                        return setMachineState("계획휴지")
                }
            }
        })

    }

    //0321 수집대기 확인해봐야함.
    const sensorCollectionFnc = (cycleDetaildata: object, viewStatus: string, sensorIsActive: string, moldInfo: any, renderingDataNum: number) => {

        // console.log("수집여부cycleDetaildata", toJS(cycleDetaildata));
        // console.log("sensorIsActive", toJS(sensorIsActive));
        // console.log("moldInfo", toJS(moldInfo));
        // console.log("renderingDataNum", toJS(renderingDataNum));

        // if (!sensorIsActive) return setSensorState("수집대기");
        // if (cycleDetaildata === undefined) return setSensorState("수집대기");
        // if (cycleDetaildata === {}) return setSensorState("수집대기");
        // if (viewStatus === undefined) return setSensorState("수집대기");
        // if (viewStatus === null) return setSensorState("수집대기");
        // return setSensorState("수집중");
        moldInfo.map((el: any, idx: number) => {
            if (idx === renderingDataNum) {
                if (el.sensorIsActive === "True") return setSensorState("수집중");
                if (el.sensorIsActive === "False") return setSensorState("수집대기");
            }
        })
    }

    return (
        <div className={myStyle.container}>
            <div className={myStyle.content_Top}>
                <div className={myStyle.contentInfo}>{monitoringData && monitoringData.machineName !== null ? monitoringData.machineName : "-"}</div>
                <div className={myStyle.content}>

                    {/* 수집여부 */}
                    <div>
                        <div className={myStyle.subTitle}>센서 수집여부</div>
                        {(() => {
                            switch (sensorState) {
                                case '수집대기':
                                    return <div className={myStyle.loader} />
                                case '수집중':
                                    return <div className={`${myStyle.loader} ${myStyle.Auto}`} />
                                default:
                                    return null
                            }
                        })()}
                        {monitoringData
                            ?
                            <div className={myStyle.contentValue}>
                                <div className={myStyle.moldState}>{sensorState}</div>
                                <div className={myStyle.moldCycle}>
                                    {
                                        // monitoringData.sensorIsActive === "false"
                                        //     ? 'Last Cycle No.'
                                        //     : monitoringData.viewStatus === 'Disconnect'
                                        //         ? 'Last Cycle No.'
                                        //         : 'Cycle No.'
                                        monitoringData.sensorIsActive === "false"
                                            ? 'Last Cycle No.'
                                            : 'Cycle No.'
                                    }
                                </div>
                                <div className={myStyle.moldCycle}>{monitoringData.cycleNo}</div>
                            </div>
                            :
                            <div className={myStyle.contentValue}>
                                <div className={myStyle.moldState}>{sensorState}</div>
                                <div className={myStyle.moldCycle}>{monitoringData.viewStatus !== 'Disconnect' ? 'Cycle No.' : 'Last Cycle No.'}</div>
                                <div className={myStyle.moldCycle}>-</div>
                            </div>

                        }
                    </div>

                    {/* 상태 */}
                    <div>
                        <div className={myStyle.subTitle}>사출기 상태</div>
                        {/* {(() => {
                            switch (monitoringData.viewStatus) {
                                case 'Disconnect':
                                    return <div className={myStyle.loader} />
                                case 'Preheating':
                                    return <div className={`${myStyle.loader} ${myStyle.Preheating}`} />
                                case 'Wait':
                                    return <div className={`${myStyle.loader} ${myStyle.Wait}`} />
                                case 'Changemold':
                                    return <div className={`${myStyle.loader} ${myStyle.Changemold}`} />
                                case 'Auto':
                                    return <div className={`${myStyle.loader} ${myStyle.Auto}`} />
                                case 'Alarm':
                                    return <div className={`${myStyle.loader} ${myStyle.Alarm}`} />
                                case 'Manual':
                                    return <div className={`${myStyle.loader} ${myStyle.Manual}`} />
                                default:
                                    return <div className={myStyle.loader} />
                            }
                        })()} */}
                        {/* 0404 seo */}
                        {(() => {
                            switch (machineState) {
                                case '계획휴지':
                                    return <div className={myStyle.loader} />
                                case '설비예열':
                                    return <div className={`${myStyle.loader} ${myStyle.Preheating}`} />
                                case '작업준비':
                                    return <div className={`${myStyle.loader} ${myStyle.Wait}`} />
                                case '금형교체':
                                    return <div className={`${myStyle.loader} ${myStyle.Changemold}`} />
                                case '자동':
                                    return <div className={`${myStyle.loader} ${myStyle.Auto}`} />
                                case '이상':
                                    return <div className={`${myStyle.loader} ${myStyle.Alarm}`} />
                                case '수동운전':
                                    return <div className={`${myStyle.loader} ${myStyle.Manual}`} />
                                default:
                                    return <div className={myStyle.loader} />
                            }
                        })()}
                        {monitoringData
                            ?
                            <div className={myStyle.contentValue}>
                                <div className={myStyle.moldState}>{machineState}</div>
                                <div className={myStyle.moldCycle}>
                                    {
                                        // monitoringData.sensorIsActive === "false"
                                        //     ? 'Last Cycle No.'
                                        //     : monitoringData.viewStatus === 'Disconnect'
                                        //         ? 'Last Cycle No.'
                                        //         : 'Cycle No.'
                                        monitoringData.sensorIsActive === "false"
                                            ? 'Last Cycle No.'
                                            : 'Cycle No.'}
                                </div>
                                <div className={myStyle.moldCycle}>{monitoringData.cycleNo}</div>
                            </div>
                            :
                            <div className={myStyle.contentValue}>
                                <div className={myStyle.moldState}>{machineState}</div>
                                <div className={myStyle.moldCycle}>{monitoringData.viewStatus !== 'Disconnect' ? 'Cycle No.' : 'Last Cycle No.'}</div>
                                <div className={myStyle.moldCycle}>-</div>
                            </div>
                        }
                    </div>
                </div>

            </div>


            {/* ----------------------------------------------------------------------------------------------------------- */}
            <div className={myStyle.content_Bottom_header}>
                Cycle 정보
            </div>
            <div className={myStyle.cycleInfoContainer}>
                <div className={myStyle.infoTitle}>
                    <div className={myStyle.blank}></div>
                    <div className={myStyle.title}>
                        <div className={myStyle.titleItem}>최대온도</div>
                        <div className={myStyle.titleItem}>최대압력</div>
                        <div className={myStyle.titleItem}>금형온도</div>
                        <div className={myStyle.titleItem}>적분</div>
                        <div className={myStyle.titleItem} style={{ fontSize: "14px" }}>융융선단온도</div>
                    </div>
                </div>
                <div className={myStyle.infoValue}>
                    <div className={myStyle.channel}>
                        {cycleDetaildata !== undefined && cycleDetaildata.Values !== undefined
                            ? cycleDetaildata.Values.map((el: any, idx: number) => {

                                return (
                                    <div key={el.ChName}>
                                        <div className={myStyle.channelName} style={{ color: `${el.Color}` }}>{el.ChName}</div>
                                        <div className={myStyle.channelValue}>
                                            <div className={myStyle.channelItem}>{el.MaxTemperature}</div>
                                            <div className={myStyle.channelItem2}>{el.MaxPressure}</div>
                                            <div className={myStyle.channelItem}>{el.MoldTemp}</div>
                                            <div className={myStyle.channelItem2}>{el.IntergralPressure}</div>
                                            <div className={myStyle.channelItem}>{el.MeltFront}</div>
                                        </div>
                                    </div>
                                )
                            })
                            : <div className={myStyle.nonValue}>수집정보없음</div>
                        }
                    </div>
                </div>
            </div>


        </div>

    )
};

export default GraphInfo;
