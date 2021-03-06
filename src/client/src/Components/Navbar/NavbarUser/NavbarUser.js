import React from 'react'
import Box from '@material-ui/core/Box'
import Typography from '@material-ui/core/Typography'
import Avatar from "@material-ui/core/Avatar";
import useStyles from './styles'
import { useSelector } from "react-redux";

const NavbarUser = () => {
    const user = useSelector(state => state.user);
    const userInitialAvatar = (!!user.fullName ? user.fullName[0] : user.username[0]).toUpperCase();
    const classes = useStyles();

    return (
        <Box
            display="flex"
            justifyContent="space-between"
            alignItems="center">
            <Avatar className={classes.avatar}>{userInitialAvatar}</Avatar>
            <Typography component="p">{user.fullName}</Typography>
        </Box>
    )
};

export default NavbarUser