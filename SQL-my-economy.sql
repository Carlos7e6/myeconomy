DROP TABLE FixedCosts
DROP TABLE Transactions
DROP TABLE BudgetCategories

CREATE TABLE BudgetCategories(
ID int IDENTITY(1,1) PRIMARY KEY,
Name varchar(50) NOT NULL UNIQUE,
Percentage numeric(5,2) NOT NULL,
Active bit DEFAULT 1
);

CREATE TABLE Transactions (
    ID int IDENTITY(1,1) PRIMARY KEY,
    Amount numeric(18, 2) NOT NULL,         
    TransactionDate date NOT NULL,         
    Description varchar(255),              
    CategoryID int NULL,                 
    IsIncome bit NOT NULL DEFAULT 0,        
    CreatedAt datetime DEFAULT GETDATE(),    
    
    FOREIGN KEY (CategoryID) REFERENCES BudgetCategories(ID)
);

CREATE TABLE FixedCosts(
    ID int IDENTITY(1,1) PRIMARY KEY,
    Name varchar(50) NOT NULL,  
    Description varchar(255) NULL,
    Amount numeric(18, 2) NOT NULL, 
    Frequency int NOT NULL, -- DAYS
    CategoryID int,
    AproxDatePayment datetime NOT NULL,
    CreatedAt datetime DEFAULT GETDATE(),    
    FOREIGN KEY (CategoryID) REFERENCES BudgetCategories(ID)
);


