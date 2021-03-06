import React from 'react'
import Container from '@material-ui/core/Container';
import TextField from '@material-ui/core/TextField';
import Typography from '@material-ui/core/Typography';
import LoadingButton from '../common/LoadingButton/LoadingButton';
import useStyles from './styles'
import { useForm } from 'react-hook-form';
import { useDispatch, useSelector } from 'react-redux';
import { loginUserStart } from './state/loginActions';

const Login = () => {
    const {errors, register, handleSubmit} = useForm({
        mode: 'onBlur',
    });
    const classes = useStyles();
    const dispatch = useDispatch();
    const isLoading = useSelector(state => state.login.isLoading);
    const onSubmit = (data) => {
        dispatch(loginUserStart(data))
    };

    return (
        <Container maxWidth="xs">
            <Typography variant="h5">Login</Typography>
            <form className={classes.form} onSubmit={handleSubmit(onSubmit)} mx={5}>
                <TextField
                    variant="outlined"
                    required
                    id="username"
                    label="Username"
                    name="username"
                    inputRef={register({required: true})}
                    error={errors.username?.type === 'required'}
                    helperText={errors.username?.type === 'required' && 'This is required'}
                    fullWidth
                    margin="normal"
                />
                <TextField
                    variant="outlined"
                    required
                    name="password"
                    label="Password"
                    type="password"
                    id="password"
                    inputRef={register({required: true})}
                    error={errors.password?.type === 'required'}
                    helperText={errors.password?.type === 'required' && 'This is required'}
                    autoComplete="current-password"
                    fullWidth
                    margin="normal"
                />
                <LoadingButton
                    type={"submit"}
                    variant="contained"
                    color="primary"
                    fullWidth
                    className={classes.submit}
                    loading={isLoading}>
                    Login
                </LoadingButton>
            </form>
        </Container>
    )
};

export default Login