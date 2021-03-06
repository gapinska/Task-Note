import { LOGIN_USER_ERROR, LOGIN_USER_START, LOGIN_USER_SUCCESS, USER_DATA_UPDATE_SUCCESS } from './loginActions'

const initialState = {
    username: '',
    isLoading: false,
    isUserLogged: false
};

const loginReducer = (state = initialState, action) => {
    switch (action.type) {
        case LOGIN_USER_START:
            return {
                ...state,
                username: action.payload?.username,
                isLoading: true,
            };
        case LOGIN_USER_SUCCESS:
            return {
                ...state,
            };
        case LOGIN_USER_ERROR:
            return {
                ...state,
                isLoading: false,
            };
        case USER_DATA_UPDATE_SUCCESS:
            return {
                ...state,
                isUserLogged: true,
                isLoading: false,
            };
        default:
            return state
    }
};

export default loginReducer;