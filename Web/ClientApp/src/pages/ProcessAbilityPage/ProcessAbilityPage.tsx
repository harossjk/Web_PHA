import { toJS } from 'mobx';
import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react';
import DropDown from '../../components/common/DropDown';
import Filter from '../../components/ProcessAbility/Filter';
import DistTable from '../../components/ProcessAbility/DistTable';
import PressureIntegralGraph from '../../components/ProcessAbility/PressureIntegralGraph';
import UpperAndLowerTop5 from '../../components/ProcessAbility/UpperAndLowerTop5';
import useStore from '../../stores';
import myStyle from './ProcessAbilityPage.module.scss'
import InquiryInfo from '../../components/ProcessAbility/InquiryInfo';
import { Oval } from 'react-loader-spinner'
import { calIncuiryData2 } from '../../utils/Utils';

const ProcessAbilityPage = () => {


    const { ProcessAbilityStore, UtilDataStore } = useStore();
    const TempInquiry = ProcessAbilityStore.getTempInquiry;
    const chName = ProcessAbilityStore.getDefaultChannel;
    const [inquiry, setInquiry] = useState<any>(TempInquiry);
    const [loading, setLoading] = useState<boolean>(false);
    const [prevData, setprevData] = useState<any>({});
    const moldName = ProcessAbilityStore.getMoldName;
    const [exportLoading, setExportLoading] = useState<boolean>(false);


    let resInquiryData = ProcessAbilityStore.getInquiryData;


    useEffect(() => {
        ProcessAbilityStore.downMoldNameData();  //전체 금형명(rfid)리스트 가져옴
    }, [])

    useEffect(() => {
        //최초 랜더링
        if (Object.keys(inquiry).length === 0) {
            if (Object.keys(resInquiryData).length === 0) setLoading(false)
            setprevData(resInquiryData)
        }
        //검색후 랜더링
        if (Object.keys(inquiry).length !== 0) {
            if (prevData === resInquiryData) {
                setLoading(true)
            }
            else {
                setLoading(false)
                setExportLoading(false);
                setprevData(resInquiryData)
            }
        }
    }, [inquiry, resInquiryData]);



    // inquiryData를 서버에 요청
    const handleChangeInquiry = (inquiryData: any, chName: string, reportExport: boolean = false) => {

        //export
        if (reportExport) {
            setExportLoading(true);
            UtilDataStore.setExportExelData({
                ...inquiryData,
                fileName: "PROCESS_ABILITY_REPORT.xlsx",
                pageName: "ProcessAbilityPage"
            })
        }

        setLoading(true)
        setInquiry(inquiryData)
        ProcessAbilityStore.downinquiryFirstData(inquiryData, chName);
        ProcessAbilityStore.setTempInquiry(inquiryData);

        if (Object.keys(resInquiryData).length === 0) {
            setLoading(true)
        }

    }

    const handlePropsLoading = (selectChName: string) => {
        ProcessAbilityStore.setChangeChName(selectChName)
    }

    return (
        <div className={myStyle.container}>
            <div className={myStyle.filter}>
                <Filter setInquiryData={setInquiry} moldName={moldName} onChangeInquiry={handleChangeInquiry} />
            </div>
            {
                loading
                    ?
                    <div className={myStyle.loading}>

                        <div className={myStyle.loadingStart}>
                            <div className={myStyle.loadingBar}>
                                <Oval secondaryColor='grey' color="#ff6021" height={80} width={80} />
                                <span className={myStyle.loadingString}>{exportLoading ? <span>Exporting...</span> : <span>조회중입니다...</span>}</span>
                            </div>
                        </div>
                        <div className={myStyle.contents}>
                            <div className={myStyle.contentsLeft}>
                                <div className={myStyle.chart}>
                                    <div className={myStyle.contentsTitle}>조회 그래프</div>
                                    <div className={myStyle.content}>
                                        <PressureIntegralGraph chName={chName} handlePropsLoading={handlePropsLoading} loading={loading} resInquiryData={resInquiryData} />
                                    </div>
                                </div>
                                <div className={myStyle.distTable}>
                                    <div className={myStyle.contentsTitle}>조회 결과</div>
                                    <div className={myStyle.content}>
                                        <DistTable resInquiryData={resInquiryData} />
                                    </div>
                                </div>
                            </div>
                            <div className={myStyle.contentsRight}>
                                <div className={myStyle.contentsTitle}>상한 / 하한 TOP 5</div>
                                <div className={myStyle.content}>
                                    <UpperAndLowerTop5 resInquiryData={resInquiryData} />
                                </div>
                            </div>
                        </div>
                    </div>
                    :
                    <div className={myStyle.contents}>
                        <div className={myStyle.contentsLeft}>
                            <div className={myStyle.chart}>
                                <div className={myStyle.contentsTitle}>조회 그래프</div>
                                <div className={myStyle.content}>
                                    {inquiry.length !== 0 || resInquiryData !== undefined
                                        ? <PressureIntegralGraph chName={chName} handlePropsLoading={handlePropsLoading} resInquiryData={resInquiryData} />
                                        : <div className={myStyle.notResult}>조회결과 없음</div>
                                    }
                                </div>
                            </div>
                            <div className={myStyle.distTable}>
                                <div className={myStyle.contentsTitle}>조회 결과</div>
                                <div className={myStyle.content}>
                                    {inquiry.length !== 0 || resInquiryData !== undefined
                                        ? <DistTable resInquiryData={resInquiryData} />
                                        : <div className={myStyle.notResult}>조회결과 없음</div>
                                    }
                                </div>
                            </div>
                        </div>
                        <div className={myStyle.contentsRight}>
                            <div className={myStyle.contentsTitle}>상한 / 하한 TOP 5</div>
                            <div className={myStyle.content}>
                                {inquiry.length !== 0 || resInquiryData !== undefined
                                    ? <UpperAndLowerTop5 resInquiryData={resInquiryData} />
                                    : <div className={myStyle.notResult}>조회결과 없음</div>
                                }
                            </div>
                        </div>
                    </div>
            }
        </div>

    )
};

export default observer(ProcessAbilityPage);