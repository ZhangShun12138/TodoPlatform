# 使用官方 .NET 9.0 SDK 镜像作为构建环境
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-stage

# 设置工作目录
WORKDIR /app

# 复制项目文件
COPY . .

# 构建项目
RUN dotnet publish -c Release -o out

# 使用 .NET 9.0 运行时镜像作为生产环境
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS production-stage

# 设置工作目录
WORKDIR /app

# 设置生产环境变量
ENV ASPNETCORE_ENVIRONMENT=Production

# 复制构建好的文件
COPY --from=build-stage /app/out .

# 暴露80端口
EXPOSE 5227

# 添加健康检查
HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:5227/health || exit 1

# 启动后端服务
ENTRYPOINT ["dotnet", "TodoBackend.dll", "--urls", "http://*:5227"]