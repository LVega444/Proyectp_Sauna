using ProyectoSauna.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSauna.Models.DTOs

    {
    public class ProductoDTO
    {
        public int idProducto { get; set; }
        public string codigo { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string? descripcion { get; set; }
        public decimal precioCompra { get; set; }
        public decimal precioVenta { get; set; }
        public int stockActual { get; set; }
        public int stockMinimo { get; set; }
        public int idCategoriaProducto { get; set; }
        public string? nombreCategoria { get; set; }

        public static ProductoDTO FromEntity(Producto p) => new ProductoDTO
        {
            idProducto = p.idProducto,
            codigo = p.codigo,
            nombre = p.nombre,
            descripcion = p.descripcion,
            precioCompra = p.precioCompra,
            precioVenta = p.precioVenta,
            stockActual = p.stockActual,
            stockMinimo = p.stockMinimo,
            idCategoriaProducto = p.idCategoriaProducto,
            nombreCategoria = p.idCategoriaProductoNavigation?.nombre
        };

        public Producto ToEntity()
        {
            return new Producto
            {
                idProducto = this.idProducto,
                codigo = this.codigo,
                nombre = this.nombre,
                descripcion = this.descripcion,
                precioCompra = this.precioCompra,
                precioVenta = this.precioVenta,
                stockActual = this.stockActual,
                stockMinimo = this.stockMinimo,
                idCategoriaProducto = this.idCategoriaProducto
            };
        }
    }
}
