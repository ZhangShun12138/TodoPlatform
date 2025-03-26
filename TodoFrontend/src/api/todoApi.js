import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7129', // 添加/api前缀
  headers: {
    'Authorization': `Bearer ${localStorage.getItem('token')}`
  }
});

export const getTodos = () => api.get('/Task/get-todos'); // 移除apiBaseUrl
export const addTodoItem = (data) => api.post('/Task/create-task', data); // 添加正确路径
export const updateTodoItem = (id, data) => api.put(`/Task/${id}`, data); // 添加正确路径
export const deleteTodoItem = (id) => api.delete(`/Task/${id}`); // 添加正确路径