import React from 'react'
import NavbarUser from './NavbarUser';
import Box from '@material-ui/core/Box'
import Typography from '@material-ui/core/Typography'
import { useSelector } from "react-redux";

const Navbar = () => {
    const isUserLogged = useSelector(state => state.login.isUserLogged);

    return (
        <Box
            component="nav"
            bgcolor="primary.main"
            color="primary.contrastText"
            py={2}
            px={4}
            boxShadow={3}
            display="flex"
            justifyContent="space-between"
            alignItems="center"
            zIndex="appBar">
            <Typography component="h1">TaskNote</Typography>
            {isUserLogged &&
                <NavbarUser />
            }
        </Box>
    )
};

export default Navbar