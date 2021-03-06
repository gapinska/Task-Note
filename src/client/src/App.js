import React from 'react'
import { Provider } from 'react-redux'
import { store, persistor } from './state/store'
import './App.scss'
import { ToastContainer } from 'react-toastify'
import { PersistGate } from 'redux-persist/integration/react'
import { createMuiTheme, ThemeProvider } from '@material-ui/core/styles'
import { blue, pink } from '@material-ui/core/colors'
import CssBaseline from '@material-ui/core/CssBaseline'
import Main from './Components/Main'
import Navbar from './Components/Navbar'

const defaultTheme = createMuiTheme({
	palette: {
		primary: {
			main: blue[800]
		},
		secondary: {
			main: pink[700]
		}
	}
})

function App() {
	return (
		<Provider store={store}>
			<PersistGate persistor={persistor}>
				<ThemeProvider theme={defaultTheme}>
					<Navbar />
					<Main />
					<ToastContainer autoClose={5000} position="bottom-right" newestOnTop pauseOnFocusLoss={false} />
					<CssBaseline />
				</ThemeProvider>
			</PersistGate>
		</Provider>
	)
}

export default App
