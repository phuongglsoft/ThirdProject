import { useAuthenticationStore } from '../store/authentication-store';
import { ChangeBirthDayRequest, ChangePasswordRequest } from '../type/types';
import myAxios from './config';

export const UserAPI = {
    changePassword: async function (request: ChangePasswordRequest) {
        const jwt = useAuthenticationStore.getState().jwt;
        if (!jwt) {
            throw new Error('User is not logged in');
        }
        return myAxios(jwt).patch(`/user/${request.user_name}/change-password`, JSON.stringify(request));
    },
    changeBirthDay: async function (request: ChangeBirthDayRequest) {
        const jwt = useAuthenticationStore.getState().jwt;
        if (!jwt) {
            throw new Error('User is not logged in');
        }
        return myAxios(jwt).patch(`/user/${request.user_name}/change-birthday`, JSON.stringify(request));
    }
}