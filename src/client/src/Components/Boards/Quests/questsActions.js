export const GET_QUESTS_REQUEST = 'GET_QUESTS_REQUEST'
export const GET_QUESTS_SUCCESS = 'GET_QUESTS_SUCCESS'

export const getQuestsRequest = (id) => ({
	type: GET_QUESTS_REQUEST,
	payload: id
})

export const getQuestsSuccess = ({ quests }) => ({
	type: GET_QUESTS_SUCCESS,
	payload: { quests }
})
