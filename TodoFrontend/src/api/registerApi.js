import axios from 'axios';

const apiBaseUrl = 'https://localhost:7129'; // 替换为你的后端 API 地址

export const userRegister = async (email, captcha, password) => {
    try {
      const response = await axios.post(`${apiBaseUrl}/User/user-register`, {
        Email: email,
        Captcha: captcha, // 关键点：参数名与后端一致
        Password: password,
      });
      return response.data; // 直接返回解析后的数据
    } catch (error) {
      // 将错误抛给调用方处理
      throw error;
    }
  };