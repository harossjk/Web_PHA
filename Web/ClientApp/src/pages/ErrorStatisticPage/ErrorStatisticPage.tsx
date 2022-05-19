import { observer } from 'mobx-react'
import { toJS } from 'mobx';
import React, { useEffect, useState } from 'react'
import AgGrid from '../../components/common/AgGrid'
import BarGraph from '../../components/ErrorStatistic/BarGraph'
import CircleGraph from '../../components/ErrorStatistic/CircleGraph'
import Filter from '../../components/ErrorStatistic/Filter'
import IndividualError from '../../components/ErrorStatistic/IndividualError'
import useStore from '../../stores'
import myStyle from './ErrorStatistic.module.scss'
import { Oval } from 'react-loader-spinner';

const ErrorStatisticPage = () => {

    const { ProcessAbilityStore, ErrorStateDataStore, UtilDataStore } = useStore();
    const TempInquiry = ErrorStateDataStore.getTempInquiry;

    const [inquiry, setInquiry] = useState<any>(TempInquiry);
    const moldName = ProcessAbilityStore.getMoldName;
    const resInquiryData: Object = ErrorStateDataStore.getInquiryError;
    const [prevData, setprevData] = useState<any>({});
    const [loading, setLoading] = useState<boolean>(false);
    const [exportLoading, setExportLoading] = useState<boolean>(false);
    const [selectOneMachineError, setSelectOneMachineError] = useState<any>();

    useEffect(() => {
        ProcessAbilityStore.downMoldNameData();  //전체 금형명(rfid)리스트 가져옴
    }, [])


    useEffect(() => {
        setSelectOneMachineError([]);
        if (Object.keys(inquiry).length === 0) {

            if (Object.keys(resInquiryData).length === 0) setLoading(false)
            setprevData(resInquiryData)
        }

        //검색후 랜더링
        else if (Object.keys(inquiry).length !== 0) {
            if (prevData === resInquiryData) {
                setLoading(true)
            }
            else {
                setLoading(false)
                setExportLoading(false);
                setprevData(resInquiryData)
                if (Object.keys(resInquiryData)[0] === 'err') alert("조회데이터가 없습니다.")
            }
        }

        //초기화
        if (Object.keys(resInquiryData).length !== 0) setSelectOneMachineError(Object.entries(Object.values(resInquiryData)[0])[0][1])
    }, [inquiry, resInquiryData]);

    let calArray: Array<any> = [];

    if (Object.keys(resInquiryData).length !== 0) {
        Object.entries(Object.values(resInquiryData)[0]).length !== 0 &&
            Object.entries(Object.values(resInquiryData)[0]).map((el: any) => {
                calArray.push(
                    {
                        id: `수평 ${el[0]}호기`,
                        label: `수평 ${el[0]}호기`,
                        value: el[1].length
                    }
                )
            });

    }

    let agGridArray: Array<any> = [];
    if (Object.keys(resInquiryData).length !== 0) {
        Object.entries(Object.values(resInquiryData)[0]).length !== 0 &&
            Object.entries(Object.values(toJS(resInquiryData))[0]).map((el: any) => {
                el[1].map((item: any) => {
                    let machineId = item.MachineId
                    agGridArray.push({ ...item, MachineId: `수평 ${machineId}호기` })
                })
            });
    }

    // inquiryData를 서버에 요청
    const handleChangeInquiry = (inquiryData: any, reportExport: boolean = false) => {
        console.log("inquiryData", inquiryData);

        //조회데이터 초기화
        setSelectOneMachineError([]);
        //export
        if (reportExport) {
            setExportLoading(true);
            UtilDataStore.setExportExelData({
                ...inquiryData,
                fileName: "ERROR_STATISTIFC_REPORT.xlsx",
                pageName: "ErrorStatisticPage"
            })
        }

        setInquiry(inquiryData)
        ErrorStateDataStore.setTempInquiry(inquiryData);
        ErrorStateDataStore.downinquiryErrorData(inquiryData);
    }

    const handleMachineChange = (myIndex: number) => {
        setSelectOneMachineError(Object.entries(Object.values(resInquiryData)[0])[myIndex][1])
    }

    return (
        <div className={myStyle.container}>
            <div className={myStyle.filter}>
                <Filter setInquiryData={setInquiry} moldName={moldName} onChangeInquiry={handleChangeInquiry} />
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
                            <div className={myStyle.errorGraph}>
                                <div className={myStyle.contentsTitle}>조회 그래프</div>
                                <div className={myStyle.content}>
                                    <div className={myStyle.content_Left}><CircleGraph data={calArray} /></div>
                                    <div className={myStyle.content_Right}><BarGraph data={calArray} /></div>
                                </div>
                            </div>
                            <div className={myStyle.detailInfo}>
                                <div className={myStyle.contentsTitle}>상세 이상 통계</div>
                                <div className={myStyle.content}>
                                    <IndividualError initErrorData={initErrorData} calArray={calArray} selectOneMachineError={selectOneMachineError} onValueChange={handleMachineChange} />
                                    <div className={myStyle.content_Left}>
                                    </div>
                                    <div className={myStyle.content_Right}>
                                        <div className={myStyle.title}>이상 통계 데이터 표</div>
                                        <div className={myStyle.contnetAgGrid}>
                                            <AgGrid data={agGridArray} defaultColDef={defaultColDef} />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    : <div className={myStyle.contents}>
                        <div className={myStyle.errorGraph}>
                            <div className={myStyle.contentsTitle}>조회 그래프</div>
                            <div className={myStyle.content}>
                                <div className={myStyle.content_Left}><CircleGraph data={calArray} /></div>
                                <div className={myStyle.content_Right}><BarGraph data={calArray} /></div>
                            </div>
                        </div>
                        <div className={myStyle.detailInfo}>
                            <div className={myStyle.contentsTitle}>상세 이상 통계</div>
                            <div className={myStyle.content}>
                                <div className={myStyle.content_Left}>
                                    <IndividualError initErrorData={initErrorData} calArray={calArray} selectOneMachineError={selectOneMachineError} onValueChange={handleMachineChange} />
                                </div>
                                <div className={myStyle.content_Right}>
                                    <div className={myStyle.title}>이상 통계 데이터 표</div>
                                    <div className={myStyle.contnetAgGrid}>
                                        <AgGrid data={agGridArray} defaultColDef={defaultColDef} />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
            }
        </div>
    )
}

