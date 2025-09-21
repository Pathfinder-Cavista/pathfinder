export interface Alert {
  id: number,
  type: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light',
  title: string,
  message: string,
}