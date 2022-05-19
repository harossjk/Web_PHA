import React, { useEffect, useState } from 'react';
import myStyle from './DropDown.module.scss'
import MenuItem from '@mui/material/MenuItem';
import FormHelperText from '@mui/material/FormHelperText';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
interface props {
    defaultValue?: string;
    title?: string;
    options?: Array<any>;
    onValueChange: (data: string) => void;
    setValue?: any;
    value?: string;
    boxType?: string;
}

const DropDown = ({ defaultValue, title, options, onValueChange, boxType }: props) => {

    const [value, setValue] = useState(defaultValue);
    const [isDefaultValue, setIsDefaultValue] = useState<Boolean>(true);
    const [type, setType] = useState(options !== undefined && boxType === "small" ? options[0] : "");


    useEffect(() => {
        setIsDefaultValue(true);
    }, [defaultValue])


    const handleChange = (event: any) => {
        setIsDefaultValue(false)
        setType(event.target.value);
        setValue(event.target.value);
        onValueChange(event.target.value);
    };

    return (
        <>
            {boxType !== "small"
                ?
                <div className={myStyle.container}>
                    <div className={myStyle.title}>{title}</div>
                    <div className={myStyle.contents}>
                        <FormControl className={myStyle.formControl} >
                            <Select
                                defaultValue={defaultValue}
                                onChange={handleChange}
                                displayEmpty
                                className={myStyle.formSelect}
                            >
                                {options !== undefined && options && options.map((el, idx) => <MenuItem key={idx} value={el}>{el}</MenuItem>)}
                            </Select>
                        </FormControl>
                    </div>
                </div >
                :
                <div className={myStyle.containerSmall}>
                    <div className={myStyle.contents}>
                        <FormControl className={myStyle.formControl}  >
                            <Select
                                value={isDefaultValue ? defaultValue : value}
                                onChange={handleChange}
                                displayEmpty
                                inputProps={{ 'aria-label': 'Without label' }}
                                className={myStyle.formSelect}

                            >
                                {options !== undefined && options && options.map((el, idx) => <MenuItem key={idx} value={el}>{el}</MenuItem>)}
                            </Select>
                        </FormControl>
                    </div>
                </div >
            }

        </>
    )

};

export default DropDown;
