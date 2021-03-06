import { applyMiddleware, compose, createStore } from 'redux'
import persistReducer from './rootReducer'
import rootSaga from './rootSaga';
import { persistStore } from 'redux-persist'
import createSagaMiddleware from 'redux-saga'

const sagaMiddleware = createSagaMiddleware();
const composeEnhancers = (typeof window !== 'undefined' && window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__) || compose;
const store = createStore(
    persistReducer,
    composeEnhancers(applyMiddleware(sagaMiddleware))
);
sagaMiddleware.run(rootSaga);
const persistor = persistStore(store);

export { store, persistor };