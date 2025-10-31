CREATE TABLE Rol (
  idRol INT IDENTITY(1,1) NOT NULL,
  nombre NVARCHAR(50) NOT NULL,
  CONSTRAINT PK_Rol PRIMARY KEY (idRol),
  CONSTRAINT UQ_Rol_nombre UNIQUE (nombre)
);


-- Creación de la tabla TipoEgreso
CREATE TABLE TipoEgreso (
  idTipoEgreso INT IDENTITY(1,1) NOT NULL,
  nombre NVARCHAR(50) NOT NULL,
  CONSTRAINT PK_TipoEgreso PRIMARY KEY (idTipoEgreso),
  CONSTRAINT UQ_TipoEgreso_nombre UNIQUE (nombre)
);

-- Creación de la tabla Usuario

CREATE TABLE Usuario (
  idUsuario INT IDENTITY(1,1) NOT NULL,
  nombreUsuario NVARCHAR(50) NOT NULL,
  contraseniaHash NVARCHAR(200) NOT NULL,
  correo NVARCHAR(150) NULL,
  fechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Usuario_fechaCreacion DEFAULT SYSUTCDATETIME(),
  activo BIT NOT NULL CONSTRAINT DF_Usuario_activo DEFAULT (1),
  idRol INT NOT NULL,
  CONSTRAINT PK_Usuario PRIMARY KEY (idUsuario),
  CONSTRAINT UQ_Usuario_nombreUsuario UNIQUE (nombreUsuario),
  CONSTRAINT FK_Usuario_Rol FOREIGN KEY (idRol) REFERENCES Rol(idRol)
);

-- Creación de la tabla Egreso
CREATE TABLE Egreso (
  idEgreso INT IDENTITY(1,1) NOT NULL,
  concepto NVARCHAR(200) NOT NULL,
  fecha DATETIME2(0) NOT NULL CONSTRAINT DF_Egreso_fecha DEFAULT SYSUTCDATETIME(),
  monto DECIMAL(12,2) NOT NULL,
  recurrente BIT NOT NULL CONSTRAINT DF_Egreso_recurrente DEFAULT (0),
  comprobanteRuta VARCHAR(80) NULL,
  idTipoEgreso INT NOT NULL,
  idUsuario INT NOT NULL,
  CONSTRAINT PK_Egreso PRIMARY KEY (idEgreso),
  CONSTRAINT CK_Egreso_monto_pos CHECK (monto > 0),
  CONSTRAINT FK_Egreso_TipoEgreso FOREIGN KEY (idTipoEgreso) REFERENCES TipoEgreso(idTipoEgreso),
  CONSTRAINT FK_Egreso_Usaurio FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)

);

CREATE TABLE ProgramaFidelizacion (
  idPrograma INT IDENTITY(1,1) NOT NULL,
  visitasParaDescuento INT NOT NULL,
  porcentajeDescuento DECIMAL(5,2) NOT NULL,
  descuentoCumpleanos BIT NOT NULL,
  montoDescuentoCumpleanos DECIMAL(12,2) NOT NULL,
  CONSTRAINT PK_ProgramaFidelizacion PRIMARY KEY (idPrograma)
);

-- Creación de la tabla Cliente
CREATE TABLE Cliente (
  idCliente INT IDENTITY(1,1) NOT NULL,
  nombre NVARCHAR(80) NOT NULL,
  apellidos NVARCHAR(120) NOT NULL,
  numero_documento NVARCHAR(20) NULL,
  telefono NVARCHAR(30) NULL,
  correo NVARCHAR(150) NULL,
  direccion NVARCHAR(200) NULL,
  fechaNacimiento DATE,
  fechaRegistro DATETIME2(0) NOT NULL CONSTRAINT DF_Cliente_fechaRegistro DEFAULT SYSUTCDATETIME(),
  visitasTotales INT NOT NULL CONSTRAINT DF_Cliente_visitasTotales DEFAULT (0),
  activo BIT NOT NULL CONSTRAINT DF_Cliente_activo DEFAULT (1),
  idPrograma INT NOT NULL,
  CONSTRAINT PK_Cliente PRIMARY KEY (idCliente),
  CONSTRAINT FK_Cliente_ProgramaFidelizacion FOREIGN KEY (idPrograma) REFERENCES ProgramaFidelizacion(idPrograma)
);

CREATE TABLE CategoriaProducto (
  idCategoriaProducto INT IDENTITY(1,1) NOT NULL,
  nombre NVARCHAR(80) NOT NULL,
  CONSTRAINT PK_CategoriaProducto PRIMARY KEY (idCategoriaProducto),
  CONSTRAINT UQ_CategoriaProducto_nombre UNIQUE (nombre)
);

