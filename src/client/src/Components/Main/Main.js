import React from 'react'
import Login from '../Login';
import Welcome from '../Welcome';
import Box from '@material-ui/core/Box'
import { useSelector } from 'react-redux';

const Main = () => {
    const isUserLogged = useSelector(state => state.login.isUserLogged);

    return (
        <Box
            component="main"
            p={{xs: 1, sm: 2, md: 3}}
            maxWidth="lg">
            {isUserLogged ? <Welcome /> : <Login/>}
        </Box>
    )
};

export default Main