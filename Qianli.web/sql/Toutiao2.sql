/*
F:\HSZ\QIANLIWEBTK\QIANLI.WEB\QIANLI.WEB\APP_DATA\QIANLIDATA.MDF 的部署脚本

此代码由工具生成。
如果重新生成此代码，则对此文件的更改可能导致
不正确的行为并将丢失。
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "F:\HSZ\QIANLIWEBTK\QIANLI.WEB\QIANLI.WEB\APP_DATA\QIANLIDATA.MDF"
:setvar DefaultFilePrefix "F_\HSZ\QIANLIWEBTK\QIANLI.WEB\QIANLI.WEB\APP_DATA\QIANLIDATA.MDF_"
:setvar DefaultDataPath "C:\Users\lenovo\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\v11.0\"
:setvar DefaultLogPath "C:\Users\lenovo\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\v11.0\"

GO
:on error exit
GO
/*
请检测 SQLCMD 模式，如果不支持 SQLCMD 模式，请禁用脚本执行。
要在启用 SQLCMD 模式后重新启用脚本，请执行:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"



ALTER DATABASE [$(DatabaseName)] SET MULTI_USER
GO
USE master;
ALTER DATABASE [$(DatabaseName)] COLLATE Chinese_PRC_CI_AS
