import { takeEvery, call, select, put } from 'redux-saga/effects'
import * as actions from './boardsActions'
import { API_URL } from '../../../utils/config/urls'
import axios from 'axios'

function* getBoards() {
	const state = yield select()
	try {
		const result = yield call(() =>
			axios.get(API_URL + 'Boards', {
				headers: {
					Authorization: state.user.token ? `Bearer ${state.user.token}` : null
				}
			})
		)
		yield put(actions.getBoardsSuccess({ boards: result.data }))
	} catch (e) {
		console.log('error')
	}
}

export function* watchGetBoardsRequest() {
	yield takeEvery(actions.GET_BOARDS_REQUEST, getBoards)
}
