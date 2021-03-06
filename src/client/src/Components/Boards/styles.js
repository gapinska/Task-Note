import makeStyles from '@material-ui/core/styles/makeStyles'

const useStyles = makeStyles((theme) => ({
	form: {
		width: '100%',
		marginTop: theme.spacing(2)
	},
	submit: {
		margin: theme.spacing(2, 0)
	}
}))

export default useStyles
