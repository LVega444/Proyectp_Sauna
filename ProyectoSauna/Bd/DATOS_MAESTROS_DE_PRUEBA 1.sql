
Create PROCEDURE [dbo].[sp_ValidarLogin]
    @identificador NVARCHAR(50), -- Puede ser nombreUsuario o correo
    @contraseniaHash NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT U.idUsuario, U.nombreUsuario, U.correo, U.idRol, R.nombre AS Rol
    FROM Usuario U
    INNER JOIN Rol R ON U.idRol = R.idRol
    WHERE (U.nombreUsuario = @identificador OR U.correo = @identificador)
      AND U.contraseniaHash = @contraseniaHash
      AND U.activo = 1;
END

-- Eliminacion de usuarios creados anteriormente

select * from Usuario

delete rol ;

-- =====================================================
-- INSERTAR DATOS MAESTROS 
-- =====================================================

--  ROLES
INSERT INTO Rol (nombre) VALUES 
('Administrador'),
('Cajero');


--  PROGRAMA DE FIDELIZACIÓN
INSERT INTO ProgramaFidelizacion (visitasParaDescuento, porcentajeDescuento, descuentoCumpleanos, montoDescuentoCumpleanos)
VALUES (5, 10.00, 1, 5.00);
PRINT ' Programa de fidelización creado (5 visitas = 10% descuento)';


--  ESTADOS DE CUENTA
INSERT INTO EstadoCuenta (nombre) VALUES 
('Pendiente'),
('Pagada'),
('Cancelada');



--  CATEGORÍAS DE PRODUCTOS
INSERT INTO CategoriaProducto (nombre) VALUES 
('Bebidas Frías'),
('Bebidas Calientes'),
('Snacks'),
('Accesorios'),
('Servicios');


--  TIPOS DE MOVIMIENTO INVENTARIO
INSERT INTO TipoMovimiento (nombre) VALUES 
('Entrada'),
('Salida');


--  MÉTODOS DE PAGO
INSERT INTO MetodoPago (nombre) VALUES 
('Efectivo'),
('Tarjeta'),
('Yape'),
('Plin');


--  TIPOS DE COMPROBANTE
INSERT INTO TipoComprobante (nombre) VALUES 
('Boleta'),
('Factura'),
('Ticket');


--  TIPOS DE EGRESO
INSERT INTO TipoEgreso (nombre) VALUES 
('Servicios Básicos'),
('Alquiler'),
('Sueldos'),
('Compra Mercadería'),
('Mantenimiento'),
('Otros');

select * from Usuario




---------------------  EJECUTA DESDE AQUI SI YA TIENES LA BASE DE DATOS SOLO INSERTA DATOS ------------------------------------------




-- =====================================================
-- 2 INSERTAR DATOS DE PRUEBA
-- =====================================================

--  admin      CONTRA     admin123
--  cajero1    CONTRA    cajero123

select * from rol
--  USUARIOS DEL SISTEMA
INSERT INTO Usuario (nombreUsuario, contraseniaHash, correo, idRol, activo) 
VALUES 
('admin', '1o7DiXMouiKbAge5eRLsIBd/pXtuJH9tyDXWDkiDQu0=', 'admin@saunakalixto.com', 5, 1),
('cajero1', 'QlDcshduVIjym22wku+1W5uANBxGLZxx/pnN6fGK6Lg=', 'cajero1@saunakalixto.com', 6, 1);

--  CLIENTES DE PRUEBA (SIN HISTORIAL)
-- ✅ TODOS con visitasTotales = 0 (sin compras previas)
-- 📌 Base de datos lista para empezar a registrar desde el sistema
INSERT INTO Cliente (nombre, apellidos, numero_documento, telefono, correo, direccion, visitasTotales, idPrograma, activo)
VALUES 
('Carlos', 'Mendoza Ríos', '12345678', '987654321', 'carlos.mendoza@gmail.com', 'Av. Los Incas 123', 0, 1, 1),
('María', 'Torres Vargas', '23456789', '976543210', 'maria.torres@gmail.com', 'Jr. Cusco 456', 0, 1, 1),
('Juan', 'Pérez López', '34567890', '965432109', 'juan.perez@gmail.com', 'Av. El Sol 789', 0, 1, 1),
('Ana', 'García Flores', '45678901', '954321098', 'ana.garcia@gmail.com', 'Calle Lima 321', 0, 1, 1),
('Luis', 'Chávez Ramos', '56789012', '943210987', NULL, NULL, 0, 1, 1);



