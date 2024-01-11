/* 
 * SQL Server Script
 *
 * This script can be executed from MS Sql Server Management Studio Express,
 * but also it is possible to use a command Line syntax:
 *
 *    > sqlcmd.exe -U [user] -P [password] -I -i SqlServerCreateTables.sql
 *
 */



/* 
 * Drop tables.                                                             
 * NOTE: before dropping a table (when re-executing the script), the tables 
 * having columns acting as foreign keys of the table to be dropped must be 
 * dropped first (otherwise, the corresponding checks on those tables could 
 * not be done).                                                            
 */
 
 USE tfg

 /* Drop Table Comment if already exists */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Carga]') 
AND type in ('U')) DROP TABLE [Carga]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Suministra]') 
AND type in ('U')) DROP TABLE [Suministra]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Tarifa]') 
AND type in ('U')) DROP TABLE [Tarifa]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[SeEncuentra]') 
AND type in ('U')) DROP TABLE [SeEncuentra]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Bateria]') 
AND type in ('U')) DROP TABLE [Bateria]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Consumo]') 
AND type in ('U')) DROP TABLE [Consumo]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Ubicacion]') 
AND type in ('U')) DROP TABLE [Ubicacion]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Estado]') 
AND type in ('U')) DROP TABLE [Estado]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[Usuario]') 
AND type in ('U')) DROP TABLE [Usuario]
GO







/*
 * Create tables.
 * Tables are created. Indexes required for the 
 * most common operations are also defined.
 */

/*  Ubicacion */

CREATE TABLE Ubicacion (
	ubicacionId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	codigoPostal BIGINT NOT NULL,
	localidad VARCHAR(30) NOT NULL,
	calle VARCHAR(40) NOT NULL,
	portal VARCHAR(30),
	numero BIGINT NOT NULL,
	etiqueta VARCHAR(50),
	bateriaSuministradora BIGINT ,

	CONSTRAINT [PK_UBICACION] PRIMARY KEY (ubicacionId),

)

PRINT N'Tabla de Ubicacion creada.'
GO

/*  Consumo */

CREATE TABLE Consumo (
	consumoId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	ubicacionId BIGINT NOT NULL,
	kwTotal FLOAT ,
	fecha DATETIME NOT NULL,
	horaIni TIME NOT NULL,
	horaFin TIME ,
	consumoActual FLOAT NOT NULL,
	

	CONSTRAINT [PK_CONSUMO] PRIMARY KEY (consumoId),

	CONSTRAINT [FK_UBICACION_BATERIA_CONSUMO] FOREIGN KEY (ubicacionId)
		REFERENCES Ubicacion (ubicacionId) ON DELETE CASCADE,
)

PRINT N'Tabla de Consumo creada.'
GO
/*  Estado */

CREATE TABLE Estado (
	estadoId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	nombre VARCHAR(20) NOT NULL,

	CONSTRAINT [PK_ESTADO] PRIMARY KEY (estadoId),
)

PRINT N'Tabla de Estado creada.'
GO


CREATE TABLE Usuario (
	usuarioId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	nombre  VARCHAR(30) NOT NULL,
	apellido1  VARCHAR(30) NOT NULL,
	apellido2  VARCHAR(30),
	email  VARCHAR(30) NOT NULL,
	contraseña VARCHAR(100) NOT NULL,
	telefono VARCHAR(9) NOT NULL,
	idioma varchar(30) NULL,
	pais varchar(30) NULL,

	CONSTRAINT [PK_USUARIO] PRIMARY KEY (usuarioId),

)

PRINT N'Tabla de Usuario creada.'
GO

