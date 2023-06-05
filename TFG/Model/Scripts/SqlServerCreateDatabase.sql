/* 
 * SQL Server Script
 * 
 * This script can be executed from MS Sql Server Management Studio Express,
 * but also it is possible to use a command Line syntax:
 *
 *    > sqlcmd.exe -U [user] -P [password] -I -i SqlServerCreateTables.sql
 *
 */

 /******************************************************************************/
 /*** PATH to store the db files. This path must exists in the local system. ***/
 /******************************************************************************/
 DECLARE @Default_DB_Path as VARCHAR(64)  
 SET @Default_DB_Path = N'C:\software\Mad\DataBase\'
 

USE [master]

/****** Drop database if already exists  ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = 'tfg')
DROP DATABASE [tfg]



/* DataBase Creation */
                                  
DECLARE @sql nvarchar(500)

SET @sql = 
  N'CREATE DATABASE [tfg] 
    ON PRIMARY ( NAME = tfg, FILENAME = "' + @Default_DB_Path + N'tfg.mdf")
    LOG ON ( NAME = tfg_log, FILENAME = "' + @Default_DB_Path + N'tfg_log.ldf")'

EXEC(@sql)
PRINT N'Database created.'
PRINT N'Done'
GO
