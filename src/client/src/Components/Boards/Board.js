import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { getBoardRequest } from './Board/boardActions'
import { getQuestsRequest } from './Quests/questsActions'

const Board = (props) => {
	const dispatch = useDispatch()
	const id = props.match.params.id
	const board = useSelector((state) => state.board.board)
	useEffect(
		() => {
			dispatch(getBoardRequest(id))
			dispatch(getQuestsRequest(id))
		},
		[ id ]
	)

	return (
		<div>
			{board && (
				<div>
					<div>{board.name}</div>
					<div>{board.description}</div>
					<div className="drag-n-drop">
						{/* <div className="dnd-group">
							<div className="dnd-item">
								
							</div>
						</div> */}
					</div>
				</div>
			)}
		</div>
	)
}

export default Board
