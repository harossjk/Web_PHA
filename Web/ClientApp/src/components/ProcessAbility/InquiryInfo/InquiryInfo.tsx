import React, { useState } from 'react'
import { dateToString } from '../../../utils/Utils';
import myStyle from './InquiryInfo.module.scss'

interface props {
    inquiryData?: any;
    setInquiryData: any;
    onChangeMachineId: (data: string) => void;

}

const InquiryInfo = ({ inquiryData, setInquiryData, onChangeMachineId }: props) => {

    const [selectItem, setSelectIem] = useState<Number>(0);
    const handleReset = () => {
        setInquiryData([]);
    }

    const handleMachineClick = (data: string, index: number) => {
        onChangeMachineId(data);
        setSelectIem(index);
    }

    console.log("inquiryData", inquiryData);


    return (
        <>
            {inquiryData !== undefined &&
                <div className={myStyle.container}>
                    <div className={myStyle.inquriyBox}>
                        <div className={myStyle.inquriyBox_Left}>
                            <div className={myStyle.InquiryDate}>
                                <div>{`조회일자 : ${dateToString(inquiryData.startDt, "yyyy년 MM월 DD일")} ~ ${dateToString(inquiryData.endDt, "yyyy년 MM월 DD일")}`}</div>
                            </div>
                            <div className={myStyle.workTime}>{`근무시간 : ${inquiryData.workType === "종일"
                                ? "오전 08시 ~ 오후 08시"
                                : inquiryData.workType === "주간"
                                    ? "오전 08시 ~ 오후 08시"
                                    : "오후 08시 ~ 익일 오전 08시"} `}
                            </div>
                            <div className={myStyle.moldName}>{`금형명 : ${inquiryData.moldName}`}</div>
                        </div>
                        <div className={myStyle.inquriyBox_Right}>
                            <div className={myStyle.machineId_Title}>조회<br />사출기</div>
                            <div className={myStyle.machineId_List}>
                                {inquiryData.machineId.map((el: any, idx: any) => (
                                    <div className={selectItem === idx
                                        ? myStyle.machineId_SelectedItem
                                        : myStyle.machineId_NonSelectedItem} onClick={() => handleMachineClick(el, idx)}>
                                        <span>{inquiryData.machineType === 'vertical' ? "수직" : "수평"}<br />{el}호기</span>
                                    </div>
                                ))
                                }
                            </div>
                        </div>

                    </div>
                    <div className={myStyle.bottonBox}>
                        <div className={myStyle.expert}>EXPORT</div>
                        <div className={myStyle.reset} onClick={handleReset}>초기화</div>
                    </div>
                </div>
            }
        </>
    )

}

export default InquiryInfo