
USE Northwind;

GO

IF(EXISTS(SELECT * FROM SYSOBJECTS WHERE NAME='EmployeesCreditCards' AND xtype='U'))
	DROP TABLE Northwind.EmployeesCreditCards
GO

CREATE TABLE Northwind.EmployeesCreditCards (
	CardNumber INT IDENTITY(1,1) NOT NULL,
	CardExpireDate DATE NOT NULL,
	CardHolder NVARCHAR(30),
	EmployeeID INT,
	CONSTRAINT PK_CardNumber PRIMARY KEY (
		CardNumber
	),
	CONSTRAINT FK_EmployeeID FOREIGN KEY(
		EmployeeID
	) REFERENCES Northwind.Employees (
		EmployeeID
	)
)

GO

CREATE INDEX CardNumber ON Northwind.EmployeesCreditCards(CardNumber)

GO