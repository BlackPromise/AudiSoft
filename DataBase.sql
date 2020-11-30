USE [master]
GO

CREATE DATABASE App
go

use app
go

Create table Product(
	ProductId int primary key identity(1,1) not null,
	ProductName varchar(50) not null,
	Price decimal(18,2) not null
);
go

insert into Product values
('Coca Cola',2500),
('Pony Malta',1500),
('Colombiana',2500),
('Pepsi',2400),
('H2 Oh',2300),
('Cola & Pola',2000),
('Poker',2000),
('Aguila',2000),
('Red Bull',4500),
('BBC',3500)
go

create table Quotation(
	QuotationId int primary key identity(1,1) not null,
	SessionValue varchar(150) not null,
	GenerateDate datetime not null default(getdate()),
	Client varchar(50) null,
	Ruc varchar(20) null,
	Seller varchar(50) null,
)
go

create table QuotationDetail(
	QuotationDetailId int primary key identity(1,1) not null,
	QuotationId int not null foreign key references Quotation(QuotationId),
	ProductId int not null foreign key references Product(ProductId),
	Quantity int not null,
	Amount decimal(18,2) not null,
	Tax decimal(18,2) not null default(0),
	Discount decimal(18,2) not null default(0),
	Total as (Amount+Tax-Discount) * Quantity
)
go

create procedure Sp_Product_Search
as begin
	
	select 
		ProductId ,
		ProductName,
		Price
	from Product with(nolock)

end
go

create procedure Sp_Quotation_Search(
	@SessionValue varchar(150)
)
as begin
	
	select 
		QuotationId,
		SessionValue,
		GenerateDate,
		Client ,
		Ruc,
		Seller
	from Quotation with(nolock)
	where SessionValue=@SessionValue

end
go

create procedure Sp_QuotationDetail_Search(
	@SessionValue varchar(150)
)
as begin
	
	select 
		QD.QuotationDetailId,
		QD.QuotationId,
		QD.QuotationId,
		QD.ProductId,
		P.ProductName,
		P.Price,
		QD.Quantity,
		QD.Amount,
		QD.Tax,
		QD.Discount,
		QD.Total
	from Quotation Q with(nolock)
	inner join QuotationDetail QD with(nolock)
	on Q.QuotationId = QD.QuotationId
	inner join Product P with(nolock)
	on P.ProductId = QD.ProductId
	where Q.SessionValue=@SessionValue

end
go

create procedure Sp_Quotation_InsertOrUpdate(
	@Result int = 0 output,
	@SessionValue varchar(150),
	@GenerateDate datetime,
	@Client varchar(50),
	@Ruc varchar(20),
	@Seller varchar(50)
)
as begin
	
	if exists(
		select 
			QuotationId
		from Quotation Q with(nolock)
		where Q.SessionValue=@SessionValue
	) begin
		
		update Quotation set
			SessionValue=@SessionValue,
			GenerateDate=@GenerateDate,
			Client=@Client ,
			Ruc=@Ruc,
			Seller=@Seller
		where SessionValue=@SessionValue;

	end else begin

		insert into Quotation values(
			@SessionValue,
			@GenerateDate,
			@Client ,
			@Ruc,
			@Seller
		);

	end

	select 
		@Result = QuotationId
	from Quotation with(nolock)
	where SessionValue=@SessionValue;
	return @Result;
end
go

create procedure Sp_QuotationDetail_Delete(
	@Result int =0 output,
	@SessionValue varchar(150)
)
as begin
	
	delete QuotationDetail
	where QuotationId in(
		select 
			QuotationId 
		from Quotation with(nolock)
		where SessionValue=@SessionValue
	)

	set @Result = @@rowcount;

end
go

create procedure Sp_QuotationDetail_Insert(
	@Result int =0 output,
	@QuotationId int,
	@ProductId int ,
	@Quantity int,
	@Amount decimal(18,2) ,
	@Tax decimal(18,2) ,
	@Discount decimal(18,2)
)
as begin
	
	insert into QuotationDetail values(
		@QuotationId,
		@ProductId,
		@Quantity,
		@Amount ,
		@Tax,
		@Discount
	)

	set @Result = @@identity;

end
go

create procedure Sp_Quotation_Delete(
	@Result int =0 output,
	@SessionValue varchar(150)
)
as begin
	
	delete Quotation
	where SessionValue=@SessionValue;

	set @Result = @@rowcount;

end

