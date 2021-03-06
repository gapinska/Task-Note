import React from 'react'

const BoardItem = ({ board }) => {
	return (
		<div>
			<div>{board.name}</div>
			<div>{board.description}</div>
		</div>
	)
}

export default BoardItem
