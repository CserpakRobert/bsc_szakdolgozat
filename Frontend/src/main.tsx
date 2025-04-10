import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { AuthProvider } from './globalHelpers/AuthContext.tsx'
import { ErrorProvider } from './globalHelpers/ErrorContext.tsx'
import { ThemeProvider } from '@emotion/react'
import { CssBaseline } from '@mui/material'
import { darkTheme } from './globalHelpers/theme.tsx'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <AuthProvider>
        <ErrorProvider>
          <ThemeProvider theme={darkTheme}>
            <CssBaseline>

              <App />
            </CssBaseline>
          </ThemeProvider>
        </ErrorProvider>
      </AuthProvider>
    </LocalizationProvider>
  </StrictMode>,
)
