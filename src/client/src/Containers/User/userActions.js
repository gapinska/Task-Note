export const UPDATE_USER_DATA = 'UPDATE_USER_DATA';

export const updateUserData = ({token, user}) => ({
    type: UPDATE_USER_DATA,
    payload: {
        token,
        user
    }
});