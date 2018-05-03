USE NorthwindExtended;
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME = 'Region' AND xtype ='U')
	EXEC SP_RENAME 'dbo.Region', 'Regions'
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME = 'Customers' AND xtype='U')
	ALTER TABLE [dbo].[Customers] 
		ADD FoundationDate DATE NULL
GO