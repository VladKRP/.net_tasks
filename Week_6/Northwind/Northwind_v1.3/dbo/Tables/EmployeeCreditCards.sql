CREATE TABLE [dbo].[EmployeeCreditCards] (
    [CardNumber] NVARCHAR (50) NOT NULL,
    [CardHolder] NVARCHAR (50) NULL,
    [ExpireDate] DATETIME      NOT NULL,
    [EmployeeID] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([CardNumber] ASC),
    CONSTRAINT [FK_EmployeeCard_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