export default observer(ErrorStatisticPage)


const defaultColDef = [
    { headerName: '설비', minWidth: 120, flex: 2, field: 'MachineId', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
    { headerName: 'RFID', minWidth: 120, flex: 1, field: 'Rfid', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
    { headerName: '금형명', minWidth: 150, flex: 3, field: 'MoldName', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
    { headerName: '데이터타입', minWidth: 150, flex: 5, field: 'DataType', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
    { headerName: '메시지', minWidth: 230, flex: 5, field: 'Message', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
    { headerName: 'Value', minWidth: 120, flex: 5, field: 'Value', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
    { headerName: 'Limit Value', minWidth: 120, flex: 5, field: 'LimitValue', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
    { headerName: '발생시각', minWidth: 200, flex: 3, field: 'CollectDt', sortable: true, floatingFilter: true, filter: 'agTextColumnFilter', resizable: true },
]



const initErrorData = [
    {
        "id": "Integral_Pressure/UpperError",
        "label": "압력적분 기준초과",
        "value": 0,
    },
    {
        "id": "Integral_Pressure/LowerError",
        "label": "압력적분 기준미만",
        "value": 0,
    },
    {
        "id": "Maximum_Pressure/UpperError",
        "label": "최대압력 기준초과",
        "value": 0,
    },
    {
        "id": "Maximum_Pressure/LowerError",
        "label": "최소압력 기준미만",
        "value": 0,
    },
    {
        "id": "MoldTemp_Temperature/UpperError",
        "label": "금형온도 기준초과",
        "value": 0,
    },
    {
        "id": "MoldTemp_Temperature/LowerError",
        "label": "금형온도 기준미만",
        "value": 0,
    },
    {
        "id": "Maximum_Temperature/UpperError",
        "label": "최대온도 기준초과",
        "value": 0,
    },
    {
        "id": "Maximum_Temperature/LowerError",
        "label": "최소온도 기준미만",
        "value": 0,
    },
]