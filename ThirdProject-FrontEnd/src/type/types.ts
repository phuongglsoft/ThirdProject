export type User = {
    user_name: string,
    birth_day: Date,
    create_time: Date,
    last_login: Date,
}

export type UserLoginRequest = {
    user_name: string,
    password: string,
}

export type UserRegisterRequest = {
    user_name: string,
    password: string,
    birth_day?: Date | null,
}

export type ChangePasswordRequest = {
    user_name: string,
    new_password: string,
}

export type ChangeBirthDayRequest = {
    user_name: string,
    new_birth_day: Date,
}
export type AuthResponse = {
    user: User,
    jwt: string,
    refresh_token: string,
}

export type RefreshTokenRequest={
    user_name: string,
    refresh_token: string,
}