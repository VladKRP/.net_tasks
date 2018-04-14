
USE NorthwindExtended;

GO

IF(EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME='EmployeesCreditCards' AND xtype='U'))
	DROP TABLE [dbo].EmployeesCreditCards
GO

CREATE TABLE [dbo].[EmployeesCreditCards] (
	CardNumber INT IDENTITY(1,1) NOT NULL,
	CardExpireDate DATE NOT NULL,
	CardHolder NVARCHAR(30),
	EmployeeID INT,
	CONSTRAINT PK_CardNumber PRIMARY KEY (
		CardNumber
	),
	CONSTRAINT FK_EmployeeID FOREIGN KEY(
		EmployeeID
	) REFERENCES dbo.Employees (
		EmployeeID
	)
)

GO

CREATE INDEX CardNumber ON [dbo].EmployeesCreditCards(CardNumber)

GO