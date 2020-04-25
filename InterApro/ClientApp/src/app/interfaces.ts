// export interface User {
//   id: number;
//   email: string;
//   password: string;
// }

export class User {
  id: number;
  username: string;
  password: string;
  firstName: string;
  lastName: string;
  bossId: number;
  rol: number;
  authData?: string;
  isAdmin?: boolean;
  userId: number;
  email: string;
}

export class Requests {
  id: number;
  UserId: number;
  FirstName: string;
  lastName: string;
  email: string;
  username: string;
  assigneeId: number;
  description: number;
  price: number;
  orderstatus: number;
  assigneeName: string;
}

export interface Response {
  success: number,
  message: string
}
