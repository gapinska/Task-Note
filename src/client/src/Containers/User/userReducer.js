import { UPDATE_USER_DATA } from './userActions'

const initialState = {
    token: ''
};

const userReducer = (state = initialState, action) => {
    switch (action.type) {
        case UPDATE_USER_DATA:
            return {
                ...state,
                token: action.payload.token,
                ...action.payload.user
            };

        default:
            return state
    }
};

export default userReducer;