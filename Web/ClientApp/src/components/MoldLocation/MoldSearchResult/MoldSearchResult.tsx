import { toJS } from 'mobx';
import React from 'react'
import myStyle from './MoldSearchResult.module.scss'
interface props {
    filterData: any;
    onclickMold: (machineId: string) => void;
}

const MoldSearchResult = ({ filterData, onclickMold }: props) => {

    // console.log("filterData", toJS(filterData));


    const curPosition = (filterData: any) => {

        if (filterData.locationBarcode !== null) {
            let type = filterData.locationBarcode.split('-')[0] === 'H' ? "수평" : "수직"
            if (filterData.locationBarcode === 'MOLD') {
                return type = "금형반"
            }
            const machineId = filterData.locationBarcode.split('_')[2]
            onclickMold(machineId)
            return `${type} ${filterData.locationName}`
        }

        else if (filterData.rackBarcode !== null) {
            const rackId = filterData.rackBarcode;
            onclickMold(rackId)
            return `Rack : ${filterData.rackBarcode}`
        }
        else return "위치확인불가";
    }

    return (
        <div className={myStyle.moldInfo}>
            <div className={myStyle.container}>
                <div className={myStyle.moldInfoTitle}>
                    <div className={myStyle.moldInfoTitle}>금형명</div>
                    <div className={myStyle.moldInfoTitle}>위치</div>
                    <div className={myStyle.moldInfoTitle}>Cavity</div>
                    <div className={myStyle.moldInfoTitle}>Ton</div>
                    <div className={myStyle.borderBottomAdd}></div>
                </div>
                <div className={myStyle.moldInfoValue}>
                    {filterData !== undefined && Object.keys(filterData).length !== 0
                        ?
                        <>
                            <div className={myStyle.moldInfoItem}>{filterData.moldName}</div>
                            <div className={myStyle.moldInfoItem}>{curPosition(filterData)}</div>
                            <div className={myStyle.moldInfoItem}>{filterData.cavity}</div>
                            <div className={myStyle.moldInfoItem}>{filterData.ton}</div>
                            <div className={myStyle.borderBottomAdd}></div>
                        </>
                        : <div className={myStyle.moldInfoItemNoData}>금형(RFID)을 선택해주세요.</div>
                    }
                </div>
            </div>
        </div>
    )
}

export default MoldSearchResult