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