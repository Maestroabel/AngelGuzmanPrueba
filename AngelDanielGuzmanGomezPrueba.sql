create database AngelPruebaDB
use AngelPruebaDB

--Tabla Persona: Contiene ID, Nombre, FechaDeNacimiento

create table Persona(
ID int primary key identity(1,1),
Nombre varchar(30) not null unique,
FechaDeNacimiento datetime not null
)

--Tabla Usuario: Contiene ID, Nombre, Password

create table Usuario(
ID int primary key identity(1,1),
Nombre varchar(30) not null unique,
Password varchar(44) not null
)