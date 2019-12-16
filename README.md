# RedisExample
Kullanılmakta olan; .Net Core Mvc Web ve .Net Core Console uygulamalarından oluşan bir (örnek) e-ticaret sistemine Redis'in nasıl implemente edildiğini anlattığım yazının detaylarına http://umutonur.com/net-core-mvc-web-uygulamalarinda-redis-kullanimi/ adresinden ulaşabilirsiniz. 

Örnekte kullandığım veriler için aşağıdaki scripti kullanabilirsiniz;
```
CREATE TABLE dbo.Products
(
	Id int identity,
	Name nvarchar(max),
	Barcode nvarchar(max),
	Price decimal(18,2) not null,
	Quantity int not null,
	InSales bit not null,
	ModifiedDate datetime2 not null,
	constraint PK_Products
		primary key (Id)
)
GO

CREATE TABLE dbo.__EFMigrationsHistory
(
	MigrationId nvarchar(150) not null,
	ProductVersion nvarchar(32) not null,
	constraint PK___EFMigrationsHistory
		primary key (MigrationId)
)
GO

INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (1, 'Dell Inspiron 3593 Intel Core i5', '123456789', 3499.00, 5, 1, '2019-12-11 21:31:54.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (2, 'HP 15-DW1000NT Intel Core i5', '234567890', 3999.00, 0, 1, '2019-12-11 21:32:52.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (3, 'Asus X509FB-BR127T Intel Core i5', '345678901', 4299.00, 0, 1, '2019-12-11 21:33:34.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (4, 'Lenovo IdeaPad S145-15IW Intel Core i5', '456789123', 3898.00, 3, 1, '2019-12-11 21:34:17.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (5, 'Lenovo Ideapad S145-14IWL Intel Celeron', '567891230', 1999.00, 2, 1, '2019-12-11 21:35:11.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (6, 'HP 15-BS154NT Intel Core i3', '678901234', 2049.00, 1, 1, '2019-12-11 21:37:11.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (7, 'Asus X540LA-XX1017D Intel Core i3', '789012345', 2540.00, 3, 1, '2019-12-11 21:37:53.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (8, 'Lenovo V130 Intel Core i3', '890123456', 2420.00, 0, 1, '2019-12-11 21:38:51.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (9, 'Acer Aspire A315-53G Intel Core i3', '901234567', 2799.00, 12, 1, '2019-12-11 21:40:25.0000000');
INSERT INTO eCommerceDb.dbo.Products (Id, Name, Barcode, Price, Quantity, InSales, ModifiedDate) VALUES (10, 'MSI Alpha 15 A3DD-023XTR AMD Ryzen 7', '012345678', 8379.00, 1, 1, '2019-12-11 21:44:12.9107133');
GO
```
