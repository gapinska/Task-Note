import { takeEvery, call, select, put } from 'redux-saga/effects'
import * as actions from './boardActions'
import { API_URL } from '../../../utils/config/urls'
import axios from 'axios'

function* getBoard({ payload }) {
	const state = yield select()
	try {
		const result = yield call(() =>
			axios.get(`${API_URL}Boards/${payload}`, {
				headers: {
					Authorization: state.user.token ? `Bearer ${state.user.token}` : null
				}
			})
		)
		yield put(actions.getBoardSuccess({ board: result.data }))
	} catch (e) {
		console.log('error')
	}
}

export function* watchGetBoardRequest() {
	yield takeEvery(actions.GET_BOARD_REQUEST, getBoard)
}