-- Creación de la tabla Producto
CREATE TABLE Producto (
  idProducto INT IDENTITY(1,1) NOT NULL,
  codigo NVARCHAR(50) NOT NULL,
  nombre NVARCHAR(120) NOT NULL,
  descripcion NVARCHAR(300) NULL,
  precioCompra DECIMAL(12,2) NOT NULL,
  precioVenta DECIMAL(12,2) NOT NULL,
  stockActual INT NOT NULL CONSTRAINT DF_Producto_stockActual DEFAULT (0),
  stockMinimo INT NOT NULL CONSTRAINT DF_Producto_stockMinimo DEFAULT (0),
  activo BIT NOT NULL CONSTRAINT DF_Producto_activo DEFAULT (1),
  idCategoriaProducto INT NOT NULL,
  CONSTRAINT PK_Producto PRIMARY KEY (idProducto),
  CONSTRAINT UQ_Producto_codigo UNIQUE (codigo),
  CONSTRAINT CK_Producto_precios_nonneg CHECK (precioCompra >= 0 AND precioVenta >= 0),
  CONSTRAINT CK_Producto_stocks_nonneg CHECK (stockActual >= 0 AND stockMinimo >= 0),
  CONSTRAINT FK_Producto_CategoriaProducto FOREIGN KEY (idCategoriaProducto) REFERENCES CategoriaProducto(idCategoriaProducto)
);
-- Creación de la tabla EstadoCuenta
CREATE TABLE EstadoCuenta (
  idEstadoCuenta INT IDENTITY(1,1) NOT NULL,
  nombre NVARCHAR(30) NOT NULL,
  CONSTRAINT PK_EstadoCuenta PRIMARY KEY (idEstadoCuenta),
  CONSTRAINT UQ_EstadoCuenta_nombre UNIQUE (nombre)
);

-- Creación de la tabla Cuenta

CREATE TABLE Cuenta (
  idCuenta INT IDENTITY(1,1) NOT NULL,
  fechaHoraCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Cuenta_fechaHoraCreacion DEFAULT SYSUTCDATETIME(),
  precioEntrada DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cuenta_precioEntrada DEFAULT (0),
  fechaHoraSalida DATETIME  NULL ,
  subtotalConsumos DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cuenta_subtotalConsumos DEFAULT (0),
  montoPagado DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
  descuento DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cuenta_descuento DEFAULT (0),
  total DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cuenta_total DEFAULT (0),
  saldo DECIMAL(12,2) NOT NULL CONSTRAINT DF_Cuenta_saldo DEFAULT (0),
  idEstadoCuenta INT FOREIGN KEY REFERENCES EstadoCuenta(idEstadoCuenta) NOT NULL,
  idUsuarioCreador INT FOREIGN KEY REFERENCES Usuario(idUsuario) NOT NULL, -- Asume que Usuario ya existe,
  idCliente INT FOREIGN KEY REFERENCES Cliente(idCliente) NOT NULL, -- Asume que Cliente ya existe,
  CONSTRAINT PK_Cuenta PRIMARY KEY (idCuenta),
  CONSTRAINT CK_Cuenta_importes_nonneg CHECK (precioEntrada >= 0 AND subtotalConsumos >= 0 AND descuento >= 0 AND total >= 0),
);

-- Creación de la tabla TipoMovimiento (para MovimientoInventario)

CREATE TABLE TipoMovimiento (
  idTipoMovimiento INT IDENTITY(1,1) NOT NULL,
  nombre NVARCHAR(40) NOT NULL,
  CONSTRAINT PK_TipoMovimiento PRIMARY KEY (idTipoMovimiento),
  CONSTRAINT UQ_TipoMovimiento_nombre UNIQUE (nombre)
);

-- Creación de la tabla MovimientoInventario

CREATE TABLE MovimientoInventario (
  idMovimiento INT IDENTITY(1,1) NOT NULL,
  cantidad INT NOT NULL,
  costoUnitario DECIMAL(12,2) NOT NULL,
  costoTotal DECIMAL(12,2) NOT NULL CONSTRAINT DF_MovInv_costoTotal DEFAULT (0),
  fecha DATETIME2(0) NOT NULL CONSTRAINT DF_MovInv_fecha DEFAULT SYSUTCDATETIME(),
  observaciones NVARCHAR(300) NULL,
  idTipoMovimiento INT  NOT NULL,
  idProducto INT NOT NULL,
  idUsuario INT  NOT NULL -- {FK} idUsuario, quien realiza el movimiento
  CONSTRAINT PK_MovimientoInventario PRIMARY KEY (idMovimiento),
  CONSTRAINT CK_MovInv_cantidad_pos CHECK (cantidad > 0),
  CONSTRAINT CK_MovInv_costos_nonneg CHECK (costoUnitario >= 0 AND costoTotal >= 0),
  CONSTRAINT FK_MovInv_TipoMovimiento FOREIGN KEY (idTipoMovimiento) REFERENCES TipoMovimiento(idTipoMovimiento),
  CONSTRAINT FK_MovInv_Producto FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
  CONSTRAINT FK_MovInv_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);


