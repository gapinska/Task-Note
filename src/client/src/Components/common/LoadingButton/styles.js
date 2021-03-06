import makeStyles from "@material-ui/core/styles/makeStyles";

const useStyles = makeStyles((theme) => ({
    spinner: {
        marginLeft: theme.spacing(1),
        color: theme.palette.grey[500]
    },
}));

export default useStyles;