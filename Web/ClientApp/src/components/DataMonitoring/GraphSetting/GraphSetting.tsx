import React, { useState } from 'react';
import myStyle from './GraphSettting.module.scss';
import InputColor from 'react-input-color';
import useStore from '../../../stores';

interface props {
    dataKeyArray: string[];
    settingComplete: () => void;
    selectMold?: number;
    customColor: any;
}

const GraphSetting = ({ dataKeyArray, settingComplete, selectMold, customColor }: props) => {

    const { MSMDStore } = useStore();
    const [colorPicker, setColorPicker] = useState(customColor)
    const [color, setColor] = useState({})
    const [visibleGraph, setVisibleGraph] = useState<Array<any>>([])

    //저장버튼 클릭이벤트
    const handleSummit = () => {
        MSMDStore.changeGraph!!(visibleGraph, selectMold!!);
        MSMDStore.changeColor!!(colorPicker, selectMold!!); // 서버에 컬러저장
        settingComplete();
    }

    //체크 유무
    const handleInputChecked = (item: string) => {
        setVisibleGraph([...visibleGraph, item])
    }

    //색 변경
    const handleColorChanged = (color: string, key: any) => {
        const newColor = colorPicker.map((item: any) => Object.keys(item)[0] === key
            ? { [key]: color }
            : item
        )
        setColorPicker(newColor)
    }
    return <>
        <div className={myStyle.container}>
            <div>header</div>
            <div>
                {dataKeyArray.map((item, idx) => {
                    return (
                        <p>
                            <input type="checkbox" onChange={() => handleInputChecked(item)} /> {item}
                            <InputColor
                                initialValue="#a6afe05e"
                                onChange={(updateColor) => handleColorChanged(updateColor.hex, item)}
                                placement="right" />
                        </p>
                    )
                })}
            </div>
            <div>
                <button onClick={() => handleSummit()}>저장</button>
            </div>
        </div>
    </>;
};

export default GraphSetting;