CREATE TABLE Bateria (
	bateriaId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	ubicacionId BIGINT NOT NULL,
	usuarioId BIGINT NOT NULL,
	precioMedio FLOAT NOT NULL,
	kwHAlmacenados FLOAT NOT NULL,
	almacenajeMaximoKwH FLOAT NOT NULL,
	fechaDeAdquisicion DATETIME NOT NULL,
	marca VARCHAR(30) NOT NULL,
	modelo VARCHAR(40) NOT NULL,
	ratioCarga FLOAT NOT NULL,
	ratioCompra FLOAT NOT NULL,
	ratioUso FLOAT NOT NULL,
	estadoBateria BIGINT NULL,
	capacidadCargador FLOAT NOT NULL,

	CONSTRAINT [PK_BATERIA] PRIMARY KEY (bateriaId),

	CONSTRAINT [FK_BATERIA_UBICACION] FOREIGN KEY (ubicacionId)
		REFERENCES Ubicacion (ubicacionId) ON DELETE CASCADE,

	CONSTRAINT [FK_BATERIA_USUARIO] FOREIGN KEY (usuarioId)
		REFERENCES Usuario (usuarioId) ON DELETE CASCADE,


)

PRINT N'Tabla de Bateria creada.'
GO

/*  Tarifa */
CREATE TABLE Tarifa (
	tarifaId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	precio BIGINT NOT NULL,
	hora BIGINT NOT NULL,
	fecha DATETIME NOT NULL,
	
	CONSTRAINT [PK_TARIFA] PRIMARY KEY (tarifaId),
)

PRINT N'Tabla de Tarifa creada.'
GO

/*  Estado en el que se encuentra la bateria */
CREATE TABLE SeEncuentra (
	seEncuentraId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	horaIni TIME NOT NULL,
	horaFin TIME ,
	fecha DATETIME NOT NULL,
	bateriaId BIGINT NOT NULL,
	estadoId BIGINT NOT NULL,
	
	CONSTRAINT [PK_SEENCUENTRA] PRIMARY KEY (seEncuentraId),

	CONSTRAINT [FK_BATERIA_SEENCUENTRA] FOREIGN KEY (bateriaId)
		REFERENCES Bateria (bateriaId) ON DELETE CASCADE,

	CONSTRAINT [FK_ESTADO_SEENCUENTRA] FOREIGN KEY (estadoId)
		REFERENCES Estado (estadoId) ON DELETE CASCADE,
)

PRINT N'Tabla de SeEncuentra creada.'
GO

CREATE TABLE Carga (
	cargaId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	bateriaId BIGINT NOT NULL,
	tarifaId BIGINT NOT NULL,
	horaIni TIME NOT NULL,
	horaFin TIME NOT NULL,
	kwH FLOAT NOT NULL,
	
	CONSTRAINT [PK_CARGA] PRIMARY KEY (cargaId),

	CONSTRAINT [FK_BATERIA_CARGA] FOREIGN KEY (bateriaId)
		REFERENCES Bateria (bateriaId) ON DELETE CASCADE,

	CONSTRAINT [FK_TARIFA_CARGA] FOREIGN KEY (tarifaId)
		REFERENCES Tarifa (tarifaId) ON DELETE CASCADE,
)

PRINT N'Tabla de Carga creada.'
GO

/*  Gestor */
CREATE TABLE Suministra (
	suministraId BIGINT IDENTITY(1,1) UNIQUE NOT NULL,
	bateriaId BIGINT NOT NULL,
	tarifaId BIGINT NOT NULL,
	horaIni TIME NOT NULL,
	horaFin TIME NOT NULL,
	kwH FLOAT NOT NULL,
	ahorro FLOAT NOT NULL,
	
	CONSTRAINT [PK_SUMINISTRA] PRIMARY KEY (suministraId),

	CONSTRAINT [FK_BATERIA_SUMINISTRA] FOREIGN KEY (bateriaId)
		REFERENCES Bateria (bateriaId) ON DELETE CASCADE,

	CONSTRAINT [FK_TARIFA_SUMINISTRA] FOREIGN KEY (tarifaId)
		REFERENCES Tarifa (tarifaId) ON DELETE CASCADE,
)

PRINT N'Tabla de Tarifa creada.'
GO
