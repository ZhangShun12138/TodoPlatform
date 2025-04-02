<template>
  <div class="login-container">
    <el-card class="login-card">
      <h2 class="login-title">用户登录</h2>
      <el-form :model="form" :rules="rules" @submit.prevent="handleLogin">
        <!-- 用户名输入 -->
        <el-form-item prop="username">
          <el-input
            v-model="form.username"
            placeholder="请输入账号"
            :prefix-icon="User"
          />
        </el-form-item>

        <!-- 密码输入 -->
        <el-form-item prop="password">
          <el-input
            v-model="form.password"
            type="password"
            placeholder="请输入密码"
            :prefix-icon="Lock"
          />
        </el-form-item>

        <!-- 登录按钮 -->
        <el-form-item>
          <el-button 
            type="primary" 
            native-type="submit" 
            class="login-btn"
            :loading="loading"
          >
            登录
          </el-button>
        </el-form-item>

        <!-- 注册链接 -->
        <div class="register-link">
          <router-link to="/register">没有账号？立即注册</router-link>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import { User, Lock } from '@element-plus/icons-vue';
import { userlogin } from '../api/userApi';

// 表单数据
const form = reactive({
  username: '',
  password: ''
});

// 表单验证规则
const rules = {
  username: [
    { required: true, message: '邮箱不能为空', trigger: 'blur' },
    { type: 'email', message: '邮箱格式不正确', trigger: ['blur', 'change'] }
  ],
  password: [
    { required: true, message: '密码不能为空' },
    { min: 8, max: 20, message: '长度在8-20个字符' },
    { pattern: /^(?=.*[a-zA-Z])(?=.*\d).+$/, 
      message: '需包含字母和数字组合' }
  ]
};

// 登录逻辑
const router = useRouter();
const loading = ref(false); // 添加 loading 变量

const handleLogin = async () => {
  loading.value = true;
  try {
    const inputdata = {
      Email: form.username,
      Password: form.password,
    };
    const res = await userlogin(inputdata);
    const data = res.data;
    if (data.success) {
      localStorage.setItem('token', data.token); // 存储 Token
      localStorage.setItem('username', form.username);
      ElMessage.success('登录成功');
      router.push('/home');
    }
  } catch (error) {
    ElMessage.error('登录失败，请检查账号密码');
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  width: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
}

.login-card {
  width: 400px;
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
}

.login-title {
  text-align: center;
  margin-bottom: 30px;
  color: #333;
}

.login-btn {
  width: 100%;
  margin-top: 10px;
}

.register-link {
  text-align: center;
  margin-top: 15px;
}
</style>