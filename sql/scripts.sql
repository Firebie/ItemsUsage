create table settings
(
	name nvarchar(4000),
	value ntext,
	constraint settings_pk primary key(name)
)

create table cars
(
  id int identity, 
  description ntext, 
  constraint cars_pk primary key(id)
)

create table inventories
(
  id int identity, 
  code nvarchar(4000), 
  description ntext, 
  price money, 
  constraint inventories_pk primary key(id)
)

create table orders
(
  id int identity,
  car_id int, 
  order_datetime datetime, 
  constraint orders_pk primary key(id)
)

create table order_items
(
  id int identity,
  order_id int, 
  inventory_id int, 
  item_quantity int, 
  item_price money, 
  item_datetime datetime, 
  constraint order_items_pk primary key(id)
)
