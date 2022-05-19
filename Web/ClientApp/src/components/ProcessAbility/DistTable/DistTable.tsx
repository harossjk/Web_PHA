import { toJS } from 'mobx';
import React from 'react'
import myStyle from './DistTable.module.scss'

interface props {
    resInquiryData?: any;
}

const DistTable = ({ resInquiryData }: props) => {

    const test = (idx: number) => {
        if (idx >= 5) return `${myStyle.boxes} ${myStyle.scroll}`
        return `${myStyle.boxes}`
    }

    return (
        <div className={myStyle.container}>

            <div className={myStyle.header}>
                분포표
            </div>
            {resInquiryData !== undefined &&
                <div className={myStyle.contents}>
                    <div className={myStyle.contentsTop}>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_title}>LCL(m)</div>
                            <div className={myStyle.content_value}>{resInquiryData.LowerLimit}</div>
                        </div>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_title}>평균(Avg)</div>
                            <div className={myStyle.content_value}>{resInquiryData.Average}</div>
                        </div>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_title}>UCL(M)</div>
                            <div className={myStyle.content_value}>{resInquiryData.UpperLimit}</div>
                        </div>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_title}>분산</div>
                            <div className={myStyle.content_value}>{resInquiryData.VarianceValue}</div>
                        </div>
                    </div>

                    <div className={myStyle.contentsBottom}>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_titleCustom}>범위</div>
                            <div className={myStyle.content_titleCustom}>Count</div>
                            <div className={myStyle.content_titleCustom}>분포비율</div>
                            <div className={myStyle.content_title2}>Cycle<br />번호 리스트</div>
                        </div>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_value_range}>{`X <= m`}</div>
                            <div className={myStyle.content_value}>{resInquiryData.overLowerLimitCount}</div>
                            <div className={myStyle.content_value}>{resInquiryData.overLowerLimitPercent}%</div>


                            <div className={myStyle.content_value2}>
                                {resInquiryData.overLowerLimitValue !== undefined
                                    && resInquiryData.overLowerLimitValue.length !== 0
                                    ?
                                    //스크롤하기
                                    <>
                                        <div className={myStyle.content_value_list} >
                                            <div className={myStyle.content_value_item}>
                                                {resInquiryData.overLowerLimitValue.map((el: any, idx: any) => {
                                                    return <div id={idx}>{el.CycleNo}</div>
                                                })
                                                }
                                            </div>
                                        </div>
                                    </>
                                    : <div className={myStyle.nonList} >{'-'}</div>
                                }
                            </div>

                        </div>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_value_range}>{`m < X < M`}</div>
                            <div className={myStyle.content_value}>{resInquiryData.normalCount}</div>
                            <div className={myStyle.content_value}>{resInquiryData.normalPercent}%</div>
                            <div className={myStyle.content_value2}>
                                <div className={myStyle.nonList} >{'-'}</div>
                            </div>

                        </div>
                        <div className={myStyle.contentsItem}>
                            <div className={myStyle.content_value_range}>{`X >= M`}</div>
                            <div className={myStyle.content_value}>{resInquiryData.overUpperLimitCount}</div>
                            <div className={myStyle.content_value}>{resInquiryData.overUpperLimitPercent}%</div>
                            <div className={myStyle.content_value2}>
                                {resInquiryData.overUpperLimitValue !== undefined
                                    && resInquiryData.overUpperLimitValue.length !== 0
                                    ?
                                    <>
                                        <div className={myStyle.content_value_list} >
                                            <div className={myStyle.content_value_item}>
                                                {resInquiryData.overUpperLimitValue.map((el: any, idx: any) => {
                                                    return <div id={idx}>{el.CycleNo}</div>
                                                })
                                                }
                                            </div>
                                        </div>
                                    </>
                                    : <div className={myStyle.nonList} >{'-'}</div>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            }

        </div >
    )
}

export default DistTable