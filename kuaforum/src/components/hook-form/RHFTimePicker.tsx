// form
import { useFormContext, Controller } from 'react-hook-form';
// @mui
import { FormControlLabel, FormControlLabelProps, TextField } from '@mui/material';
import StaticTimePicker from '@mui/lab/StaticTimePicker';
import { useEffect, useState } from 'react';


// ----------------------------------------------------------------------

type IProps = Omit<FormControlLabelProps, 'control'>;

interface Props extends IProps {
  name: string;
  setValue:any
}

export default function RHFTimePicker({ name,setValue, ...other }: Props) {
  const { control } = useFormContext();
  const [time, setTime] = useState<Date | null>(new Date());
  const handleChange=(value:Date |null)=>{
      setTime(value as Date)
    setValue(name,time)
  }
  useEffect(() => {
    setValue(name,time)
  }, [time]);
  
  return (
    <FormControlLabel
      control={
        <Controller
          name={name}
          control={control}
          render={({ field }) => 
          <StaticTimePicker
          orientation="landscape"
          {...field}
          openTo="minutes"
          value={time}
          onChange={(value)=>handleChange(value)}
          renderInput={(params) => <TextField  {...params} />}
        />}
        />
      }
      {...other}
    />
  );
}