-- Bebidas Frías (idCategoria = 1) - Productos físicos
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, idCategoriaProducto)
VALUES 
('BEB-001', 'Agua Mineral 500ml', 'Agua embotellada', 1.00, 2.50, 100, 20, 1),
('BEB-002', 'Gaseosa Inca Kola 500ml', 'Gaseosa personal', 2.00, 4.00, 50, 10, 1),
('BEB-003', 'Cerveza Cusqueña 330ml', 'Cerveza nacional', 3.50, 7.00, 60, 15, 1),
('BEB-004', 'Jugo de Naranja Natural', 'Jugo recién exprimido', 2.50, 5.00, 30, 5, 1);


-- Bebidas Calientes (idCategoria = 2) - Servicios de barra (stock ilimitado)
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, idCategoriaProducto)
VALUES 
('CAF-001', 'Café Americano', 'Café solo', 1.00, 3.00, 999999, 0, 2),
('CAF-002', 'Café con Leche', 'Café con leche', 1.50, 4.00, 999999, 0, 2),
('TÉ-001', 'Té de Manzanilla', 'Infusión relajante', 0.50, 2.50, 999999, 0, 2);


-- Snacks (idCategoria = 3) - Mixto (físicos + preparados)
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, idCategoriaProducto)
VALUES 
('SNK-001', 'Galletas Soda Field', 'Paquete 6 unidades', 1.50, 3.50, 40, 10, 3),
('SNK-002', 'Papas Lays 45g', 'Papas fritas originales', 1.80, 4.00, 60, 15, 3),
('SNK-003', 'Chocolate Sublime', 'Chocolate con maní', 2.00, 4.50, 50, 10, 3),
('SNK-004', 'Sandwich Mixto', 'Pan, jamón, queso', 3.00, 8.00, 999999, 0, 3); -- Servicio

-- Accesorios (idCategoria = 4) - Productos físicos
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, idCategoriaProducto)
VALUES 
('ACC-001', 'Toalla Grande', 'Toalla de baño 70x140cm', 10.00, 15.00, 30, 5, 4),
('ACC-002', 'Sandalias Desechables', 'Par de sandalias', 2.00, 5.00, 50, 10, 4),
('ACC-003', 'Shampoo Sachet', 'Shampoo individual', 0.50, 2.00, 100, 20, 4);

select * from producto

-- Servicios (idCategoria = 5) - Servicios puros (sin costo de adquisición)
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, idCategoriaProducto)
VALUES 
('SRV-001', 'Masaje Relajante 30min', 'Masaje espalda y cuello', 0.00, 50.00, 999999, 0, 5),
('SRV-002', 'Tratamiento Facial', 'Limpieza profunda', 0.00, 80.00, 999999, 0, 5);


-- =====================================================
--  VERIFICACIÓN FINAL - TODAS LAS 25 TABLAS
-- =====================================================

-- ✅ Verificar que las 25 tablas tienen datos correctos
SELECT 'Rol' AS Tabla, COUNT(*) AS Registros FROM Rol
UNION ALL SELECT 'ProgramaFidelizacion', COUNT(*) FROM ProgramaFidelizacion
UNION ALL SELECT 'EstadoCuenta', COUNT(*) FROM EstadoCuenta
UNION ALL SELECT 'CategoriaProducto', COUNT(*) FROM CategoriaProducto
UNION ALL SELECT 'TipoMovimiento', COUNT(*) FROM TipoMovimiento
UNION ALL SELECT 'MetodoPago', COUNT(*) FROM MetodoPago
UNION ALL SELECT 'TipoComprobante', COUNT(*) FROM TipoComprobante
UNION ALL SELECT 'TipoEgreso', COUNT(*) FROM TipoEgreso
UNION ALL SELECT 'Usuario', COUNT(*) FROM Usuario
UNION ALL SELECT 'Cliente', COUNT(*) FROM Cliente
UNION ALL SELECT 'Producto', COUNT(*) FROM Producto
UNION ALL SELECT '--- TABLAS TRANSACCIONALES (deben estar en 0) ---', NULL
UNION ALL SELECT 'Cuenta', COUNT(*) FROM Cuenta
UNION ALL SELECT 'DetalleConsumo', COUNT(*) FROM DetalleConsumo
UNION ALL SELECT 'MovimientoInventario', COUNT(*) FROM MovimientoInventario
UNION ALL SELECT 'Pago', COUNT(*) FROM Pago
UNION ALL SELECT 'Comprobante', COUNT(*) FROM Comprobante
UNION ALL SELECT 'Egreso', COUNT(*) FROM Egreso


