create database AgenciadeTours
use AgenciadeTours

CREATE TABLE Paises (
PaisID INT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(100) NOT NULL
)

CREATE TABLE Destinos (
DestinoID INT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(150) NOT NULL,
PaisID INT NOT NULL,
Dias_Duracion INT NOT NULL CHECK (Dias_Duracion BETWEEN 0 AND 365),
Horas_Duracion INT NOT NULL CHECK (Horas_Duracion BETWEEN 0 AND 23),

CONSTRAINT FK_Destinos_Paises
FOREIGN KEY (PaisID) REFERENCES Paises(PaisID)
ON DELETE CASCADE
)

CREATE TABLE Tours (
TourID INT IDENTITY(1,1) PRIMARY KEY,
Nombre NVARCHAR(200) NOT NULL,
PaisID INT NOT NULL,
DestinoID INT NOT NULL,
Fecha DATE NOT NULL,
Hora TIME NOT NULL,
Precio DECIMAL(18,2) NOT NULL CHECK (Precio >= 0),

CONSTRAINT FK_Tours_Paises
FOREIGN KEY (PaisID) REFERENCES Paises(PaisID),

CONSTRAINT FK_Tours_Destinos
FOREIGN KEY (DestinoID) REFERENCES Destinos(DestinoID)
)

CREATE INDEX IX_Destinos_Paises ON Destinos(PaisID);
CREATE INDEX IX_Tours_Paises ON Tours(PaisID);
CREATE INDEX IX_Tours_Destinos ON Tours(DestinoID);

INSERT INTO Paises (Nombre)
VALUES ('República Dominicana'), ('México'), ('España')

INSERT INTO Destinos (Nombre, PaisID, Dias_Duracion, Horas_Duracion)
VALUES ('Punta Cana', 1, 5, 6), ('Cancún Hotel Zone', 2, 4, 8), ('Madrid Centro Histórico', 3, 3, 4);

INSERT INTO Tours (Nombre, PaisID, DestinoID, Fecha, Hora, Precio)
VALUES ('Tour Playa Bávaro Premium', 1, 1, '2025-07-15', '09:00', 150.00), ('Tour Isla Mujeres Deluxe', 2, 2, '2025-08-10', '08:30', 220.00),
('Tour Museo del Prado & Caminata Histórica', 3, 3, '2025-09-05', '10:00', 180.00);

select * from Paises