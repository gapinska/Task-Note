import React, {useState} from 'react'
import {addUser, editUser} from './actions/appActions'
import {connect} from 'react-redux' 

const Form = ({
    addUser,
    editUser,
    author='',
    comment='',
    callback,
    id,
    rate
}) => {
    const [authorInput, setAuthorInput] = useState(author)
const [rateInput, setRateInput] = useState(rate)
const [commentInput, setCommentInput] = useState(comment)

const handleChangeAuthor = event => setAuthorInput (event.target.value)

const handleChangeRate = event => setRateInput (event.target.value)

const handleCommentChange = event => setCommentInput (event.target.value)

const handleOnSubmit = event => {
    event.preventDefault()

    if(!authorInput.length) {
        return
    }

    const rateObject = {
        author: authorInput,
        comment: commentInput,
        id, 
        rate: Number(rateInput)
    }

    console.log(rateObject)

    id ? editUser(rateObject) : addUser(rateObject)

    if(id) {
        callback()
    }
}

return (
    <form onSubmit={handleOnSubmit}>
        <div>
            <label htmlFor="">
                Autor:
                <input
                onChange={handleChangeAuthor}
                type="text"
                value={authorInput}
                />
            </label>
        </div>
        <div>
            <label htmlFor="">
                Ocena:
                <input
                onChange={handleChangeRate}
                type="number"
                value={rateInput}
                />
            </label>
        </div>
        <div>
            <label htmlFor="">
                Komentarz:
                <input
                onChange={handleCommentChange}
                type="text"
                value={commentInput}
                />
            </label>
        </div>
        <button type="submit">
            {id ? 'Edycja oceny' : 'Dodaj ocenę'}
        </button>
    </form>
)
}

const connectActionsToProps = ({
    addUser,
    editUser
})

const FormConsumer = connect(null, connectActionsToProps)(Form)
export default FormConsumer