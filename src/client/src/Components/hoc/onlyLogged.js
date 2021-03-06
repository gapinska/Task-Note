import React from 'react'
import { useSelector } from 'react-redux'
import { useHistory } from 'react-router-dom'

export default function onlyLogged(WrappedComponent) {
	return function(props) {
		const username = useSelector((state) => state.user.username)
		const history = useHistory()

		if (username.trim() !== '') {
			console.log('niezalogowany')
			history.push('/')
		} else {
			console.log('zalogowany')
		}

		return <WrappedComponent {...props} />
	}
}
