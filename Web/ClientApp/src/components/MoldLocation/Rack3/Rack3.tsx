import React, { useEffect, useState } from 'react'
import myStyle from './Rack3.module.scss'
import moldA from '../../../asset/img/moldA.png'
import moldB from '../../../asset/img/moldB.png'
import { toJS } from 'mobx';
import { observer } from 'mobx-react';
interface props {
    filterData: any;
    loading: boolean;
    selectY?: number;
    moldList: any;
    rackIconClicked: boolean;
}

const Rack3 = ({ filterData, loading, selectY, moldList, rackIconClicked }: props) => {

    // console.log("들어온 moldList", moldList);
    // console.log("filterData", toJS(filterData));
    // console.log("selectY", selectY);
    useEffect(() => {
    }, [moldList]);

    //배열 순서정렬
    moldList.sort((prev: any, next: any) => prev.y.localeCompare(next.y));

    //금형이미지 같은위치 중복제거
    const uniqueMoldPosition = moldList.filter((el: any, idx: any, arr: any) => {
        return arr.findIndex((item: any) => item.y === el.y) === idx
    });

    //길이채우기
    if (uniqueMoldPosition.length < 4) {
        console.log("uniqueMoldPosition", uniqueMoldPosition);
        
        for (let i = uniqueMoldPosition.length; i < 4; i++) {
            uniqueMoldPosition.push({ moldName: '', locationName: '', x: '', y: '2' });

        }
    }
    //금형명 리스트에 랜더링
    const moldNameRender = () => {
        let array = [];
        for (let i = 1; i <= 4; i++) {
            array.push(
                <div className={myStyle.moldInfoItem}>
                    <div className={myStyle.moldInfoTitle}>적재리스트 </div>
                    <div className={myStyle.moldNameList}>
                        <div className={myStyle.moldNameListLayout}>
                            {
                                [...Array(4)].map((n, j) => {
                                    return (
                                        <div key={j} className={myStyle.moldNameItemBox}>
                                        </div>
                                    )
                                })
                            }
                            <div className={myStyle.moldNameListValue}>
                                {
                                    moldList.map((el: any, idx: number) => {
                                        if (i === Number(el.y)) {
                                            if (el.moldName === filterData.moldName) {
                                                return (
                                                    <div key={idx} className={myStyle.moldNameListValueItem}>
                                                        <span className={myStyle.font2}>{el.moldName}</span>
                                                    </div>
                                                )
                                            }
                                            return (
                                                <div key={idx} className={myStyle.moldNameListValueItem}>
                                                    <span className={myStyle.font}>{el.moldName}</span>
                                                </div>
                                            )
                                        }

                                    })
                                }
                            </div>
                        </div>
                    </div>
                </div>
            );
        }
        return array.reverse();
    }

    return (
        <>
            {
                !loading && <div className={myStyle.gridTable}>
                    <div className={myStyle.moldBoxList}>
                        {uniqueMoldPosition.slice(0).reverse().map((el: any) => {

                            if (filterData !== undefined && el.locationName === filterData.locationName && el.y === filterData.rackBarcode.charAt(filterData.rackBarcode.length - 1)) {
                                return <div className={myStyle.moldBoxItem}><img src={moldB} alt="" className={myStyle.imgStyle} /></div>
                            }
                            if(el.moldName ==='') return< div className={myStyle.moldBoxItem}/>
                            return <div className={myStyle.moldBoxItem}><img src={moldA} alt="" className={myStyle.imgStyle} /></div>
                        })}
                    </div>
                    <div className={myStyle.moldInfoList}>
                        {moldNameRender()}
                    </div>
                </div>
            }
        </>
    )
}

export default observer(Rack3)



