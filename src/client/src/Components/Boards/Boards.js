import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { getBoardsRequest } from './state/boardsActions'
import BoardItem from './BoardItem'
import { Link } from 'react-router-dom'

const Boards = () => {
	const dispatch = useDispatch()

	const boards = useSelector((state) => state.boards.boards)
	useEffect(() => {
		dispatch(getBoardsRequest())
	}, [])

	return (
		<div>
			{boards &&
				boards.map((board) => (
					<div key={board.id}>
						<Link to={`/${board.id}`}>
							<BoardItem board={board} />
						</Link>
					</div>
				))}
		</div>
	)
}

export default Boards
