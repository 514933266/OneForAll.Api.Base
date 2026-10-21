## OneForAll-基础管理服务

> ### ⚠️ 生产 IIS 部署特殊说明（PUT/DELETE 405 与 500.19）
> - **现象**：发布到生产 IIS 后 `DELETE`/`PUT` 返回 `405`（`Allow` 仅 `GET,HEAD,OPTIONS,TRACE`），本机正常。根因：服务器装了 IIS `WebDAVModule`，在请求进入 ASP.NET Core 前拦截 PUT/DELETE。
> - **修复（已入库）**：`Base.Host/web.config`（源码级模板，`dotnet publish` 以它为基合并 `aspNetCore` 节点并保留以下节点）已在 `system.webServer` 下加 `<handlers><remove name="WebDAV" /></handlers>` 与 `<modules><remove name="WebDAVModule" /></modules>`，随每次发布自动生效。
> - **新服务器踩坑**：若服务器把 `system.webServer/modules` 节父级锁定，应用级 `<modules>` 块会触发 `HTTP 500.19`（`0x80070021` 不能在此路径中使用此配置节）。解法（每台新服务器一次性执行、之后长期有效）：管理员运行 `C:\Windows\System32\inetsrv\appcmd.exe unlock config /section:system.webServer/modules`，或 IIS「功能委派」把 Modules 设为可读/可写。应急：临时删除站点 web.config 的 `<modules>` 块即可恢复上线。
> - 详见仓库根 `README.md`。
帮助开发人员快速构建Web后台管理系统常见基础管理服务<br/>
主项目传送门：https://github.com/514933266/OneForAll.Web

### 一、基础管理
#### 1.1 用户管理
  + 用户账号列表
  + 支持重置密码、禁用操作
#### 1.2 角色管理
  + 角色列表
  + 分配角色权限、角色分配用户操作（用于控制菜单以及操作权限）

### 二、图文管理
#### 2.1 系统通知
  + 发送系统文章消息（仅能通知当前租户）

