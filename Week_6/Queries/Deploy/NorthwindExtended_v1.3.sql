USE Northwind;
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME = 'Region' AND xtype ='U')
	EXEC SP_RENAME 'Northwind.Region', 'Regions'
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME = 'Customers' AND xtype='U')
	ALTER TABLE [Northwind].[Customers] 
		ADD FoundationDate DATE NULL
GO