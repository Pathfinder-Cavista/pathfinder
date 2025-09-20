export interface LoginResponseModel {
  accessToken: string,
  refreshToken: string,
}

export interface CurrentUserResponseModel {
  title: string,
  id: string,
  fullName: string,
  email: string,
  phone: string,
  photoUrl: string,
  lastLogin: string,
}