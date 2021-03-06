export const LOGIN_USER_START = 'LOGIN_USER_START';
export const LOGIN_USER_SUCCESS = 'LOGIN_USER_SUCCESS';
export const LOGIN_USER_ERROR = 'LOGIN_USER_ERROR';
export const USER_DATA_UPDATE_SUCCESS = 'USER_DATA_UPDATE_SUCCESS';

export const loginUserStart = ({username, password}) => ({
    type: LOGIN_USER_START,
    payload: {
        username,
        password
    }
});

export const loginUserSuccess = (response) => ({
    type: LOGIN_USER_SUCCESS,
    payload: response
});

export const loginUserError = (response) => ({
    type: LOGIN_USER_ERROR,
    payload: response
});

export const userDataUpdateSuccess = () => ({
    type: USER_DATA_UPDATE_SUCCESS
});