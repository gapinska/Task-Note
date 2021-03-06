import * as actions from '../actions/appActions'

export const appReducer = (state= [], action) => {
    switch (action.type) {
        case actions.ADD_USER:
            return [...state, action.payload]
        case actions.DELETE_USER:            
            return state.filter(currentStateElement=> currentStateElement.id !== action.payload.id)
        case actions.EDIT_USER:
            return state.map(currentElement => {
                if(currentElement.id !== action.payload.id) {
                    return currentElement
                } else {
                    const {author, comment, rate, id} = action.payload

                    return ({
                        author,
                        comment,
                        id,
                        rate,
                    })
                }                
            })
        default:
            return state
    }
}