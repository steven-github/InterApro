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
  authData?: string;
}

export interface Response {
  success: number,
  message: string
}
