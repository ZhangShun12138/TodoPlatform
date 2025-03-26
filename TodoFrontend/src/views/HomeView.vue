<template>
  <div class="home-container">
    <!-- 头部 -->
    <div class="header">
      <h1>智能待办平台</h1>
      <el-button type="danger" @click="handleLogout">退出登录</el-button>
    </div>

    <!-- 添加待办事项 -->
    <div class="add-todo">
      <el-input
        v-model="newTodo"
        placeholder="请输入新的待办事项"
        @keyup.enter="addTodo"
      >
        <template #append>
          <el-button type="primary" @click="addTodo">添加</el-button>
        </template>
      </el-input>
    </div>

    <!-- 待办事项列表 -->
    <div class="todo-list">
      <el-card v-for="(todo, index) in todos" :key="index" class="todo-item">
        <div class="todo-content">
          <el-checkbox v-model="todo.isCompleted" @change="updateTodo(todo)">
            <span :class="{ 'completed': todo.isCompleted }">{{ todo.title }}</span>
          </el-checkbox>
          <el-button type="danger" icon="el-icon-delete" @click="deleteTodo(todo.id)">删除</el-button>
        </div>
        <div class="todo-meta">
          <span>创建时间：{{ formatDate(todo.createdAt) }}</span>
          <br>
          <span>完成事件：{{ todo.dueDate ? formatDate(todo.dueDate) : '' }}</span>
        </div>
      </el-card>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import { getTodos, addTodoItem, updateTodoItem, deleteTodoItem } from '../api/todoApi';
import { id } from 'element-plus/es/locales.mjs';

const router = useRouter();
const newTodo = ref('');
const todos = ref([]);

// 获取待办事项
const fetchTodos = async () => {
  try {
    const res = await getTodos();
    todos.value = res.data;
  } catch (error) {
    ElMessage.error('获取待办事项失败');
  }
};

// 添加待办事项
const addTodo = async () => {
  if (!newTodo.value.trim()) {
    ElMessage.warning('请输入待办事项内容');
    return;
  }

  try {
    const username = localStorage.getItem('username');
    const taskData = {
      title: newTodo.value,
      description: '', // 添加默认值
      isCompleted: false, // 添加默认值
      // createdAt: new Date().toLocaleString(), // 添加默认值
      priority: 1, // 添加默认值
      userId: username
    };
    await addTodoItem(taskData);
    newTodo.value = '';
    await fetchTodos();
    ElMessage.success('添加成功');
  } catch (error) {
    ElMessage.error('添加失败');
  }
};

// 更新待办事项状态
const updateTodo = async (todo) => {
  try {
    const username = localStorage.getItem('username');
    await updateTodoItem(todo.id, {
      id: todo.id,
      title: todo.title,
      description: todo.description, // 添加默认值
      isCompleted: todo.isCompleted, // 添加默认值
      createdAt: todo.createdAt, // 添加默认值
      priority: todo.priority, // 添加默认值
      userId: username
    });
    await fetchTodos();
    ElMessage.success('更新成功');
  } catch (error) {
    ElMessage.error('更新失败');
  }
};

// 删除待办事项
const deleteTodo = async (id) => {
  try {
    await deleteTodoItem(id);
    await fetchTodos();
    ElMessage.success('删除成功');
  } catch (error) {
    ElMessage.error('删除失败');
  }
};

// 格式化日期
const formatDate = (dateString) => {
  return new Date(dateString).toLocaleString();
};

// 退出登录
const handleLogout = () => {
  localStorage.removeItem('token');
  router.push('/login');
};

// 组件挂载时获取待办事项
onMounted(() => {
  fetchTodos();
});
</script>

<style scoped>
.home-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.add-todo {
  margin-bottom: 20px;
}

.todo-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.todo-item {
  transition: all 0.3s ease;
}

.todo-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.todo-meta {
  margin-top: 10px;
  font-size: 0.8em;
  color: #666;
}

.completed {
  text-decoration: line-through;
  color: #999;
}
</style>
