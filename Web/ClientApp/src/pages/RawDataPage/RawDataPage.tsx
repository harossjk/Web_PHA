import { toJS } from 'mobx';
import { observer } from 'mobx-react';
import React, { useCallback, useEffect, useRef, useState } from 'react'
import AgGrid from '../../components/common/AgGrid';
import useStore from '../../stores';
import myStyle from './RawDataPage.module.scss'
import Filter from '../../components/RawData/Filter'
import { Oval } from 'react-loader-spinner';
const RawDataPage = () => {

    const { ProcessAbilityStore, RawDataStore, UtilDataStore } = useStore();
    const TempInquiry = RawDataStore.getTempInquiry;

    const [inquiry, setInquiry] = useState<any>(TempInquiry);
    const [inquiryData, setInquiryData] = useState<Array<any>>([]);
    const resInquiryData = RawDataStore.getRawData;
    const [loading, setLoading] = useState<boolean>(false);
    const moldName = ProcessAbilityStore.getMoldName;
    const [prevData, setprevData] = useState<any>({});
    const [exportLoading, setExportLoading] = useState<boolean>(false);


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

        // if (Object.keys(resInquiryData)[0] === 'err') alert("조회데이터가 없습니다.")

        return () => {
            UtilDataStore.setExportBtnClick(false);
        }

    }, [inquiry, resInquiryData, exportLoading]);


    // inquiryData를 서버에 요청
    const handleChangeInquiry = (inquiryData: any, reportExport: boolean = false) => {
        //export
        if (reportExport) {
            setExportLoading(true);
            UtilDataStore.setExportExelData({
                ...inquiryData,
                fileName: "RAW_Data_REPORT.xlsx",
                pageName: "RawDataPage"
            })
        }

        setLoading(true)

        setInquiry(inquiryData)
        RawDataStore.setTempInquiry(inquiryData);
        RawDataStore.downRawData(inquiryData);

        if (Object.keys(resInquiryData).length === 0) {
            setLoading(true)
        }
    }

    const handleChangeExport = (isClicked: boolean) => {
        UtilDataStore.setExportBtnClick(isClicked);
    }


    return (
        <div className={myStyle.container}>
            <div className={myStyle.filter}>
                <Filter setInquiryData={setInquiry} moldName={moldName} onChangeInquiry={handleChangeInquiry} onChangeExport={handleChangeExport} />
            </div>
            {
                loading
                    ? <div className={myStyle.loading}>
                        <div className={myStyle.loadingStart}>
                            <div className={myStyle.loadingBar}>
                                <Oval secondaryColor='grey' color="#ff6021" height={80} width={80} />
                                <span className={myStyle.loadingString}>{exportLoading ? <span>Exporting...</span> : <span>조회중입니다...</span>}</span>
                            </div>
                        </div>
                        <div className={myStyle.contents}>
                            <div className={myStyle.contentsTitle}>전체 데이터 Row 수:{resInquiryData.length !== 0 && resInquiryData.length}</div>
                            <div className={myStyle.contentsAgGrid}>
                                <AgGrid data={resInquiryData} defaultColDef={defaultColDef} type="rawData" />
                            </div>
                        </div>
                    </div>

                    : <div className={myStyle.contents}>
                        <div className={myStyle.contentsTitle}>전체 데이터 Row 수: {resInquiryData.length !== 0 && resInquiryData.length}</div>
                        <div className={myStyle.contentsAgGrid}>
                            <AgGrid data={resInquiryData} defaultColDef={defaultColDef} type="rawData" />
                        </div>
                    </div>
            }
        </div>
    )
}

export default observer(RawDataPage)



const defaultColDef = [
    {
        headerName: '설비타입',
        minWidth: 120,
        flex: 1,
        field: 'MachineType',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '설비',
        minWidth: 100,
        flex: 1,
        field: 'MachineId',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '금형명',
        minWidth: 120,
        flex: 2,
        field: 'MoldName',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: 'RFID',
        minWidth: 120,
        flex: 1,
        field: 'Rfid',
        ortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '수집시간',
        minWidth: 120,
        flex: 2,
        field: 'CollectDt',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: 'Cycle NO',
        minWidth: 120,
        flex: 1,
        field: 'CycleNo',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '채널명',
        minWidth: 120,
        flex: 1,
        field: 'ChName',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '최대 온도',
        minWidth: 120,
        flex: 1,
        field: 'Maximum_Temperature',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '최대 압력',
        minWidth: 120,
        flex: 1,
        field: 'Maximum_Pressure',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '압력 적분',
        minWidth: 120,
        flex: 1,
        field: 'Integral_Pressure',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '융융선단 온도',
        minWidth: 150,
        flex: 1,
        field: 'Meltfront_Temperature',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },
    {
        headerName: '금형 온도',
        minWidth: 120,
        flex: 1,
        field: 'MoldTemp_Temperature',
        sortable: true,
        floatingFilter: true,
        filter: 'agTextColumnFilter',
        valueFormatter: (params: any) => {
            if (params.value === null) {
                return '-'
            }
        }
    },

]