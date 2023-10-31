import { toast } from 'react-toastify';
import { useAuthenticationStore } from '../store/authentication-store';
import { AuthenticationAPI } from './authentication-api';
import axios from 'axios'
export let BASEURL = 'http://localhost:8081/api';
if (import.meta.env.PROD) {
    BASEURL = 'https://midouz.online:8080';
}
const headers = (jwt?: string) => {
    if (!jwt) {
        return { 'Content-Type': 'application/json', withCredentials: true }
    }
    return {
        Authorization: `Bearer ${jwt}`,
        'Content-Type': 'application/json',
        withCredentials: true,
    }

}

const myAxios = (jwt?: string, abortController?: AbortController) => {
    const mAxios = axios.create({ headers: headers(jwt), baseURL: BASEURL, signal: abortController?.signal })
    mAxios.interceptors.response.use(response => {
        return response
    },
        error => {
            if (error.code === 'ERR_CANCELED') {
                return Promise.resolve({ status: 499 })
            }
            if (error.response.status === 401) {
                const { updateJwt, logout, user } = useAuthenticationStore.getState();
                return AuthenticationAPI.requestRefreshToken().then((tokenResponse) => {
                    const token = tokenResponse.data;
                    updateJwt(token);
                    const originalRequest = error.config;
                    originalRequest.headers.Authorization = `Bearer ${token}`;
                    return axios(originalRequest);
                }).catch(() => {
                    if (user) toast.info("Please log in again!");
                    logout();
                })
            }
            return Promise.reject((error.response && error.response.data) || 'Error')
        }
    );
    return mAxios
};
export default myAxios;
