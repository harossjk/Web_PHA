import React, { useEffect, useState } from 'react'
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import myStyle from './TimePicker.module.scss'
import { dateToString } from '../../../utils/Utils';

interface props {
    title?: String;
    onTimeChange: (data: string) => void;
    defaultValue?: string;
}


const TimePicker = ({ defaultValue, title, onTimeChange }: props) => {
    const [value, setValue] = useState<string>("");
    useEffect(() => {
        if (defaultValue !== undefined) setValue(defaultValue);
    }, [])

    const handleDateChange = (event: any) => {
        setValue(event.target.value)
        onTimeChange(event.target.value);
    };
    return (
        <div className={myStyle.container}>
            <div className={myStyle.title}>{title}</div>
            <div className={myStyle.contents}>
                <TextField
                    id="date"
                    type="date"
                    InputLabelProps={{
                        shrink: true,
                    }}
                    className={myStyle.textField}
                    onChange={handleDateChange}
                    value={value}
                />
            </div>
        </div>
    )
}
export default TimePicker;
