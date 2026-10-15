-- Apply after CREATE DATABASE [benchmark] in the SQL Server adapter.
ALTER DATABASE [benchmark] SET RECOVERY SIMPLE;
ALTER DATABASE [benchmark] SET DELAYED_DURABILITY = DISABLED;
