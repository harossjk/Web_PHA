import React from 'react'
import myStyle from './UpperAndLowerTop5.module.scss'
import { BiRectangle } from 'react-icons/bi'
import { toJS } from 'mobx';
import { dateToString } from '../../../utils/Utils';
interface props {
    resInquiryData?: any;

}

const UpperAndLowerTop5 = ({ resInquiryData }: props) => {

    // console.log("resInquiryData", toJS(resInquiryData));
    // console.log("resInquiryData.lowerTop5", toJS(resInquiryData.lowerTop5));

    return (
        <>
            {resInquiryData !== undefined &&
                <>
                    <div className={myStyle.upperContainer}>
                        <div className={myStyle.header}>
                            <span>상한선 이상 / 근접</span>
                            <span className={myStyle.headerInfo1}>(오차율 : 상한 압력적분값 기준)</span>
                            <span className={myStyle.headerInfo2}>
                                <BiRectangle className={myStyle.icon6} />
                                상한선 이상
                            </span>
                        </div>
                        <div className={myStyle.contentsList}>
                            {resInquiryData.upperTop5 !== undefined
                                && resInquiryData.upperTop5.length !== 0
                                && resInquiryData.upperTop5.map((el: any, idx: any) => (
                                    <div id={idx} className={myStyle.contents_item}>
                                        {Number(el.Inp) >= Number(el.DefaultUpperLimit)
                                            ? <div className={myStyle.item_numberOver}>{`No.${idx + 1}`}</div>
                                            : <div className={myStyle.item_number}>{`No.${idx + 1}`}</div>
                                        }
                                        <div className={myStyle.item_cycle}>
                                            {/* 0413 seo 수평 수직 수정예정 */}
                                            <div className={myStyle.item_title}>사출기</div>
                                            <div className={myStyle.item_value}>{`${el.MachineId}호기`}</div>
                                        </div>
                                        <div className={myStyle.item_cycle}>
                                            <div className={myStyle.item_title}>Cycle No.</div>
                                            <div className={myStyle.item_value}>{el.CycleNo}</div>
                                        </div>
                                        <div className={myStyle.item_mistake}>
                                            <div className={myStyle.item_title}>오차율</div>
                                            <div className={myStyle.item_value}>{el.errorRateUpper}%</div>
                                        </div>
                                        <div className={myStyle.item_pi}>
                                            <div className={Number(el.Inp) >= Number(el.DefaultUpperLimit)
                                                ? myStyle.item_titleOver
                                                : myStyle.item_title
                                            }>압력적분값</div>
                                            <div className={Number(el.Inp) >= Number(el.DefaultUpperLimit)
                                                ? myStyle.item_valueOver
                                                : myStyle.item_value}>{el.Inp}</div>
                                        </div>
                                        <div className={myStyle.item_upperPi}>
                                            <div className={myStyle.item_title}>상한<br />압력적분값</div>
                                            <div className={myStyle.item_value}>{el.DefaultUpperLimit}</div>
                                        </div>
                                        <div className={myStyle.item_cp}>
                                            <div className={myStyle.item_title}>수집일</div>
                                            <div className={myStyle.item_value}>{dateToString(new Date(el.CollectDt), `yyyy`)}<br />{dateToString(new Date(el.CollectDt), `MM-DD`)}</div>
                                        </div>
                                        <div className={myStyle.item_cpk}>
                                            <div className={myStyle.item_title}>수집시간</div>
                                            <div className={myStyle.item_value}>{dateToString(new Date(el.CollectDt), "HH:mm:ss")}</div>
                                        </div>
                                    </div>
                                ))}
                        </div>
                    </div>
                    <div className={myStyle.lowerContainer}>
                        <div className={myStyle.header}>
                            하한선 이상 / 근접
                            <span className={myStyle.headerInfo1}>(오차율 : 하한 압력적분값 기준)</span>
                            <span className={myStyle.headerInfo2}>
                                <BiRectangle className={myStyle.icon6} />
                                하한선 이하
                            </span>
                        </div>
                        <div className={myStyle.contentsList}>
                            {resInquiryData.lowerTop5 !== undefined
                                && resInquiryData.lowerTop5.length !== 0
                                && resInquiryData.lowerTop5.map((el: any, idx: any) => (
                                    <div id={idx} className={myStyle.contents_item}>
                                        {Number(el.Inp) <= Number(el.DefaultLowerLimit)
                                            ? <div className={myStyle.item_numberOver}>{`No.${idx + 1}`}</div>
                                            : <div className={myStyle.item_number}>{`No.${idx + 1}`}</div>
                                        }
                                        <div className={myStyle.item_cycle}>
                                            {/* 0413 seo 수평 수직 수정예정 */}
                                            <div className={myStyle.item_title}>사출기</div>
                                            <div className={myStyle.item_value}>{`${el.MachineId}호기`}</div>
                                        </div>
                                        <div className={myStyle.item_cycle}>
                                            <div className={myStyle.item_title}>Cycle No.</div>
                                            <div className={myStyle.item_value}>{el.CycleNo}</div>
                                        </div>
                                        <div className={myStyle.item_mistake}>
                                            <div className={myStyle.item_title}>오차율</div>
                                            <div className={myStyle.item_value}>{el.errorRateLower}%</div>
                                        </div>
                                        <div className={myStyle.item_pi}>
                                            <div className={Number(el.Inp) <= Number(el.DefaultLowerLimit)
                                                ? myStyle.item_titleOver
                                                : myStyle.item_title
                                            }>압력적분값</div>
                                            <div className={Number(el.Inp) <= Number(el.DefaultLowerLimit)
                                                ? myStyle.item_valueOver
                                                : myStyle.item_value}>{el.Inp}</div>
                                        </div>
                                        <div className={myStyle.item_upperPi}>
                                            <div className={myStyle.item_title}>하한<br />압력적분값</div>
                                            <div className={myStyle.item_value}>{el.DefaultLowerLimit}</div>
                                        </div>
                                        <div className={myStyle.item_cp}>
                                            <div className={myStyle.item_title}>수집일</div>
                                            <div className={myStyle.item_value}>{dateToString(new Date(el.CollectDt), `yyyy`)}<br />{dateToString(new Date(el.CollectDt), `MM-DD`)}</div>
                                        </div>
                                        <div className={myStyle.item_cpk}>
                                            <div className={myStyle.item_title}>수집시간</div>
                                            <div className={myStyle.item_value}>{dateToString(new Date(el.CollectDt), "HH:mm:ss")}</div>

                                        </div>
                                    </div>
                                ))}
                        </div>
                    </div>
                </>


            }

        </>
    )
}

export default UpperAndLowerTop5;
