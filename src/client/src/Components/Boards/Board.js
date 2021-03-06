import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { getBoardRequest } from './Board/boardActions'

const Board = (props) => {
	const dispatch = useDispatch()
	const id = props.match.params.id
	const board = useSelector((state) => state.board.board)
	useEffect(
		() => {
			dispatch(getBoardRequest(id))
		},
		[ id ]
	)

	return (
		<div>
			{board && (
				<div>
					<div>{board.name}</div>
					<div>{board.description}</div>
				</div>
			)}
		</div>
	)
}

export default Board
