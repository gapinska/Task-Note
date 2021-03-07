export const GET_BOARD_REQUEST = 'GET_BOARD_REQUEST'
export const GET_BOARD_SUCCESS = 'GET_BOARD_SUCCESS'

export const getBoardRequest = (id) => ({
	type: GET_BOARD_REQUEST,
	payload: id
})

export const getBoardSuccess = ({ board }) => ({
	type: GET_BOARD_SUCCESS,
	payload: { board }
})
