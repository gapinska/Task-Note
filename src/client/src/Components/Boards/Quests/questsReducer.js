import { GET_QUESTS_REQUEST, GET_QUESTS_SUCCESS } from '../Quests/questsActions'
const initialState = {
	quests: {}
}

const questsReducer = (state = initialState, action) => {
	switch (action.type) {
		case GET_QUESTS_REQUEST:
			return {
				...state,
				id: action.payload?.id
			}
		case GET_QUESTS_SUCCESS:
			return {
				...state,
				quests: action.payload.quests
			}
		default:
			return state
	}
}

export default questsReducer
