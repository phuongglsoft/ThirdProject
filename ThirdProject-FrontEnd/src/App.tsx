import { Navigate, Route, Routes } from 'react-router-dom';
import { useAuthenticationStore } from './store/authentication-store'
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import './styles/Reset.scss';
import { RegisterPage } from './pages/RegisterPage';

function App() {
  const { user } = useAuthenticationStore();
  return (
    <>
      <Routes>
        <Route index element={<Navigate to='/login' />} />
        <Route path='/login' element={user ? <Navigate to='/home' /> : <LoginPage />} />
        <Route path='/register' element={user ? <Navigate to='/home' /> : < RegisterPage />} />
        <Route path='/home' element={user ? <HomePage /> : <Navigate to='/login' />} />
      </Routes>
    </>
  )
}

export default App
