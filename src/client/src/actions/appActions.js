export const ADD_USER = 'ADD_USER';
export const DELETE_USER = 'DELETE_USER';
export const EDIT_USER = 'EDIT_USER';

let idNum = 0


export const addUser = ({author, comment, rate}) => ({
    type: ADD_USER,
    payload: {
        author,
        comment,
        rate,
        id: ++idNum
    }
})

export const deleteUser = id => ({
    type: DELETE_USER,
    payload: {
        id
    }
})

export const editUser = data => ({
    type: EDIT_USER,
    payload: {
        ...data
    }
})






// {
//     type: ADD_RATE,
//     payload: {
//         author: 'jan kowalski',
//         rate: 5,
//         comment: 'bardzo dobra ksiazka'
//     }
// }
