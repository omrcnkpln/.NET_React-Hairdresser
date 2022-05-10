// form
import { useFormContext, Controller } from 'react-hook-form';
// @mui
import { Autocomplete, Box, MenuItem, TextField, TextFieldProps } from '@mui/material';

// ----------------------------------------------------------------------

interface IProps {
  name: string;
  options:object[];
  label:string;
    setvalue:any;
}

export default function RHFAutocomplete({ name,options,label,setvalue }:IProps) {
  const { control } = useFormContext();

  return (
   <Controller
                        name={name}
                        control={control}
                        render={({ field }) => (
                            <Autocomplete
                                size='small'
                                {...field}
                                options={options}
                                onChange={(event, newValue) => { setvalue("name",newValue?.id)}}
                                getOptionLabel={(option) => option.name}
                                renderOption={(props, option) => (
                                    <MenuItem  {...props}  key={option.id}>
                                        <option>{option.name}</option>
                                    </MenuItem>
                                )}
                                renderInput={(params) => (
                                    <TextField
                                        {...params}
                                        label={label}
                                        variant="outlined"
                                    />
                                )}
                            />)}
                    />
  );
}