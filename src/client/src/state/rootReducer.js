import { combineReducers } from 'redux'
import { appReducer } from '../reducers/appReducer'
import loginReducer from '../Components/Login/state/loginReducer'
import boardsReducer from '../Components/Boards/state/boardsReducer'
import userReducer from '../Containers/User/userReducer'
import boardReducer from '../Components/Boards/Board/boardReducer'
import { persistReducer } from 'redux-persist'
import storage from 'redux-persist/lib/storage'

const persistConfig = {
	key: 'root',
	storage
}

const rootReducer = combineReducers({
	rates: appReducer,
	login: loginReducer,
	user: userReducer,
	boards: boardsReducer,
	board: boardReducer
})

export default persistReducer(persistConfig, rootReducer)
