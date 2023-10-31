import { Button, TextField, Typography } from '@mui/material';
import styles from '../styles/RegisterPage.module.scss';
import { Link } from 'react-router-dom';
import { useState } from 'react';
import { UserRegisterRequest } from '../type/types';
import { DatePicker } from '@mui/x-date-pickers';
import { toast } from 'react-toastify';
import { AuthenticationAPI } from '../api/authentication-api';
import { useAuthenticationStore } from '../store/authentication-store';

export function RegisterPage() {
    const { login } = useAuthenticationStore();
    const [userSignUp, setUserSignUp] = useState<UserRegisterRequest>({
        user_name: '',
        password: '',
    });
    const [confirmPassword, setConfirmPassword] = useState('');

    function handleBirthDayChange(e: Date | null) {
        if (e) {
            setUserSignUp(prev => ({ ...prev, birth_day: e }))
        }
    }

    function handleUserNameChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        setUserSignUp(prev => ({ ...prev, user_name: e.target.value }));
    }

    function handlePasswordChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        setUserSignUp(prev => ({ ...prev, password: e.target.value }));
    }

    function handleConfirmPasswordChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        setConfirmPassword(e.target.value);
    }

    function validate() {
        if (confirmPassword !== userSignUp.password) {
            toast.error('Confirm password not match');
            return false;
        }
        if (!userSignUp.birth_day) {
            toast.error('Please enter your birthday');
            return false;
        }
        return true;
    }

    async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        if (!validate()) return;
        try {
            const authResponse = await AuthenticationAPI.signUp(userSignUp);
            login(authResponse.data);
            toast.success('Signed up successfully!');
        } catch (err) {
            toast.error(err as String);
            console.error(err);
        }
    }

    return (
        <div className={styles['back-drop']}>
            <form className={styles['content-container']} onSubmit={handleSubmit}>
                <Typography variant='h2'>Sign up</Typography>
                <TextField required label="Username" variant="outlined" placeholder='enter your user name...' fullWidth onChange={handleUserNameChange} value={userSignUp.user_name} />
                <TextField required label="Password" variant="outlined" placeholder='enter your password...' type='password' fullWidth onChange={handlePasswordChange} value={userSignUp.password} />
                <TextField required label="Confirm password" variant="outlined" placeholder='enter your password...' type='password' fullWidth onChange={handleConfirmPasswordChange} value={confirmPassword} />
                <DatePicker sx={{ width: "100%" }}
                    label='Date of birth*' format='dd/MM/yyyy' value={userSignUp.birth_day ? new Date(userSignUp.birth_day) : null} onChange={handleBirthDayChange} />
                <Button variant='contained' fullWidth type='submit'>Sign Up</Button>
                <Typography alignSelf={'flex-start'}>Already have an account? <Link to='/login'>Log in!</Link></Typography>
            </form>
        </div>
    );
}
