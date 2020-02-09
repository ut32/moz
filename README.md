

`Moz`，是一个基于`ASP.NET CORE 3.1` + `SqlSugar` + `LayUI`的极速网站开发框架。只需dll引用，就拥有了一个网站需要的诸多基础功能，再配合代码生成器，就可以很快完成网站框架的搭建。框架默认实现了分类管理、会员管理、广告管理、文章管理、权限角色管理、定时任务管理等。

![](https://static.ut32.com/upload/01d25ecd4695486fa273480f1b259808.jpg)

### Moz功能
 
 - [ ] 日志
 - [x] JWT认证
 - [x] 分布式缓存
 - [x] 事件发布订阅
 - [ ] 多语言
 - [x] [定时任务](https://ut32.com/post/moz_quartz)
 - [x] 多数据库
 - [x] 后台菜单
 - [x] 分类管理
 - [x] 万能文章发布管理
 - [x] 角色管理
 - [x] 权限管理
 - [x] 用户管理
 - [x] 广告管理
 - [ ] 插件
 - [x] [自定义后台路径](https://ut32.com/post/moz_custom_admin_path)
 - [x] 自定义欢迎页

 
### 如何使用

- 新建.NetCore Web项目

- Nuget安装

  使用 nuget 安装核心包
  ```
  Install-Package Moz
  ```

  使用 nuget 安装后台
  ```
  Install-Package Moz.Admin.Layui
  ```

- 编辑项目文件

  ```
  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <PreserveCompilationReferences>true</PreserveCompilationReferences>
    <PreserveCompilationContext>true</PreserveCompilationContext>
  </PropertyGroup>
  ```

- 修改Startup文件

  ```
  public void ConfigureServices(IServiceCollection services)
  {
        services.AddMoz(options =>
        { 
            options.EncryptKey = "jEeESr7VySYru5c2";
            options.Admin.Path = "myadmin";
            options.Db.Add(new DbOptions
            {
                MasterConnectionString = Configuration["ConnectionString"]
            });
        });
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMoz(env);
   }
  ```
### Demo

可参考Demo，https://github.com/ut32/moz/tree/master/samples/WebApp

### 数据库文件

数据库文件位于 : https://github.com/ut32/moz/tree/master/db/

超级管理员 
用户名：admin  密码：620389!

### 文档
官方文档，暂时不齐，后边慢慢补充

https://ut32.com/category/opensource/moz


