import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7129', // 添加/api前缀
//   headers: {
//     'Authorization': `Bearer ${localStorage.getItem('token')}`
//   }
});

export const sendVerificationCode = (data) => api.post('/User/send-code', data); // 移除apiBaseUrl
export const userRegister = (data) => api.post('/User/user-register', data); // 添加正确路径
export const userlogin = (data) => api.post(`/User/login`, data); // 添加正确路径