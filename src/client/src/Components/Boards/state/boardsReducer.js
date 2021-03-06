import { GET_BOARDS_SUCCESS } from './boardsActions'

const initialState = {
	boards: []
}

const boardsReducer = (state = initialState, action) => {
	switch (action.type) {
		case GET_BOARDS_SUCCESS:
			return {
				...state,
				boards: action.payload.boards
			}
		default:
			return state
	}
}

export default boardsReducer
