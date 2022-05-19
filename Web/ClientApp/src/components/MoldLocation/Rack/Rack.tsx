import React, { useEffect, useState } from 'react'
import myStyle from './Rack.module.scss'
import moldImage from '../../../asset/img/testImage.png'
import { toJS } from 'mobx';
interface props {

    loading: boolean;
    selectX?: number;
    selectY?: number;
    matchingRackList: any;
}

const Rack = ({ loading, selectX, selectY, matchingRackList }: props) => {
    // const [cnt2, setCnt2] = useState(0)

    // useEffect(() => {

    //     setTimeout(() => {
    //         setCnt2(cnt2 + 1);
    //       }, 3000);
    // }, [cnt2]);

    // console.log(cnt2);

    console.log("matchingRackList", toJS(matchingRackList));



    const selectMold = (myX: number, myY: number, x: number, y: number) => {
        console.log("myX", myX);
        console.log("X", x);
        console.log("myY", myY);
        console.log("y", y);

        if (myX === x && myY === y) return myStyle.myMoldImage
        return myStyle.moldImage
    }

    const iconPosition = () => {
        let array = [];
        for (let x = 0; x < 4; x++) {
            array.push(
                <div className={myStyle.tableCol}>
                    {[...Array(4)].map((n, y) => {
                        return (
                            <div key={n} className={myStyle.tableRow}>
                                {matchingRackList.map((el: any, idx: number) => (
                                    x === Number(`${el.x}`) - 1 && y === Number(`${el.y}`) - 1 && selectX !== undefined && selectY !== undefined &&
                                    <div key={idx} className={myStyle.image_box} ><img key={idx} src={moldImage} alt="" className={selectMold(selectX, selectY, Number(`${el.x}`), Number(`${el.y}`))} /></div>
                                    // <div key={idx} className={myStyle.image_box} ><img key={idx} src={moldImage} alt="" className={selectMold(selectX, selectY, x, y)} /></div>
                                ))}
                            </div>
                        )
                    })}
                </div>
            );
        }
        return array;
    }

    return (
        <>
            {!loading && <div className={myStyle.gridTable}>{iconPosition()}</div>}
        </>
    )
}

export default Rack