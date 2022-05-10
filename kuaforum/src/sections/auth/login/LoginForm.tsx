import * as Yup from 'yup';
import { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
// form
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
// @mui
import { Link, Stack, Alert, IconButton, InputAdornment, Dialog, DialogTitle, Button, DialogActions, DialogContent } from '@mui/material';
import { LoadingButton } from '@mui/lab';
// routes
import { PATH_AUTH } from '../../../routes/paths';
// hooks
import useAuth from '../../../hooks/useAuth';
import useIsMountedRef from '../../../hooks/useIsMountedRef';
// components
import Iconify from '../../../components/Iconify';
import { FormProvider, RHFTextField, RHFCheckbox } from '../../../components/hook-form';
import useLocales from '../../../hooks/useLocales';
// ----------------------------------------------------------------------

type FormValuesProps = {
  userName: string;
  password: string;
  remember: boolean;
  afterSubmit?: string;
};

type Props = {
  open: boolean;
  setOpenDialog: any;
};

export default function LoginForm({ open, setOpenDialog }: Props) {

  const { translate } = useLocales();

  const { login } = useAuth();

  const isMountedRef = useIsMountedRef();
  const [error, setError] = useState('')
  const [showPassword, setShowPassword] = useState(false);

  const handleCloseDialog = () => {
    setOpenDialog(false);
  };

  const LoginSchema = Yup.object().shape({
    userName: Yup.string().required(translate('Email is required')),
    password: Yup.string().required(translate('Password is required')),
  });

  const defaultValues = {
    userName: '',
    password: '',
    remember: true,
  };

  const methods = useForm<FormValuesProps>({
    resolver: yupResolver(LoginSchema),
    defaultValues,
  });

  const {
    reset,
    //setError,
    handleSubmit,
    formState: { isSubmitting },
  } = methods;

  const onSubmit = async (data: FormValuesProps) => {
    try {
      await login(data.userName, data.password);
    } catch (error) {
      reset();
      if (isMountedRef.current) {
        setError(error as string);
      }
    }
  };

  return (
    <Dialog
      sx={{ '& .MuiDialog-paper': { width: '30%', maxHeight: 600 } }}
      maxWidth="md"
      open={open}
      onClose={handleCloseDialog}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogTitle>Giriş Yap</DialogTitle>
      <FormProvider methods={methods} onSubmit={handleSubmit(onSubmit)}>
      <DialogContent>

        <Stack spacing={3}>
          {!!error && <Alert severity="error">{error}</Alert>}

          <RHFTextField name="userName" label="Kullanıcı Adı" />

          <RHFTextField
            name="password"
            label="Şifre"
            type={showPassword ? 'text' : 'password'}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton onClick={() => setShowPassword(!showPassword)} edge="end">
                    <Iconify icon={showPassword ? 'eva:eye-fill' : 'eva:eye-off-fill'} />
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />
        </Stack>

        <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ my: 1 }}>
          <RHFCheckbox name="remember" label="Beni Hatırla" />
          <Link component={RouterLink} variant="subtitle2" to={PATH_AUTH.resetPassword}>
            Şifremi Unuttum
          </Link>

        </Stack>
          <LoadingButton
              fullWidth
              size="large"
              type="submit"
              variant="contained"
              loading={isSubmitting}
            >
              Giriş Yap
            </LoadingButton>
        </DialogContent>
      </FormProvider>
    </Dialog>
  );
}
