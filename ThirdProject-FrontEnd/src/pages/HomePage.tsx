import { Button, Modal, TextField, Typography } from '@mui/material'
import { useAuthenticationStore } from '../store/authentication-store'
import styles from '../styles/HomePage.module.scss'
import Text from '../components/Text';
import { useEffect, useState } from 'react';
import { UserAPI } from '../api/user-api';
import { toast } from 'react-toastify';
import { DatePicker } from '@mui/x-date-pickers';
function HomePage() {
  const { logout, user, updateUser } = useAuthenticationStore();
  const [newBirthDay, setNewBirthday] = useState<Date | undefined | null>(null);
  const [newPassword, setNewPassword] = useState('');
  const [openChangeBirthDayModal, setOpenChangeBirthDayModal] = useState(false);
  const [openChangePasswordModal, setOpenChangePasswordModal] = useState(false);

  useEffect(() => {
    if (user?.birth_day) {
      setNewBirthday(new Date(user.birth_day));
    }
    else {
      setNewBirthday(null);
    }
  }, [user?.birth_day])

  async function handleSubmitChangePassword(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    try {
      await UserAPI.changePassword({
        user_name: user!.user_name,
        new_password: newPassword
      });
      toast.success('Password changed successfully');
      setOpenChangePasswordModal(false);
      setNewPassword('');
    } catch (err) {
      console.error(err)
    }
  }

  async function handleSubmitChangeBirthday(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    if (!newBirthDay) return;
    try {
      await UserAPI.changeBirthDay({
        user_name: user!.user_name,
        new_birth_day: newBirthDay
      });
      user!.birth_day = newBirthDay;
      updateUser(user!);
      toast.success('Birthday changed successfully');
      setOpenChangeBirthDayModal(false);

    } catch (err) {
      console.error(err);
    }
  }

  function handleNewBirthDayChange(e: Date | null) {
    if (e) {
      setNewBirthday(e);
    }
  }

  function handleCloseChangePasswordModal() {
    setOpenChangePasswordModal(false);
    setNewPassword('');
  }
  
  function handleNewPasswordChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>){
    setNewPassword(e.target.value);
  }

  return (
    <div className={styles['back-drop']}>
      <div className={styles.container}>
        <Typography variant='h3'>Current user info</Typography>

        <Text label='Username'>{user?.user_name}</Text>
        <Text label='Create time'>{user?.create_time.toLocaleString('en-GB')}</Text>
        <Text label='Birthday'>{user?.birth_day.toLocaleDateString('en-GB')}</Text>
        <Text label='Last login'>{user?.last_login.toLocaleString('en-GB')}</Text>
        <div className={styles['actions-container']}>
          <Button variant='contained' onClick={() => setOpenChangeBirthDayModal(true)}>Change birthday</Button>
          <Button variant='contained' onClick={() => setOpenChangePasswordModal(true)}>Change Password</Button>
        </div>
        <Button onClick={logout} variant='outlined' color='error'>Log out</Button>
      </div>

      <Modal open={openChangeBirthDayModal} onClose={() => setOpenChangeBirthDayModal(false)} style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
        <form className={styles['modal-container']} onSubmit={handleSubmitChangeBirthday}>
          <Typography variant='h4'>Change Birthday</Typography>
          <DatePicker sx={{ width: "100%" }}
            label='Date of birth*' format='dd/MM/yyyy' value={newBirthDay ? new Date(newBirthDay) : null} onChange={handleNewBirthDayChange} />
          <div className={styles['actions-container']}>
            <Button onClick={() => setOpenChangeBirthDayModal(false)}>Cancel</Button>
            <Button variant='outlined' color='error' type='submit'>Change birthday</Button>
          </div>
        </form>
      </Modal>

      <Modal open={openChangePasswordModal} onClose={handleCloseChangePasswordModal} style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
        <form className={styles['modal-container']} onSubmit={handleSubmitChangePassword}>
          <Typography variant='h4'>Change password</Typography>
          <TextField required label='New password' placeholder='Enter new password' value={newPassword} onChange={handleNewPasswordChange} type='password' />
          <div className={styles['actions-container']}>
            <Button onClick={handleCloseChangePasswordModal}>Cancel</Button>
            <Button variant='outlined' color='error' type='submit'>Change password</Button>
          </div>
        </form>
      </Modal>
    </div>
  )
}

export default HomePage