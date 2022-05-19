import React, { useEffect, useState } from 'react'
import Draggable from "react-draggable";
import myStyle from './CycleModal.module.scss'
import DetailCycleInfo from '../DetailCycleInfo'
import DetailCycleGraph from '../DetailCycleGraph';
import useStore from '../../../stores';
import { toJS } from 'mobx';
import { observer } from 'mobx-react';

interface props {
    setOpenSettingModal: any;
    monitoringData: any;

}

const CycleModal = ({ setOpenSettingModal, monitoringData }: props) => {
    const { MSMDStore } = useStore();
    const cycleDetailOneCycle = MSMDStore.getDetailInfoOneCycle;
    const cycleDetailOneGraph = MSMDStore.getDetailInfoOneGraph;
    const [position, setPosition] = useState({ x: 0, y: 0 }); // box의 포지션 값



    useEffect(() => {
        MSMDStore.downCycleInfoOne(monitoringData); //모달오픈시 처음보여줄 정보
    }, []);

    // 업데이트 되는 값을 set 해줌
    const trackPos = (data: any) => {
        setPosition({ x: data.x, y: data.y });
    };

    const handleChangeInquiry = async (inquiryData: any) => {
        await MSMDStore.downCycleInfoOne(inquiryData);
    }

    console.log("cycleDetailOneCycle", toJS(cycleDetailOneCycle));


    return (

        <Draggable onDrag={(e, data) => trackPos(data)} >
            <div className={myStyle.monitoringSearch}>
                <DetailCycleInfo onChangeInquiry={handleChangeInquiry} setOpenSettingModal={setOpenSettingModal} initFilterData={monitoringData} />
                <div className={myStyle.contents}>
                    {cycleDetailOneGraph.length !== 0 ?
                        <>
                            <div>
                                <DetailCycleGraph cycleDetailOneGraph={cycleDetailOneGraph[0]} />
                            </div>
                            <div className={myStyle.cycleInfoContainer}>
                                <div className={myStyle.infoTitle}>
                                    <div className={myStyle.blank}>채널</div>
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
                                        {cycleDetailOneCycle !== undefined && cycleDetailOneCycle.Values !== undefined && cycleDetailOneCycle.Values.map((el: any, idx: number) => {
                                            return (
                                                <div key={idx}>
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
                                        })}
                                    </div>
                                </div>
                            </div>
                        </>
                        : <div>금일 조회결과 없음</div>
                    }
                </div>
            </div>
        </Draggable>

    )
}

export default observer(CycleModal)