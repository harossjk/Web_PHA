import React, { useEffect, useState } from 'react';
import myStyle from './DropDownMultiSelect.module.scss'
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import Checkbox from '@mui/material/Checkbox';
import ListItemText from '@mui/material/ListItemText';
import { ListItemIcon } from '@mui/material';
interface props {
    title?: String;
    options?: Array<any>;
    onValueChange: (data: any) => void;
    setValue?: any;
    value?: String;
}

const DropDownMultiSelect = ({ title, options, onValueChange }: props) => {

    const [selected, setSelected] = useState<Array<string> | undefined>([]);
    const isAllSelected =
        options !== undefined &&
        selected !== undefined &&
        options.length > 0 &&
        selected.length === options.length;

    const handleChange = (event: any) => {
        const value = event.target.value;
        if (value[value.length - 1] === "all") {
            setSelected(selected !== undefined && options !== undefined && selected.length === options.length ? [] : options);
            onValueChange(selected !== undefined && options !== undefined && selected.length === options.length ? [] : options);
            return;
        }
        setSelected(value);
        onValueChange(value);
    };

    return (
        <div className={myStyle.container}>
            <div className={myStyle.title}>{title}</div>
            <div className={myStyle.contents}>
                <FormControl className={myStyle.formControl}>
                    <Select
                        labelId="mutiple-select-label"
                        multiple
                        value={selected}
                        onChange={handleChange}
                        renderValue={(selected) => selected.join(", ")}
                        MenuProps={MenuProps}
                        className={myStyle.formSelect}
                    >
                        <MenuItem
                            value="all"
                            classes={{
                                root: isAllSelected ? myStyle.selectedAll : ""
                            }}
                        >
                            <ListItemIcon>
                                <Checkbox
                                    classes={{ indeterminate: myStyle.indeterminateColor }}
                                    checked={isAllSelected}
                                    indeterminate={
                                        options !== undefined &&
                                        selected !== undefined &&
                                        selected.length > 0 &&
                                        selected.length < options.length
                                    }
                                />
                            </ListItemIcon>
                            <ListItemText
                                classes={{ primary: myStyle.selectAllText }}
                                primary="전체 선택"
                            />
                        </MenuItem>
                        {options !== undefined && options.map((option) => (
                            <MenuItem key={option} value={option}>
                                <ListItemIcon>
                                    <Checkbox checked={selected !== undefined && selected.indexOf(option) > -1} />
                                </ListItemIcon>
                                <ListItemText primary={option} />
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </div>
        </div>

    );

    // const handleChange = (event: any) => {
    //     const {
    //         target: { value },
    //     } = event;
    //     setPersonName(
    //         typeof value === 'string' ? value.split(',') : value,
    //     );
    //     console.log("event.target.value", event.target.value);
    //     onValueChange(event.target.value);
    // };

    // return (
    //     <div className={myStyle.container}>
    //         <div className={myStyle.title}>{title}</div>
    //         <div className={myStyle.contents}>
    //             <FormControl className={myStyle.formControl}>
    //                 <Select
    //                     labelId="multiple-checkbox-label"
    //                     id="multiple-checkbox"
    //                     multiple
    //                     value={personName}
    //                     onChange={handleChange}
    //                     renderValue={(selected) => selected.join(', ')}
    //                     MenuProps={MenuProps}
    //                     className={myStyle.formSelect}
    //                 >
    //                     {options !== undefined && options.map((el) => (
    //                         <MenuItem key={el} value={el}>
    //                             <Checkbox checked={personName.indexOf(el) > -1} />
    //                             <ListItemText primary={el} />
    //                         </MenuItem>
    //                     ))}
    //                 </Select>
    //             </FormControl>
    //         </div>
    //     </div>
    // );
};

export default DropDownMultiSelect;

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 200,
        },
    },
};
