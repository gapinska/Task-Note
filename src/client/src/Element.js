import React, {useState} from 'react'
import Form from './Form'

const Element = ({author, comment, id, rate}) => {
    const [isVisibleForm, setIsVisibleForm] = useState(false)
    const toogleElements = () => setIsVisibleForm(prev => !prev)

    const formOrButtonElement = isVisibleForm 
    ? 
    (<Form 
        author={author} 
        comment = {comment} 
        id = {id} 
        callback={toogleElements}
        rate={rate}
        />
        ) : (
        <button onClick={toogleElements}>
            Edytuj książkę
            </button>
            )
    return (
<li>
    <p>Author oceny: {author}</p>
    <p>Ocena: {rate}</p>
    <p>KOmentarz: {comment}</p>
    {formOrButtonElement}
</li>
    )
}

export default Element