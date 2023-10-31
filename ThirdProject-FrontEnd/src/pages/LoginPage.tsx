import { Button, TextField, Typography } from '@mui/material'
import styles from '../styles/LoginPage.module.scss';
import { Link } from 'react-router-dom';
import { useState } from 'react';
import { UserLoginRequest } from '../type/types';
import { AuthenticationAPI } from '../api/authentication-api';
import { useAuthenticationStore } from '../store/authentication-store';
import { toast } from 'react-toastify';
function LoginPage() {
    const { login } = useAuthenticationStore();
    const [userLogin, setUserLogin] = useState<UserLoginRequest>({
        user_name: '',
        password: '',
    });
    function handleUserNameChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        setUserLogin(prev => ({ ...prev, user_name: e.target.value }))
    }

    function handlePasswordChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        setUserLogin(prev => ({ ...prev, password: e.target.value }))
    }

    async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        try {
            const authResponse = await AuthenticationAPI.logIn(userLogin);
            login(authResponse.data);
            console.log(authResponse);
            toast.success('Welcome back');
        } catch (err) {
            toast.error('Invalid username or password!');
            console.error('error logging in: ', err);
        }
    }

    return (
        <div className={styles['back-drop']}>
            <form className={styles['content-container']} onSubmit={handleSubmit}>
                <Typography variant='h2'>Login</Typography>
                <TextField required label="User name" variant="outlined" placeholder='enter your user name...' fullWidth onChange={handleUserNameChange} value={userLogin.user_name} />
                <TextField required label="Password" variant="outlined" placeholder='enter your password...' type='password' fullWidth onChange={handlePasswordChange} value={userLogin.password} />
                <Typography alignSelf={'flex-end'}>Don't have an account? <Link to='/register'>Sign up</Link></Typography>
                <Button variant='contained' fullWidth type='submit'>Log in</Button>
            </form>
        </div>

    )
}

export default LoginPage