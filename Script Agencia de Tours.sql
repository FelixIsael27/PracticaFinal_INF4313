create database AgenciadeTours
use AgenciadeTours

CREATE TABLE Paises (
    PaisID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);