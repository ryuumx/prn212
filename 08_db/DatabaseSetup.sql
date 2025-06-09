-- Execute this SQL script in SQL Server Management Studio or VS Code

-- Create database for Database First demo
CREATE DATABASE MyStore;
GO

USE MyStore;
GO

-- Create Categories table
CREATE TABLE Categories (
    CategoryID int IDENTITY(1,1) PRIMARY KEY,
    CategoryName nvarchar(50) NOT NULL,
    Description nvarchar(500) NULL
);

-- Create Products table
CREATE TABLE Products (
    ProductID int IDENTITY(1,1) PRIMARY KEY,
    ProductName nvarchar(100) NOT NULL,
    CategoryID int NOT NULL,
    UnitPrice decimal(18,2) NOT NULL DEFAULT 0,
    UnitsInStock smallint NOT NULL DEFAULT 0,
    ReorderLevel smallint NOT NULL DEFAULT 0,
    Discontinued bit NOT NULL DEFAULT 0,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Insert sample data
INSERT INTO Categories (CategoryName, Description) VALUES
('Electronics', 'Electronic devices and accessories'),
('Books', 'Books and publications'),
('Clothing', 'Apparel and fashion items'),
('Home & Garden', 'Home improvement and garden supplies');

INSERT INTO Products (ProductName, CategoryID, UnitPrice, UnitsInStock, ReorderLevel) VALUES
('Laptop Computer', 1, 1299.99, 25, 5),
('Wireless Mouse', 1, 29.99, 100, 20),
('Programming Guide', 2, 49.99, 50, 10),
('Fiction Novel', 2, 14.99, 75, 15),
('Business Shirt', 3, 79.99, 30, 10),
('Garden Tools Set', 4, 89.99, 15, 5);

-- Verify data
SELECT * FROM Categories;
SELECT * FROM Products;