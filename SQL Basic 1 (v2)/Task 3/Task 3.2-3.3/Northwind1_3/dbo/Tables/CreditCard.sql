CREATE TABLE [dbo].[CreditCard] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [CardNumber]     INT           NOT NULL,
    [ExpirationDate] DATE          NOT NULL,
    [CardHolder]     NVARCHAR (50) NOT NULL,
    [EmployeeId]     INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CreditCard_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [PK_CreditCard] UNIQUE NONCLUSTERED ([CardNumber] ASC)
);