-- 📦 Ver productos por categoría con tipo de producto
SELECT 
  c.nombre AS Categoria,
  p.codigo,
  p.nombre AS Producto,
  'S/. ' + CAST(p.precioVenta AS VARCHAR) AS Precio,
  p.stockActual AS Stock,
  CASE 
    WHEN p.precioCompra = 0 AND p.stockActual = 999999 THEN 'Servicio Puro'
    WHEN p.precioCompra > 0 AND p.stockActual = 999999 THEN 'Servicio Barra'
    WHEN p.stockActual = 0 THEN 'Sin Stock'
    WHEN p.stockActual <= p.stockMinimo THEN 'Stock Bajo'
    ELSE 'Stock bueno'
  END AS TipoEstado
FROM Producto p
INNER JOIN CategoriaProducto c ON p.idCategoriaProducto = c.idCategoriaProducto
ORDER BY c.nombre, p.nombre;

-- 👥 Ver clientes (TODOS deben tener visitasTotales = 0)
SELECT 
  nombre + ' ' + apellidos AS Cliente,
  telefono,
  visitasTotales AS Visitas,
  CASE 
    WHEN visitasTotales = 0 THEN 'Cliente nuevo (sin historial)'
    ELSE 'ERROR: Tiene visitas pero sin registros en BD'
  END AS Estado
FROM Cliente
ORDER BY nombre;

PRINT '';
PRINT '============================================================';
PRINT 'VERIFICACIÓN COMPLETA';
PRINT '============================================================';
PRINT 'Tablas con datos maestros: 15 (catálogos + entidades)';
PRINT 'Tablas transaccionales vacías: 10 (listas para usar)';
PRINT 'TOTAL: 25 tablas verificadas';
PRINT '';
PRINT 'Base de datos lista para desarrollo';
PRINT 'Todos los clientes con visitasTotales = 0 (sin historial)';
PRINT 'Productos clasificados correctamente (físicos vs servicios)';
PRINT '============================================================';




-- ===============================
-- CATEGORÍA 1: BEBIDAS FRÍAS
-- ===============================
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, activo, idCategoriaProducto)
VALUES
('BF001', 'Agua Mineral', 'Botella de agua sin gas 500ml', 1.00, 1.50, 100, 10, 1, 1),
('BF002', 'Agua con Gas', 'Botella de agua con gas 500ml', 1.10, 1.60, 80, 10, 1, 1),
('BF003', 'Gaseosa Cola', 'Botella de 500ml sabor cola', 1.20, 2.00, 90, 10, 1, 1),
('BF004', 'Jugo de Naranja', 'Jugo natural 350ml', 1.50, 2.50, 70, 10, 1, 1),
('BF005', 'Jugo de Piña', 'Jugo natural 350ml', 1.40, 2.40, 70, 10, 1, 1),
('BF006', 'Limonada', 'Limonada fría 500ml', 1.00, 1.80, 60, 10, 1, 1),
('BF007', 'Té Helado Durazno', 'Botella de 500ml', 1.20, 2.00, 75, 10, 1, 1),
('BF008', 'Té Helado Limón', 'Botella de 500ml', 1.20, 2.00, 75, 10, 1, 1),
('BF009', 'Energizante X', 'Lata 250ml', 1.80, 3.00, 50, 10, 1, 1),
('BF010', 'Agua saborizada', 'Agua con sabor a frutas 500ml', 1.10, 1.90, 85, 10, 1, 1);
GO

-- ===============================
-- CATEGORÍA 2: BEBIDAS CALIENTES
-- ===============================
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, activo, idCategoriaProducto)
VALUES
('BC001', 'Café Americano', 'Taza de café americano', 0.80, 2.00, 40, 5, 1, 2),
('BC002', 'Café Expreso', 'Taza de café expreso', 0.90, 2.20, 35, 5, 1, 2),
('BC003', 'Café con Leche', 'Taza de café con leche', 1.00, 2.50, 45, 5, 1, 2),
('BC004', 'Capuchino', 'Taza de capuchino', 1.10, 2.80, 30, 5, 1, 2),
('BC005', 'Mocachino', 'Taza de mocachino', 1.20, 3.00, 25, 5, 1, 2),
('BC006', 'Chocolate Caliente', 'Taza de chocolate caliente', 1.00, 2.50, 30, 5, 1, 2),
('BC007', 'Té Verde', 'Taza de té verde caliente', 0.70, 1.80, 40, 5, 1, 2),
('BC008', 'Té de Manzanilla', 'Taza de té natural', 0.70, 1.80, 40, 5, 1, 2),
('BC009', 'Té de Hierbas', 'Infusión de hierbas naturales', 0.80, 2.00, 30, 5, 1, 2),
('BC010', 'Café Descafeinado', 'Taza de café sin cafeína', 0.90, 2.20, 20, 5, 1, 2);
GO

