import React, { useEffect, useState } from 'react';
import myStyle from './ChannelSetting.module.scss';
import InputColor from 'react-input-color';
import useStore from '../../../stores';
import setting from '../../../asset/img/icon_setting.svg'
import { ReactComponent as Line } from '../../../asset/img/horizontal_rule.svg'
import { toJS } from 'mobx';


interface props {
    monitoringData: any;
    selectMold: number;
    clickedSetting: (data: boolean) => void;
}

const ChannelSetting = ({ monitoringData, clickedSetting, selectMold }: props) => {

    const { MSMDStore } = useStore();

    const graphItem = monitoringData.colorS
    const [openSettingModal, setOpenSettingModal] = useState<Boolean>(false)
    const [defaultChecked, setDefaultChecked] = useState<boolean>(true);
    const [changeChannelData, setChangeChannelData] = useState(graphItem)

    useEffect(() => {
        setChangeChannelData(graphItem);

    }, [monitoringData, graphItem]);



    const openSetting = () => {
        setOpenSettingModal(true)
        clickedSetting(true);
    }

    //저장버튼 클릭이벤트
    const handleSummit = () => {
        setDefaultChecked(true);
        setOpenSettingModal(false)
        // MSMDStore.changeGraph!!(visibleGraph, selectMold!!);
        MSMDStore.changeColor!!(changeChannelData, monitoringData); // 서버에 컬러저장
        clickedSetting(false);
    }

    //체크 유무
    const handleInputChecked = (color: string, visible: boolean, key: any) => {
        const newColor = changeChannelData.map((item: any) => Object.keys(item)[0] === key[0]
            ? { [key]: { color: color, visible: visible } }
            : item
        )
        setChangeChannelData(newColor)

    }

    //색 변경
    const handleColorChanged = (color: string, visible: boolean, key: any) => {
        console.log(key, visible);

        const newColor = changeChannelData.map((item: any) => Object.keys(item)[0] === key[0]
            ? { [key]: { color: color, visible: visible } }
            : item
        )
        setChangeChannelData(newColor)
    }

    const pNameArr: [] = graphItem !== undefined && graphItem.lengh !== 0 && graphItem.filter((el: any, idx: number) => {
        return Object.keys(el)[0].charAt(0) === 'P'
    })
    const tNameArr: [] = graphItem !== undefined && graphItem.lengh !== 0 && graphItem.filter((el: any, idx: number) => {
        return Object.keys(el)[0].charAt(0) === 'T'
    })

    const test = (idx: number) => {
        if (idx >= 5) return `${myStyle.boxes} ${myStyle.scroll}`
        return `${myStyle.boxes}`
    }


    //0428 seo null색상 랜덤부여
    pNameArr !== undefined && pNameArr.length !== 0 && pNameArr && pNameArr.map((item: object, idx: number) => {
        if (Object.values(item)[0].color === null) {
            return Object.values(item)[0].color = "#" + Math.round(Math.random() * 0xffffff).toString(16);
        }
    })
    tNameArr !== undefined && tNameArr.length !== 0 && tNameArr && tNameArr.map((item: object, idx: number) => {
        if (Object.values(item)[0].color === null) {
            return Object.values(item)[0].color = "#" + Math.round(Math.random() * 0xffffff).toString(16);
        }
    })


    return (
        <>
            <div className={myStyle.position}>
                <div className={myStyle.settingBtn}>
                    {!openSettingModal
                        ? <img onClick={() => openSetting()} src={setting} alt='' />
                        : <button onClick={() => handleSummit()} className={myStyle.saveIconPosition}>저장</button>
                    }
                </div>
                {!openSettingModal
                    ?
                    <div className={myStyle.nonClickbox}>
                        <div className={myStyle.pTitle}>압력</div>
                        <div className={pNameArr !== undefined && pNameArr.length !== 0 && pNameArr ? test(pNameArr.length) : undefined}>
                            {pNameArr !== undefined && pNameArr.length !== 0 && pNameArr &&
                                pNameArr.map((item: object, idx: number) =>
                                    <div key={idx} className={myStyle.boxItem}>
                                        <span className={myStyle.iconCustom}><Line fill={`${Object.values(item)[0].color}`} /></span>
                                        <span className={myStyle.textCustom} style={{ color: `${Object.values(item)[0].color}` }}>{Object.keys(item)}</span>
                                    </div>
                                )}
                        </div>
                        <div className={myStyle.tTitle}>온도</div>
                        <div className={tNameArr !== undefined && tNameArr.length !== 0 && tNameArr ? test(tNameArr.length) : undefined}>
                            {tNameArr !== undefined && tNameArr.length !== 0 && tNameArr &&
                                tNameArr.map((item: object, idx: number) =>
                                    <div key={idx} className={myStyle.boxItem}>
                                        <span className={myStyle.iconCustom}><Line fill={`${Object.values(item)[0].color}`} /></span>
                                        <span className={myStyle.textCustom} style={{ color: `${Object.values(item)[0].color}` }}>{Object.keys(item)}</span>
                                    </div>
                                )}
                        </div>
                    </div>
                    :
                    <div className={myStyle.clickbox}>
                        <div className={myStyle.pTitle}>압력</div>
                        <div className={pNameArr !== undefined && pNameArr.length !== 0 && pNameArr ? test(pNameArr.length) : undefined}>
                            {pNameArr !== undefined && pNameArr.length !== 0 && pNameArr &&
                                pNameArr.map((item: object, idx: number) =>
                                    <div key={idx} className={myStyle.settingBox}>
                                        <input className={myStyle.checkBox} type="checkbox"
                                            defaultChecked={defaultChecked && Object.values(item)[0].visible}
                                            onChange={(event) => {
                                                handleInputChecked(
                                                    Object.values(item)[0].color,
                                                    defaultChecked
                                                        ? !Object.values(item)[0].visible
                                                        : event.currentTarget.checked,
                                                    Object.keys(item),
                                                )
                                                setDefaultChecked(false);
                                            }} />
                                        <span className={myStyle.textCustom} style={{ color: `${Object.values(item)[0].color}` }}>{Object.keys(item)}</span>
                                        <div className={myStyle.colorBox}>
                                            <InputColor
                                                initialValue={`${Object.values(item)[0].color}` as any as string}
                                                onChange={(updateColor) => handleColorChanged(updateColor.hex, Object.values(item)[0].visible, Object.keys(item))}
                                                placement="right" />
                                        </div>
                                    </div>
                                )}
                        </div>
                        <div className={myStyle.tTitle}>온도</div>
                        <div className={tNameArr !== undefined && tNameArr.length !== 0 && tNameArr ? test(tNameArr.length) : undefined}>
                            {tNameArr !== undefined && tNameArr.length !== 0 && tNameArr &&
                                tNameArr.map((item: object, idx: number) =>
                                    <div key={idx} className={myStyle.settingBox}>
                                        <input className={myStyle.checkBox} type="checkbox"
                                            defaultChecked={defaultChecked && Object.values(item)[0].visible}
                                            onChange={(event) => {
                                                handleInputChecked(
                                                    Object.values(item)[0].color,
                                                    defaultChecked
                                                        ? !Object.values(item)[0].visible
                                                        : event.currentTarget.checked,
                                                    Object.keys(item),
                                                )
                                                setDefaultChecked(false);
                                            }} />
                                        <span className={myStyle.textCustom} style={{ color: `${Object.values(item)[0].color}` }}>{Object.keys(item)}</span>
                                        <div className={myStyle.colorBox}>
                                            <InputColor
                                                initialValue={`${Object.values(item)[0].color}` as any as string}
                                                onChange={(updateColor) => handleColorChanged(updateColor.hex, Object.values(item)[0].visible, Object.keys(item))}
                                                placement="right" />
                                        </div>
                                    </div>
                                )}
                        </div>
                    </div>
                }

            </div>

        </>
    )
};

export default ChannelSetting;

