import React from 'react'
import { useForm } from 'react-hook-form'

const NewBoard = () => {
	const { register, handleSubmit, watch, errors } = useForm()

	const onSubmit = (data) => {
		console.timeLog(data)
	}

	return (
		<div className="form-container">
			<form onSubmit={handleSubmit(onSubmit)}>
				<h3 className="form-title">Add a new board</h3>
				<input
					className="input-form"
					name="board"
					defaultValue="board"
					ref={register({ required: true, minLength: 2 })}
					placeholder="Enter your board title..."
				/>
				<textarea
					type="text"
					className="input-form"
					name="content"
					ref={register({ required: true })}
					placeholder="Board description"
				/>
				{/* errors will return when field validation fails  */}
				{errors.exampleRequired && <span>This field is required</span>}

				<input type="submit" />
			</form>
		</div>
	)
}

export default NewBoard
