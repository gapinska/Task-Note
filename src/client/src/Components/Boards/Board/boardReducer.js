import { GET_BOARD_REQUEST, GET_BOARD_SUCCESS } from '../Board/boardActions'

const initialState = {
	board: {}
}

const boardReducer = (state = initialState, action) => {
	switch (action.type) {
		case GET_BOARD_REQUEST:
			return {
				...state,
				id: action.payload?.id
			}
		case GET_BOARD_SUCCESS:
			return {
				...state,
				board: action.payload.board
			}
		default:
			return state
	}
}

export default boardReducer
