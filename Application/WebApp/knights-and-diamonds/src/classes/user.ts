export class User
{
    constructor(
        id: number,
        token: string,
        email: string,
        password: string,
        name: string,
        connId: string
    ){}
}

export interface OnlineUsers
{
    id: number,
    name: string,
    surName: string,
    userName: string,
}