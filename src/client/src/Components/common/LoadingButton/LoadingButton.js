import React from 'react'
import Button from '@material-ui/core/Button';
import CircularProgress from '@material-ui/core/CircularProgress'
import useStyles from "./styles";

const LoadingButton = (props) => {
    const classes = useStyles();
    const {
        children,
        loading,
        ...rest
    } = props;

    return (
        <Button
            disabled={loading}
            {...rest}>
            {children}
            {loading && <CircularProgress
                className={classes.spinner}
                size={20}
            />
            }
        </Button>
    )
};

export default LoadingButton