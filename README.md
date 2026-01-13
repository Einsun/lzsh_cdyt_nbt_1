NBTEdu
介绍

AeDataService 波形数据处理服务
NBWebApp 后台服务

软件架构

基于 .NET 7 开发，可升级为 .NET 8

安装教程

安装 .NET 7 SDK
https://dotnet.microsoft.com/zh-cn/download/dotnet/7.0

安装 .NET 8 SDK
https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0

安装 VS Code（可选）

使用说明

dotnet restore
恢复项目的依赖项和工具

dotnet run
运行源代码

dotnet publish
将应用程序及其依赖项发布到文件夹以部署到托管系统
https://learn.microsoft.com/zh-cn/dotnet/core/tools/dotnet-publish

数据库

安装 dotnet-ef
dotnet tool update --global dotnet-ef

创建第一个迁移
dotnet ef migrations add InitalClass

创建数据库并从迁移中创建架构
dotnet ef database update