import axios from 'axios';
import { AuthResponse, RefreshTokenRequest, UserLoginRequest, UserRegisterRequest } from './../type/types';
import myAxios, { BASEURL } from './config';
import { LocalStorage_RefreshToken, useAuthenticationStore } from '../store/authentication-store';
export const AuthenticationAPI = {
    logIn: async function (request: UserLoginRequest) {
        const authResponse = myAxios().post<AuthResponse>('/auth/login', JSON.stringify(request));
        const user = (await authResponse).data.user;
        user.birth_day = new Date(user.birth_day);
        user.create_time = new Date(user.create_time);
        user.last_login = new Date(user.last_login);
        return authResponse;
    },
    signUp: async function (request: UserRegisterRequest) {
        const authResponse = myAxios().post<AuthResponse>('/auth/register', JSON.stringify(request))
        const user = (await authResponse).data.user;
        user.birth_day = new Date(user.birth_day);
        user.create_time = new Date(user.create_time);
        user.last_login = new Date(user.last_login);
        return authResponse;
    },
    requestRefreshToken: async function () {
        const userName = useAuthenticationStore.getState().user?.user_name;
        const refreshToken = localStorage.getItem(LocalStorage_RefreshToken);
        if (!refreshToken || !userName) throw new Error("username or refresh token not found");
        const refreshTokenRequest: RefreshTokenRequest = {
            refresh_token: refreshToken,
            user_name: userName
        }
        return await axios.post<string>(`${BASEURL}/auth/refresh-token`, JSON.stringify(refreshTokenRequest), { headers: { 'Content-Type': 'application/json', withCredentials: true } });
    }
}