export const GET_BOARDS_REQUEST = 'GET_BOARDS_REQUEST'
export const GET_BOARDS_SUCCESS = 'GET_BOARDS_SUCCESS'

export const getBoardsRequest = () => ({
	type: GET_BOARDS_REQUEST
})

export const getBoardsSuccess = ({ boards }) => ({
	type: GET_BOARDS_SUCCESS,
	payload: { boards }
})
