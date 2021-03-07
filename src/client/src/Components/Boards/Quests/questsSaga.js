import { takeEvery, call, select, put } from 'redux-saga/effects'
import * as actions from './questsActions'
import { API_URL } from '../../../utils/config/urls'
import axios from 'axios'

function* getQuests({ payload }) {
	const state = yield select()
	try {
		const result = yield call(() =>
			axios.get(`${API_URL}boards/${payload}/Quests`, {
				headers: {
					Authorization: state.user.token ? `Bearer ${state.user.token}` : null
				}
			})
		)
		yield put(actions.getQuestsSuccess({ quests: result.data }))
	} catch (e) {
		console.log('error')
	}
}

export function* watchGetQuestsRequest() {
	yield takeEvery(actions.GET_QUESTS_REQUEST, getQuests)
}
