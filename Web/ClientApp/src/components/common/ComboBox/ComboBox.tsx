import React, { useState } from 'react'
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import myStyle from './ComboBox.module.scss'
import Popper from "@material-ui/core/Popper";
import { withStyles } from "@material-ui/core/styles";
interface props {
  title?: String;
  options: Array<string>;
  onValueChange: (data: string) => void;
  setValue?: any;
  value?: String;
  boxType?: String;
  placeholder?: string;
  defaultValue?: any;
}

const ComboBox = ({ defaultValue, placeholder, title, options, onValueChange, boxType }: props) => {
  const [value, setValue] = useState(defaultValue);

  const handleChange = (event: any, value: any) => {
    console.log("value", value);

    onValueChange(value);
    setValue(value);
  };

  const PopperMy = (props: any) => {
    return <Popper {...props} style={{ width: "fitContent", marginLeft: "120px" }} />;
  };

  return (
    <div className={myStyle.container}>
      <div className={myStyle.title}>{title}</div>
      <div className={myStyle.contents}>
        <Autocomplete
          disablePortal
          id="combo-box"
          options={options}
          renderInput={(params) => <TextField {...params} placeholder={placeholder} />}
          onChange={handleChange}
          className={myStyle.formSelect}
          PopperComponent={PopperMy}
          value={value}
        />
      </div>
    </div>
  )
}

export default ComboBox;