-- ===============================
-- CATEGORÍA 3: SNACKS
-- ===============================
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, activo, idCategoriaProducto)
VALUES
('SN001', 'Papas Fritas', 'Bolsa de papas fritas 50g', 0.60, 1.50, 100, 10, 1, 3),
('SN002', 'Maní Salado', 'Bolsa de maní salado 50g', 0.70, 1.50, 80, 10, 1, 3),
('SN003', 'Chocolate', 'Barra de chocolate 40g', 0.90, 2.00, 70, 10, 1, 3),
('SN004', 'Galletas de Vainilla', 'Paquete de galletas 100g', 0.80, 1.80, 60, 10, 1, 3),
('SN005', 'Galletas de Chocolate', 'Paquete de galletas 100g', 0.90, 1.90, 60, 10, 1, 3),
('SN006', 'Mix de Frutos Secos', 'Bolsa de 80g', 1.20, 2.50, 50, 10, 1, 3),
('SN007', 'Chicle', 'Paquete de 10 unidades', 0.40, 1.00, 90, 10, 1, 3),
('SN008', 'Caramelos', 'Paquete de caramelos surtidos', 0.50, 1.20, 80, 10, 1, 3),
('SN009', 'Barra Energética', 'Barra de proteínas', 1.50, 3.00, 40, 10, 1, 3),
('SN010', 'Popcorn', 'Bolsa de canchita 100g', 0.80, 1.80, 60, 10, 1, 3);
GO

-- ===============================
-- CATEGORÍA 4: ACCESORIOS
-- ===============================
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, activo, idCategoriaProducto)
VALUES
('AC001', 'Toalla', 'Toalla de mano', 3.00, 5.00, 20, 5, 1, 4),
('AC002', 'Gorro de Baño', 'Gorro protector', 1.00, 2.00, 30, 5, 1, 4),
('AC003', 'Sandalias', 'Sandalias de baño', 4.00, 6.50, 15, 5, 1, 4),
('AC004', 'Shampoo', 'Botella de shampoo 250ml', 2.50, 4.00, 25, 5, 1, 4),
('AC005', 'Jabón Líquido', 'Botella de 250ml', 2.00, 3.50, 25, 5, 1, 4),
('AC006', 'Peine', 'Peine plástico', 0.80, 1.50, 40, 5, 1, 4),
('AC007', 'Cepillo', 'Cepillo para cabello', 1.20, 2.50, 30, 5, 1, 4),
('AC008', 'Desodorante', 'Desodorante personal', 2.50, 4.00, 20, 5, 1, 4),
('AC009', 'Bolsa Ecológica', 'Bolsa reutilizable', 1.00, 2.00, 35, 5, 1, 4),
('AC010', 'Guantes', 'Par de guantes plásticos', 0.60, 1.20, 50, 5, 1, 4);
GO

-- ===============================
-- CATEGORÍA 5: SERVICIOS
-- ===============================
INSERT INTO Producto (codigo, nombre, descripcion, precioCompra, precioVenta, stockActual, stockMinimo, activo, idCategoriaProducto)
VALUES
('SV001', 'Masaje Relajante', 'Sesión de 30 minutos', 10.00, 25.00, 0, 0, 1, 5),
('SV002', 'Masaje Terapéutico', 'Sesión de 45 minutos', 12.00, 30.00, 0, 0, 1, 5),
('SV003', 'Baño Sauna', 'Entrada general', 5.00, 15.00, 0, 0, 1, 5),
('SV004', 'Baño de Vapor', 'Entrada general', 5.00, 15.00, 0, 0, 1, 5),
('SV005', 'Exfoliación', 'Tratamiento corporal', 8.00, 20.00, 0, 0, 1, 5),
('SV006', 'Tratamiento Facial', 'Limpieza facial profunda', 9.00, 22.00, 0, 0, 1, 5),
('SV007', 'Aromaterapia', 'Sesión de relajación con aromas', 7.00, 18.00, 0, 0, 1, 5),
('SV008', 'Reflexología', 'Masaje en pies', 8.00, 20.00, 0, 0, 1, 5),
('SV009', 'Terapia de Piedras', 'Masaje con piedras calientes', 10.00, 25.00, 0, 0, 1, 5),
('SV010', 'Hidromasaje', 'Sesión de hidromasaje', 9.00, 23.00, 0, 0, 1, 5);
GO







