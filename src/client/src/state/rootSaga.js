import { all, fork } from 'redux-saga/effects'
import { loginSaga } from '../Components/Login/state/loginSaga'
import { watchGetBoardsRequest } from '../Components/Boards/state/boardsSaga'
import { watchGetBoardRequest } from '../Components/Boards/Board/boardSaga'

export default function* rootSaga() {
	yield all([ fork(loginSaga), fork(watchGetBoardsRequest), fork(watchGetBoardRequest) ])
}
