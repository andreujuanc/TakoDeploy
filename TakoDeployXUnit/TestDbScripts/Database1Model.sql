SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerDemographics](
	[CustomerTypeID] [nchar](10) NOT NULL,
	[CustomerDesc] [ntext] NULL,
 CONSTRAINT [PK_CustomerDemographics] PRIMARY KEY NONCLUSTERED 
(
	[CustomerTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[RegionID] [int] NOT NULL,
	[RegionDescription] [nchar](50) NOT NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY NONCLUSTERED 
(
	[RegionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[FirstName] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](30) NULL,
	[TitleOfCourtesy] [nvarchar](25) NULL,
	[BirthDate] [datetime] NULL,
	[HireDate] [datetime] NULL,
	[Address] [nvarchar](60) NULL,
	[City] [nvarchar](15) NULL,
	[Region] [nvarchar](15) NULL,
	[PostalCode] [nvarchar](10) NULL,
	[Country] [nvarchar](15) NULL,
	[HomePhone] [nvarchar](24) NULL,
	[Extension] [nvarchar](4) NULL,
	[Photo] [image] NULL,
	[Notes] [ntext] NULL,
	[ReportsTo] [int] NULL,
	[PhotoPath] [nvarchar](255) NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [LastName] ON [dbo].[Employees] 
(
	[LastName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PostalCode] ON [dbo].[Employees] 
(
	[PostalCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](15) NOT NULL,
	[Description] [ntext] NULL,
	[Picture] [image] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CategoryName] ON [dbo].[Categories] 
(
	[CategoryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [nchar](5) NOT NULL,
	[CompanyName] [nvarchar](40) NOT NULL,
	[ContactName] [nvarchar](30) NULL,
	[ContactTitle] [nvarchar](30) NULL,
	[Address] [nvarchar](60) NULL,
	[City] [nvarchar](15) NULL,
	[Region] [nvarchar](15) NULL,
	[PostalCode] [nvarchar](10) NULL,
	[Country] [nvarchar](15) NULL,
	[Phone] [nvarchar](24) NULL,
	[Fax] [nvarchar](24) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [City] ON [dbo].[Customers] 
(
	[City] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CompanyName] ON [dbo].[Customers] 
(
	[CompanyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PostalCode] ON [dbo].[Customers] 
(
	[PostalCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [Region] ON [dbo].[Customers] 
(
	[Region] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shippers]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shippers](
	[ShipperID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](40) NOT NULL,
	[Phone] [nvarchar](24) NULL,
 CONSTRAINT [PK_Shippers] PRIMARY KEY CLUSTERED 
(
	[ShipperID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](40) NOT NULL,
	[ContactName] [nvarchar](30) NULL,
	[ContactTitle] [nvarchar](30) NULL,
	[Address] [nvarchar](60) NULL,
	[City] [nvarchar](15) NULL,
	[Region] [nvarchar](15) NULL,
	[PostalCode] [nvarchar](10) NULL,
	[Country] [nvarchar](15) NULL,
	[Phone] [nvarchar](24) NULL,
	[Fax] [nvarchar](24) NULL,
	[HomePage] [ntext] NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CompanyName] ON [dbo].[Suppliers] 
(
	[CompanyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PostalCode] ON [dbo].[Suppliers] 
(
	[PostalCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [nchar](5) NULL,
	[EmployeeID] [int] NULL,
	[OrderDate] [datetime] NULL,
	[RequiredDate] [datetime] NULL,
	[ShippedDate] [datetime] NULL,
	[ShipVia] [int] NULL,
	[Freight] [money] NULL,
	[ShipName] [nvarchar](40) NULL,
	[ShipAddress] [nvarchar](60) NULL,
	[ShipCity] [nvarchar](15) NULL,
	[ShipRegion] [nvarchar](15) NULL,
	[ShipPostalCode] [nvarchar](10) NULL,
	[ShipCountry] [nvarchar](15) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CustomerID] ON [dbo].[Orders] 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CustomersOrders] ON [dbo].[Orders] 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [EmployeeID] ON [dbo].[Orders] 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [EmployeesOrders] ON [dbo].[Orders] 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [OrderDate] ON [dbo].[Orders] 
(
	[OrderDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ShippedDate] ON [dbo].[Orders] 
(
	[ShippedDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ShippersOrders] ON [dbo].[Orders] 
(
	[ShipVia] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ShipPostalCode] ON [dbo].[Orders] 
(
	[ShipPostalCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](40) NOT NULL,
	[SupplierID] [int] NULL,
	[CategoryID] [int] NULL,
	[QuantityPerUnit] [nvarchar](20) NULL,
	[UnitPrice] [money] NULL,
	[UnitsInStock] [smallint] NULL,
	[UnitsOnOrder] [smallint] NULL,
	[ReorderLevel] [smallint] NULL,
	[Discontinued] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CategoriesProducts] ON [dbo].[Products] 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [CategoryID] ON [dbo].[Products] 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ProductName] ON [dbo].[Products] 
(
	[ProductName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [SupplierID] ON [dbo].[Products] 
(
	[SupplierID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [SuppliersProducts] ON [dbo].[Products] 
(
	[SupplierID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order Details]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order Details](
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Quantity] [smallint] NOT NULL,
	[Discount] [real] NOT NULL,
 CONSTRAINT [PK_Order_Details] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [OrderID] ON [dbo].[Order Details] 
(
	[OrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [OrdersOrder_Details] ON [dbo].[Order Details] 
(
	[OrderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ProductID] ON [dbo].[Order Details] 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [ProductsOrder_Details] ON [dbo].[Order Details] 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeTerritories]    Script Date: 08/12/2011 11:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeTerritories](
	[EmployeeID] [int] NOT NULL,
	[TerritoryID] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_EmployeeTerritories] PRIMARY KEY NONCLUSTERED 
(
	[EmployeeID] ASC,
	[TerritoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Orders Qry]    Script Date: 08/12/2011 11:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[Orders Qry] AS
SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, 
	Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, 
	Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, 
	Customers.CompanyName, Customers.Address, Customers.City, Customers.Region, Customers.PostalCode, Customers.Country
FROM Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GO
/****** Object:  View [dbo].[Quarterly Orders]    Script Date: 08/12/2011 11:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[Quarterly Orders] AS
SELECT DISTINCT Customers.CustomerID, Customers.CompanyName, Customers.City, Customers.Country
FROM Customers RIGHT JOIN Orders ON Customers.CustomerID = Orders.CustomerID
WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'
GO
/****** Object:  View [dbo].[Invoices]    Script Date: 08/12/2011 11:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--create view [dbo].[Invoices] AS
--SELECT Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, 
--	Orders.ShipCountry, Orders.CustomerID, Customers.CompanyName AS CustomerName, Customers.Address, Customers.City, 
--	Customers.Region, Customers.PostalCode, Customers.Country, 
--	(FirstName + ' ' + LastName) AS Salesperson, 
--	Orders.OrderID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Shippers.CompanyName As ShipperName, 
--	"Order Details".ProductID, Products.ProductName, "Order Details".UnitPrice, "Order Details".Quantity, 
--	"Order Details".Discount, 
--	(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice, Orders.Freight
--FROM 	Shippers INNER JOIN 
--		(Products INNER JOIN 
--			(
--				(Employees INNER JOIN 
--					(Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID) 
--				ON Employees.EmployeeID = Orders.EmployeeID) 
--			INNER JOIN "Order Details" ON Orders.OrderID = "Order Details".OrderID) 
--		ON Products.ProductID = "Order Details".ProductID) 
--	ON Shippers.ShipperID = Orders.ShipVia
--GO
--/****** Object:  View [dbo].[Product Sales for 1997]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Product Sales for 1997] AS
--SELECT Categories.CategoryName, Products.ProductName, 
--Sum(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS ProductSales
--FROM (Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID) 
--	INNER JOIN (Orders 
--		INNER JOIN "Order Details" ON Orders.OrderID = "Order Details".OrderID) 
--	ON Products.ProductID = "Order Details".ProductID
--WHERE (((Orders.ShippedDate) Between '19970101' And '19971231'))
--GROUP BY Categories.CategoryName, Products.ProductName
--GO
--/****** Object:  StoredProcedure [dbo].[SalesByCategory]    Script Date: 08/12/2011 11:46:02 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--CREATE PROCEDURE [dbo].[SalesByCategory]
--    @CategoryName nvarchar(15), @OrdYear nvarchar(4) = '1998'
--AS
--IF @OrdYear != '1996' AND @OrdYear != '1997' AND @OrdYear != '1998' 
--BEGIN
--	SELECT @OrdYear = '1998'
--END

--SELECT ProductName,
--	TotalPurchase=ROUND(SUM(CONVERT(decimal(14,2), OD.Quantity * (1-OD.Discount) * OD.UnitPrice)), 0)
--FROM [Order Details] OD, Orders O, Products P, Categories C
--WHERE OD.OrderID = O.OrderID 
--	AND OD.ProductID = P.ProductID 
--	AND P.CategoryID = C.CategoryID
--	AND C.CategoryName = @CategoryName
--	AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear
--GROUP BY ProductName
--ORDER BY ProductName
--GO
--/****** Object:  StoredProcedure [dbo].[CustOrdersOrders]    Script Date: 08/12/2011 11:46:02 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--CREATE PROCEDURE [dbo].[CustOrdersOrders] @CustomerID nchar(5)
--AS
--SELECT OrderID, 
--	OrderDate,
--	RequiredDate,
--	ShippedDate
--FROM Orders
--WHERE CustomerID = @CustomerID
--ORDER BY OrderID
--GO
--/****** Object:  StoredProcedure [dbo].[CustOrderHist]    Script Date: 08/12/2011 11:46:01 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--CREATE PROCEDURE [dbo].[CustOrderHist] @CustomerID nchar(5)
--AS
--SELECT ProductName, Total=SUM(Quantity)
--FROM Products P, [Order Details] OD, Orders O, Customers C
--WHERE C.CustomerID = @CustomerID
--AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID
--GROUP BY ProductName
--GO
--/****** Object:  StoredProcedure [dbo].[CustOrdersDetail]    Script Date: 08/12/2011 11:46:01 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--CREATE PROCEDURE [dbo].[CustOrdersDetail] @OrderID int
--AS
--SELECT ProductName,
--    UnitPrice=ROUND(Od.UnitPrice, 2),
--    Quantity,
--    Discount=CONVERT(int, Discount * 100), 
--    ExtendedPrice=ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2)
--FROM Products P, [Order Details] Od
--WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID
--GO
--/****** Object:  StoredProcedure [dbo].[Ten Most Expensive Products]    Script Date: 08/12/2011 11:46:02 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create procedure [dbo].[Ten Most Expensive Products] AS
--SET ROWCOUNT 10
--SELECT Products.ProductName AS TenMostExpensiveProducts, Products.UnitPrice
--FROM Products
--ORDER BY Products.UnitPrice DESC
--GO
--/****** Object:  View [dbo].[Current Product List]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Current Product List] AS
--SELECT Product_List.ProductID, Product_List.ProductName
--FROM Products AS Product_List
--WHERE (((Product_List.Discontinued)=0))
----ORDER BY Product_List.ProductName
--GO
--/****** Object:  View [dbo].[Order Details Extended]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Order Details Extended] AS
--SELECT "Order Details".OrderID, "Order Details".ProductID, Products.ProductName, 
--	"Order Details".UnitPrice, "Order Details".Quantity, "Order Details".Discount, 
--	(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice
--FROM Products INNER JOIN "Order Details" ON Products.ProductID = "Order Details".ProductID
----ORDER BY "Order Details".OrderID
--GO
--/****** Object:  View [dbo].[Products Above Average Price]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Products Above Average Price] AS
--SELECT Products.ProductName, Products.UnitPrice
--FROM Products
--WHERE Products.UnitPrice>(SELECT AVG(UnitPrice) From Products)
----ORDER BY Products.UnitPrice DESC
--GO
--/****** Object:  View [dbo].[Products by Category]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Products by Category] AS
--SELECT Categories.CategoryName, Products.ProductName, Products.QuantityPerUnit, Products.UnitsInStock, Products.Discontinued
--FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID
--WHERE Products.Discontinued <> 1
----ORDER BY Categories.CategoryName, Products.ProductName
--GO
--/****** Object:  View [dbo].[Alphabetical list of products]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Alphabetical list of products] AS
--SELECT Products.*, Categories.CategoryName
--FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID
--WHERE (((Products.Discontinued)=0))
--GO
--/****** Object:  View [dbo].[Order Subtotals]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Order Subtotals] AS
--SELECT "Order Details".OrderID, Sum(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS Subtotal
--FROM "Order Details"
--GROUP BY "Order Details".OrderID
--GO
--/****** Object:  View [dbo].[Customer and Suppliers by City]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Customer and Suppliers by City] AS
--SELECT City, CompanyName, ContactName, 'Customers' AS Relationship 
--FROM Customers
--UNION SELECT City, CompanyName, ContactName, 'Suppliers'
--FROM Suppliers
----ORDER BY City, CompanyName
--GO
--/****** Object:  View [dbo].[Sales Totals by Amount]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Sales Totals by Amount] AS
--SELECT "Order Subtotals".Subtotal AS SaleAmount, Orders.OrderID, Customers.CompanyName, Orders.ShippedDate
--FROM 	Customers INNER JOIN 
--		(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
--	ON Customers.CustomerID = Orders.CustomerID
--WHERE ("Order Subtotals".Subtotal >2500) AND (Orders.ShippedDate BETWEEN '19970101' And '19971231')
--GO
--/****** Object:  View [dbo].[Sales by Category]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Sales by Category] AS
--SELECT Categories.CategoryID, Categories.CategoryName, Products.ProductName, 
--	Sum("Order Details Extended".ExtendedPrice) AS ProductSales
--FROM 	Categories INNER JOIN 
--		(Products INNER JOIN 
--			(Orders INNER JOIN "Order Details Extended" ON Orders.OrderID = "Order Details Extended".OrderID) 
--		ON Products.ProductID = "Order Details Extended".ProductID) 
--	ON Categories.CategoryID = Products.CategoryID
--WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'
--GROUP BY Categories.CategoryID, Categories.CategoryName, Products.ProductName
----ORDER BY Products.ProductName
--GO
--/****** Object:  StoredProcedure [dbo].[Sales by Year]    Script Date: 08/12/2011 11:46:02 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create procedure [dbo].[Sales by Year] 
--	@Beginning_Date DateTime, @Ending_Date DateTime AS
--SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal, DATENAME(yy,ShippedDate) AS Year
--FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
--WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
--GO
--/****** Object:  StoredProcedure [dbo].[Employee Sales by Country]    Script Date: 08/12/2011 11:46:02 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create procedure [dbo].[Employee Sales by Country] 
--@Beginning_Date DateTime, @Ending_Date DateTime AS
--SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal AS SaleAmount
--FROM Employees INNER JOIN 
--	(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
--	ON Employees.EmployeeID = Orders.EmployeeID
--WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
--GO
--/****** Object:  View [dbo].[Summary of Sales by Quarter]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Summary of Sales by Quarter] AS
--SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal
--FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
--WHERE Orders.ShippedDate IS NOT NULL
----ORDER BY Orders.ShippedDate
--GO
--/****** Object:  View [dbo].[Summary of Sales by Year]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Summary of Sales by Year] AS
--SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal
--FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
--WHERE Orders.ShippedDate IS NOT NULL
----ORDER BY Orders.ShippedDate
--GO
--/****** Object:  View [dbo].[Category Sales for 1997]    Script Date: 08/12/2011 11:46:04 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--create view [dbo].[Category Sales for 1997] AS
--SELECT "Product Sales for 1997".CategoryName, Sum("Product Sales for 1997".ProductSales) AS CategorySales
--FROM "Product Sales for 1997"
--GROUP BY "Product Sales for 1997".CategoryName
--GO
--/****** Object:  Default [DF_Order_Details_UnitPrice]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF_Order_Details_UnitPrice]  DEFAULT (0) FOR [UnitPrice]
--GO
--/****** Object:  Default [DF_Order_Details_Quantity]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF_Order_Details_Quantity]  DEFAULT (1) FOR [Quantity]
--GO
--/****** Object:  Default [DF_Order_Details_Discount]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF_Order_Details_Discount]  DEFAULT (0) FOR [Discount]
--GO
--/****** Object:  Default [DF_Orders_Freight]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_Freight]  DEFAULT (0) FOR [Freight]
--GO
--/****** Object:  Default [DF_Products_UnitPrice]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_UnitPrice]  DEFAULT (0) FOR [UnitPrice]
--GO
--/****** Object:  Default [DF_Products_UnitsInStock]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_UnitsInStock]  DEFAULT (0) FOR [UnitsInStock]
--GO
--/****** Object:  Default [DF_Products_UnitsOnOrder]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_UnitsOnOrder]  DEFAULT (0) FOR [UnitsOnOrder]
--GO
--/****** Object:  Default [DF_Products_ReorderLevel]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_ReorderLevel]  DEFAULT (0) FOR [ReorderLevel]
--GO
--/****** Object:  Default [DF_Products_Discontinued]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_Discontinued]  DEFAULT (0) FOR [Discontinued]
--GO
--/****** Object:  Check [CK_Birthdate]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Employees]  WITH NOCHECK ADD  CONSTRAINT [CK_Birthdate] CHECK  (([BirthDate] < getdate()))
--GO
--ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [CK_Birthdate]
--GO
--/****** Object:  Check [CK_Discount]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [CK_Discount] CHECK  (([Discount] >= 0 and [Discount] <= 1))
--GO
--ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [CK_Discount]
--GO
--/****** Object:  Check [CK_Quantity]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [CK_Quantity] CHECK  (([Quantity] > 0))
--GO
--ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [CK_Quantity]
--GO
--/****** Object:  Check [CK_UnitPrice]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [CK_UnitPrice] CHECK  (([UnitPrice] >= 0))
--GO
--ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [CK_UnitPrice]
--GO
--/****** Object:  Check [CK_Products_UnitPrice]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [CK_Products_UnitPrice] CHECK  (([UnitPrice] >= 0))
--GO
--ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_Products_UnitPrice]
--GO
--/****** Object:  Check [CK_ReorderLevel]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [CK_ReorderLevel] CHECK  (([ReorderLevel] >= 0))
--GO
--ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_ReorderLevel]
--GO
--/****** Object:  Check [CK_UnitsInStock]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [CK_UnitsInStock] CHECK  (([UnitsInStock] >= 0))
--GO
--ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_UnitsInStock]
--GO
--/****** Object:  Check [CK_UnitsOnOrder]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [CK_UnitsOnOrder] CHECK  (([UnitsOnOrder] >= 0))
--GO
--ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_UnitsOnOrder]
--GO
--/****** Object:  ForeignKey [FK_CustomerCustomerDemo]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[CustomerCustomerDemo]  WITH CHECK ADD  CONSTRAINT [FK_CustomerCustomerDemo] FOREIGN KEY([CustomerTypeID])
--REFERENCES [dbo].[CustomerDemographics] ([CustomerTypeID])
--GO
--ALTER TABLE [dbo].[CustomerCustomerDemo] CHECK CONSTRAINT [FK_CustomerCustomerDemo]
--GO
--/****** Object:  ForeignKey [FK_CustomerCustomerDemo_Customers]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[CustomerCustomerDemo]  WITH CHECK ADD  CONSTRAINT [FK_CustomerCustomerDemo_Customers] FOREIGN KEY([CustomerID])
--REFERENCES [dbo].[Customers] ([CustomerID])
--GO
--ALTER TABLE [dbo].[CustomerCustomerDemo] CHECK CONSTRAINT [FK_CustomerCustomerDemo_Customers]
--GO
--/****** Object:  ForeignKey [FK_Employees_Employees]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Employees]  WITH NOCHECK ADD  CONSTRAINT [FK_Employees_Employees] FOREIGN KEY([ReportsTo])
--REFERENCES [dbo].[Employees] ([EmployeeID])
--GO
--ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Employees]
--GO
--/****** Object:  ForeignKey [FK_EmployeeTerritories_Employees]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[EmployeeTerritories]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeTerritories_Employees] FOREIGN KEY([EmployeeID])
--REFERENCES [dbo].[Employees] ([EmployeeID])
--GO
--ALTER TABLE [dbo].[EmployeeTerritories] CHECK CONSTRAINT [FK_EmployeeTerritories_Employees]
--GO
--/****** Object:  ForeignKey [FK_EmployeeTerritories_Territories]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[EmployeeTerritories]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeTerritories_Territories] FOREIGN KEY([TerritoryID])
--REFERENCES [dbo].[Territories] ([TerritoryID])
--GO
--ALTER TABLE [dbo].[EmployeeTerritories] CHECK CONSTRAINT [FK_EmployeeTerritories_Territories]
--GO
--/****** Object:  ForeignKey [FK_Order_Details_Orders]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Details_Orders] FOREIGN KEY([OrderID])
--REFERENCES [dbo].[Orders] ([OrderID])
--GO
--ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [FK_Order_Details_Orders]
--GO
--/****** Object:  ForeignKey [FK_Order_Details_Products]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Details_Products] FOREIGN KEY([ProductID])
--REFERENCES [dbo].[Products] ([ProductID])
--GO
--ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [FK_Order_Details_Products]
--GO
--/****** Object:  ForeignKey [FK_Orders_Customers]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Orders]  WITH NOCHECK ADD  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerID])
--REFERENCES [dbo].[Customers] ([CustomerID])
--GO
--ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
--GO
--/****** Object:  ForeignKey [FK_Orders_Employees]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Orders]  WITH NOCHECK ADD  CONSTRAINT [FK_Orders_Employees] FOREIGN KEY([EmployeeID])
--REFERENCES [dbo].[Employees] ([EmployeeID])
--GO
--ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Employees]
--GO
--/****** Object:  ForeignKey [FK_Orders_Shippers]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Orders]  WITH NOCHECK ADD  CONSTRAINT [FK_Orders_Shippers] FOREIGN KEY([ShipVia])
--REFERENCES [dbo].[Shippers] ([ShipperID])
--GO
--ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Shippers]
--GO
--/****** Object:  ForeignKey [FK_Products_Categories]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryID])
--REFERENCES [dbo].[Categories] ([CategoryID])
--GO
--ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
--GO
--/****** Object:  ForeignKey [FK_Products_Suppliers]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [FK_Products_Suppliers] FOREIGN KEY([SupplierID])
--REFERENCES [dbo].[Suppliers] ([SupplierID])
--GO
--ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Suppliers]
--GO
--/****** Object:  ForeignKey [FK_Territories_Region]    Script Date: 08/12/2011 11:46:03 ******/
--ALTER TABLE [dbo].[Territories]  WITH CHECK ADD  CONSTRAINT [FK_Territories_Region] FOREIGN KEY([RegionID])
--REFERENCES [dbo].[Region] ([RegionID])
--GO
--ALTER TABLE [dbo].[Territories] CHECK CONSTRAINT [FK_Territories_Region]
--GO