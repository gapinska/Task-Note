import React from 'react'
import Container from '@material-ui/core/Container'
import Typography from '@material-ui/core/Typography'
import { useSelector } from 'react-redux'
import Boards from '../Boards/Boards'
import NewBoard from '../NewBoard'
import { BrowserRouter as Router, Link, Switch, Route } from 'react-router-dom'
import Board from '../Boards/Board'
import onlyLogged from '../../Components/hoc/onlyLogged'

const Welcome = () => {
	const username = useSelector((state) => state.user.username)

	return (
		<Container>
			<Typography variant="h5">Welcome, {username}</Typography>
			<Router>
				<nav>
					<button>
						<Link to="/">Boards</Link>
					</button>
					<button>
						<Link to="/create-new-board">Create new board</Link>
					</button>
				</nav>
				<Switch>
					<Route path="/create-new-board" component={NewBoard} />
					<Route path="/:id" component={Board} />
					<Route path="/" exact component={Boards} />
				</Switch>
			</Router>
		</Container>
	)
}

export default Welcome
