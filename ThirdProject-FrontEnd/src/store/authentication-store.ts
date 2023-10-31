import { create } from 'zustand';
import { AuthResponse, User } from '../type/types';

type AuthenticationStore = {
    user?: User | null,
    jwt?: string | null
    login: (authResponse: AuthResponse) => void,
    logout: () => void,
    updateUser: (user: User) => void,
    updateJwt: (jwt: string | null) => void,
}

export const LocalStorage_RefreshToken = 'refreshToken';
export const useAuthenticationStore = create<AuthenticationStore>((set) => ({
    user: null,
    login: (authResponse: AuthResponse) => set(() => {
        localStorage.setItem(LocalStorage_RefreshToken, authResponse.refresh_token);
        return { user: authResponse.user, jwt: authResponse.jwt }
    }),
    logout: () => set(() => {
        localStorage.removeItem(LocalStorage_RefreshToken);
        return { user: null, jwt: null }
    }
    ),
    updateUser: (user: User) => set(() => ({ user })),
    updateJwt: (jwt: string | null) => set(() => ({ jwt })),
}
))