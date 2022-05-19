import React, { useState } from 'react'
import TextField from '@mui/material/TextField';
import myStyle from './InputBox.module.scss';

interface props {
  defaultValue?: string;
  title?: String;
  onValueChange: (data: string) => void;
}

const InputBox = ({ defaultValue, title, onValueChange }: props) => {
  const [value, setValue] = useState(defaultValue);

  const handleChange = (event: any) => {
    setValue(event.target.value);
    onValueChange(event.target.value);
  };

  return (
    <>
      <div className={myStyle.container}>
        <div className={myStyle.title}>{title}</div>
        <div className={myStyle.contents}>
          <TextField
            value={value}
            InputProps={{ className: myStyle.input }}
            onChange={handleChange}
          />

        </div>
      </div>
    </>
  )
}

export default InputBox