-- Creación de la tabla DetalleConsumo

CREATE TABLE DetalleConsumo (
  idDetalle INT IDENTITY(1,1) NOT NULL,
  cantidad INT NOT NULL,
  precioUnitario DECIMAL(12,2) NOT NULL CONSTRAINT DF_DetalleConsumo_precioUnitario DEFAULT (0),
  subtotal DECIMAL(12,2) NOT NULL CONSTRAINT DF_DetalleConsumo_subtotal DEFAULT (0),
  idCuenta INT FOREIGN KEY REFERENCES Cuenta(idCuenta) NOT NULL,
  idProducto INT NOT NULL
  CONSTRAINT PK_DetalleConsumo PRIMARY KEY (idDetalle),
  CONSTRAINT CK_DetalleConsumo_cantidad_pos CHECK (cantidad > 0),
  CONSTRAINT CK_DetalleConsumo_importes_nonneg CHECK (precioUnitario >= 0 AND subtotal >= 0),
  CONSTRAINT FK_DetalleConsumo_Producto FOREIGN KEY (idProducto) REFERENCES Producto(idProducto)
);

-- Creación de la tabla MetodoPago

CREATE TABLE MetodoPago (
  idMetodoPago INT IDENTITY(1,1) NOT NULL,
  nombre VARCHAR(50) NOT NULL,
  CONSTRAINT PK_MetodoPago PRIMARY KEY (idMetodoPago),
  CONSTRAINT UQ_MetodoPago_nombre UNIQUE (nombre)
);

-- Creación de la tabla Pago
CREATE TABLE Pago (
  idPago INT IDENTITY(1,1) NOT NULL,
  fechaHora DATETIME2(0) NOT NULL CONSTRAINT DF_Pago_fechaHora DEFAULT SYSUTCDATETIME(),
  monto DECIMAL(12,2) NOT NULL,
  numeroReferencia NVARCHAR(100) NULL,
  idMetodoPago INT  NOT NULL,
  idCuenta INT  NOT NULL
  CONSTRAINT PK_Pago PRIMARY KEY (idPago),
  CONSTRAINT CK_Pago_monto_pos CHECK (monto > 0),
  CONSTRAINT FK_Pago_MetodoPago FOREIGN KEY (idMetodoPago) REFERENCES MetodoPago(idMetodoPago),
  CONSTRAINT FK_Pago_Cuenta FOREIGN KEY (idCuenta) REFERENCES Cuenta(idCuenta),

);


-- Creación de la tabla TipoComprobante

CREATE TABLE TipoComprobante (
  idTipoComprobante INT IDENTITY(1,1) NOT NULL,
  nombre NVARCHAR(30) NOT NULL,
  CONSTRAINT PK_TipoComprobante PRIMARY KEY (idTipoComprobante),
  CONSTRAINT UQ_TipoComprobante_nombre UNIQUE (nombre)
);


-- Creación de la tabla Comprobante

CREATE TABLE Comprobante (
  idComprobante INT IDENTITY(1,1) NOT NULL,
  serie NVARCHAR(10) NOT NULL,
  numero NVARCHAR(15) NOT NULL,
  fechaEmision DATETIME2(0) NOT NULL CONSTRAINT DF_Comprobante_fechaEmision DEFAULT SYSUTCDATETIME(),
  subtotal DECIMAL(12,2) NOT NULL CONSTRAINT DF_Comprobante_subtotal DEFAULT (0),
  igv DECIMAL(12,2) NOT NULL CONSTRAINT DF_Comprobante_igv DEFAULT (0),
  total DECIMAL(12,2) NOT NULL CONSTRAINT DF_Comprobante_total DEFAULT (0),
  idTipoComprobante INT NOT NULL,
  idCuenta INT NOT NULL,
  CONSTRAINT PK_Comprobante PRIMARY KEY (idComprobante),
  CONSTRAINT CK_Comprobante_importes_nonneg CHECK (subtotal >= 0 AND igv >= 0 AND total >= 0),
  CONSTRAINT UQ_Comprobante_SerieNumero UNIQUE (serie, numero),
  CONSTRAINT UQ_Comprobante_idCuenta UNIQUE (idCuenta),
  CONSTRAINT FK_Comprobante_TipoComprobante FOREIGN KEY (idTipoComprobante) REFERENCES TipoComprobante(idTipoComprobante),
  CONSTRAINT FK_Comprobante_Cuenta FOREIGN KEY (idCuenta) REFERENCES Cuenta(idCuenta)
);
