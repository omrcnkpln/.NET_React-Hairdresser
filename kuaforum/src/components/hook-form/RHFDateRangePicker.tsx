import { DateRange, DateRangePicker } from '@mui/lab';
import { TextField, Box, FormControlLabelProps } from '@mui/material';
import React from 'react'
import { Controller, useFormContext } from 'react-hook-form';

type IProps = Omit<FormControlLabelProps, 'control'>;

interface Prop extends IProps{
    name: string;
  }

const RHFDateRangePicker = ({name , ...other}:Prop) => {
    const { control } = useFormContext();
    const [date, setDate] = React.useState<DateRange<Date>>([null, null]);
    return (
        <Controller
            control={control}
            name= {name}
            render={({ field }) => (
                <DateRangePicker
                    className="input"
                    startText="Check-in"
                    endText="Check-out"
                    onChange={(e:any) => setDate(e)}
                    showToolbar
                    value={date}
                    renderInput={(startProps:any, endProps:any) => (
                        <React.Fragment>
                            <TextField size='small' {...startProps} />
                            <Box sx={{ mx: 1}}>to</Box>
                            <TextField size='small' {...endProps} />
                        </React.Fragment>
                    )}
                />
            )}
            />
    )
}

export default RHFDateRangePicker