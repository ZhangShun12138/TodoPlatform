<template>
    <div class="register-container">
      <el-card class="register-card">
        <h2 class="register-title">用户注册</h2>
        <el-form :model="form" :rules="rules" ref="registerForm" @submit.prevent="handleRegister">
          <!-- 邮箱输入 -->
           <el-form-item prop="email">
            <el-input 
              v-model="form.email" 
              placeholder="请输入邮箱地址">
              <template #prefix>
                <el-icon><User /></el-icon>
              </template>
            </el-input>
          </el-form-item>

          <!-- 验证码模块 -->
          <el-form-item prop="code">
           <div class="code-wrapper">
              <el-input
                v-model="form.code"
                placeholder="6位验证码"
                 maxlength="6">
                <template #prefix>
                  <el-icon><Lock /></el-icon>
                </template>
              </el-input>
              <el-button 
                :disabled="isCounting" 
                @click="sendCode"
                class="code-btn">
                {{ codeBtnText }}
              </el-button>
            </div>
          </el-form-item>
  
          <!-- 密码设置 -->
          <el-form-item prop="password">
            <el-input
              v-model="form.password"
              type="password"
              show-password
              placeholder="8-20位字母+数字组合">
              <template #prefix>
                <el-icon><Lock /></el-icon>
              </template>
            </el-input>
          </el-form-item>

          <!-- 立即注册 -->
          <el-form-item>
            <el-button
            type="primary"
            class="register-btn"
            @click="submitForm">
              立即注册
            </el-button>
          </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>
<script setup>
import { ref, reactive } from 'vue';
import { ElMessage } from 'element-plus';
import { User, Lock } from '@element-plus/icons-vue';
import { sendVerificationCode } from '../api/emailApi';
import { userRegister } from '../api/registerApi';
import router from '@/router';

// 表单数据结构
const form = reactive({
  email: '',
  code: '',
  password: ''
})

// 验证规则（参考网页3、5的密码要求）
const rules = {
  email: [
    { required: true, message: '邮箱不能为空', trigger: 'blur' },
    { type: 'email', message: '邮箱格式不正确', trigger: ['blur', 'change'] }
  ],
  code: [
    { required: true, message: '验证码不能为空', trigger: 'blur' },
    { pattern: /^\d{6}$/, message: '验证码为6位数字' }
  ],
  password: [
    { required: true, message: '密码不能为空' },
    { min: 8, max: 20, message: '长度在8-20个字符' },
    { pattern: /^(?=.*[a-zA-Z])(?=.*\d).+$/, 
      message: '需包含字母和数字组合' }
  ]
}

// 验证码倒计时逻辑
const isCounting = ref(false)
const codeBtnText = ref('获取验证码')
let countdown = 60

const sendCode = async () => {
  if (!form.email) {
    ElMessage.warning('请先填写邮箱')
    return
  }
  
  // 模拟API调用（需替换为真实接口）
  try {
    await sendVerificationCode(form.email)
    startCountdown()
    ElMessage.success('验证码已发送')
  } catch (error) {
    ElMessage.error('发送失败，请稍后重试')
  }
}

const startCountdown = () => {
  isCounting.value = true
  const timer = setInterval(() => {
    countdown--
    codeBtnText.value = `${countdown}秒后重发`
    if (countdown <= 0) {
      clearInterval(timer)
      isCounting.value = false
      codeBtnText.value = '重新发送'
      countdown = 60
    }
  }, 1000)
}

const submitForm = async () => {
  if (!form.email) {
    ElMessage.warning('请先填写邮箱')
    return
  }
  if (!form.code) {
    ElMessage.warning('请先填写验证码')
    return
  }
  
  // 模拟API调用（需替换为真实接口）

  try {
    const data = await userRegister(form.email, form.code, form.password);
    const success = data.success
    const result = data.result
    // 3. 检查字段大小写（Result 而非 result）
    if (success && result == '注册成功') {
      ElMessage.success(result); // 注意 Result 首字母大写
      router.push('/login')
    } else {
      // 处理业务逻辑错误（如验证码错误）
      ElMessage.error(result);
    }
  } catch (error) {
    // 4. 精准错误处理
    if (error.response) {
      // HTTP 状态码非 2xx（如 500）
      const errorMessage = error.response.data?.Error || '服务器内部错误';
      ElMessage.error(`服务器错误: ${errorMessage}`);
    } else if (error.request) {
      // 请求未收到响应（如网络断开）
      ElMessage.error('网络错误，请检查连接');
    } else {
      // 其他配置错误
      ElMessage.error(`请求失败: ${error.message}`);
    }
  }
}

const handleRegister = async () => {

  };

</script>

<style scoped>
  .register-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    width: 100vh;
    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  }
  
  .register-card {
    width: 400px;
    padding: 30px;
    border-radius: 8px;
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
  }
  
  .register-title {
    text-align: center;
    margin-bottom: 30px;
    color: #333;
  }
  
  .register-btn {
    width: 100%;
    margin-top: 10px;
  }
  </style>