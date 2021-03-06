import { all, call, put, takeLatest } from 'redux-saga/effects'
import {
    LOGIN_USER_ERROR,
    LOGIN_USER_START,
    LOGIN_USER_SUCCESS,
    loginUserError,
    loginUserSuccess,
    userDataUpdateSuccess
} from './loginActions'
import { UPDATE_USER_DATA, updateUserData } from '../../../Containers/User/userActions'
import axios from 'axios'
import { toast } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'

export function* loginUserRequest({payload}) {
    try {
        const response = yield call(() => axios.post('/api/auth/login', {
            "username": payload?.username,
            "password": payload?.password
        }));
        yield put(loginUserSuccess(response));
    } catch (err) {
        yield put(loginUserError(err));
    }
}

export function* loginUserRequestError(action) {
    const errorText = action.payload.response?.statusText ?
        action.payload.response?.statusText :
        'Oops, something went wrong. Try again later.';

    if (action.payload.response?.status === 401) {
        yield toast.error('Wrong username or password');
    } else {
        yield toast.error(errorText);
    }
}

export function* loginUserRequestSuccess(action) {
    if (action.payload.status === 200) {
        yield put(updateUserData(action.payload.data));
        toast.success('Login successful.')
    } else {
        toast.error('Oops, something went wrong. Try again later.');
    }
}

export function* updateLoginStateUserLogged(action) {
    yield put(userDataUpdateSuccess())
}

export function* loginSaga() {
    yield all([
        takeLatest(LOGIN_USER_START, loginUserRequest),
        takeLatest(LOGIN_USER_SUCCESS, loginUserRequestSuccess),
        takeLatest(LOGIN_USER_ERROR, loginUserRequestError),
        takeLatest(UPDATE_USER_DATA, updateLoginStateUserLogged)
    ])
}
