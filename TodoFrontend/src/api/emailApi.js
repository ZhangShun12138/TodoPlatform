import axios from 'axios';

const apiBaseUrl = 'http://localhost:5227'; // 替换为你的后端 API 地址

export const sendVerificationCode = async (email) => {
        const response = await axios.post(`${apiBaseUrl}/User/send-code`, {
            Email: email
        });
    };