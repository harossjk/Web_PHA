import React, { useEffect, useState } from 'react'
import myStyle from './Rack4.module.scss'
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
    rackFloor: string;
    rackId: string;
}

const Rack4 = ({ rackId, rackFloor, filterData, loading, selectY, moldList, rackIconClicked }: props) => {



    useEffect(() => {
    }, [moldList]);

    //배열 순서정렬
    moldList.sort((prev: any, next: any) => prev.y.localeCompare(next.y));

    //금형이미지 같은위치 중복제거
    const uniqueMoldPosition = moldList.filter((el: any, idx: any, arr: any) => {
        return arr.findIndex((item: any) => item.y === el.y) === idx
    });

    //같은층에 이미지 선택금형이미지 색 변경하기위해 moldName 묶기
    const tempMap = moldList.reduce((ret: any, { y, moldName }: any) => {
        if (ret[y]) {
            ret[y].push(moldName);
        } else {
            ret[y] = [moldName];
        }
        return ret;
    }, {});

    const resultObject = Object.entries(tempMap).reduce((arr: any, [y, moldName]) => {
        // arr.push({ key, name }); //key === y값!!!!!
        arr.push({ y, moldName }); //key === y값!!!!!
        return arr;
    }, []);


    // console.log(" Number(rackFloor)", Number(rackFloor));
    // console.log("uniqueMoldPosition", uniqueMoldPosition);
    // console.log("resultObject", resultObject);
    // console.log("moldList", moldList);

    //금형명 리스트에 랜더링
    const moldNameRender = () => {
        let array = [];
        for (let i = 1; i <= Number(rackFloor); i++) {
            array.push(
                <div className={myStyle.moldInfoItem}>
                    <div className={myStyle.moldInfoImage}>
                        {resultObject.map((el: any) => {
                            if (i === Number(el.y) && resultObject.length !== 0) {
                                if (el.moldName.includes(filterData.moldName)) return <div className={myStyle.moldImage}><img src={moldB} alt="" className={myStyle.imgStyle} /></div>
                                else return <div className={myStyle.moldImage}><img src={moldA} alt="" className={myStyle.imgStyle} /></div>
                            }
                            if (i === Number(el.y) && el.moldName !== filterData.moldName) return <div className={myStyle.moldImage}><img src={moldA} alt="" className={myStyle.imgStyle} /></div>
                            if (i !== Number(el.y)) return <div className={myStyle.moldImage}></div>
                        })}
                    </div>

                    <div className={myStyle.moldInfoTitle}>
                        적재리스트<br />{`${rackId}-${i}`}
                    </div>
                    <div className={myStyle.moldNameList}>
                        <div className={myStyle.moldNameListLayout}>
                            {
                                [...Array(4)].map((n, j) => {
                                    return (
                                        <div key={j} className={myStyle.moldNameItemBox}>
                                            {/* {moldList.length !== 0 &&  moldList[j+1]} */}
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
                    <div className={rackFloor === "2"
                        ? myStyle.moldInfoList2
                        : rackFloor === "3"
                            ? myStyle.moldInfoList3
                            : myStyle.moldInfoList4}>
                        {moldNameRender()}
                    </div>
                </div>
            }
        </>
    )
}

export default observer(Rack4)



