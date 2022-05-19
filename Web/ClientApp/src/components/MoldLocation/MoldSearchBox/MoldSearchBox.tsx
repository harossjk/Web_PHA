import React, { useState } from 'react'
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import myStyle from './MoldSearchBox.module.scss'
import Popper from "@material-ui/core/Popper";
import { makeStyles } from "@material-ui/core/styles";
import { border, borderRadius, margin } from '@mui/system';
import Person from '@material-ui/icons/Person'
import MoldSearchResult from '../MoldSearchResult';
interface props {
  title?: String;
  options: Array<string>;
  onValueChange: (data: string) => void;
  setValue?: any;
  value?: String;
  boxType?: String;
}


const useStyles = makeStyles({
  paper: {
    borderRadius: 0,
    background: "#1f2328",
    color: "white",
    fontSize: "16px",
    border: "1px solid rgb(117, 117, 117)",
  },
  option: {
    borderBottom: "1px solid rgb(117, 117, 117)",
    "&:hover": {
      backgroundColor: "#c2d8ed !important",
      color: "#1f2328 !important",
      fontWeight: "600"
    },
    "&:active": {
      backgroundColor: "#f2f6fc !important",
    },
  }
});

const MoldSearchBox = ({ title, options, onValueChange, setValue, value, boxType }: props) => {
  const [open, setOpen] = useState(true);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleChange = (event: any, value: any) => {
    const rfid = value !== null && value.split(" ")[0].substr(1, value.split(" ")[0].length - 2)
    onValueChange(rfid);


  };

  const classes = useStyles();

  return (
    <div className={myStyle.container}>
      <div className={myStyle.contents}>
        <div className={myStyle.contentsTitle}>금형 위치 조회</div>
        <Autocomplete
          popupIcon={false}
          sx={{
            ".MuiOutlinedInput-root": {
              borderRadius: 0
            }
          }}
          disablePortal
          open={open}
          onOpen={handleOpen}
          onClose={() => setOpen(true)}
          id="combo-box"
          options={options}
          renderInput={(params) => <TextField {...params} placeholder="금형명을 입력해주세요.." />}
          onChange={handleChange}
          className={myStyle.formSelect}
          // PopperComponent={PopperMy}
          classes={{ paper: classes.paper, option: classes.option }}


        />
        {/* <div className={myStyle.title}>금형명</div> */}
      </div>
    </div>
  )
}

export default MoldSearchBox;

