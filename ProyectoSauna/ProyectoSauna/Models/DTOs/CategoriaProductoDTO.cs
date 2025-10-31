using ProyectoSauna.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSauna.Models.DTOs
{
    public class CategoriaProductoDTO
    {
        public int idCategoriaProducto { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int cantidadProductos { get; set; } // opcional, útil para listados

        public static CategoriaProductoDTO FromEntity(CategoriaProducto c) => new CategoriaProductoDTO
        {
            idCategoriaProducto = c.idCategoriaProducto,
            nombre = c.nombre
        };

        public CategoriaProducto ToEntity() => new CategoriaProducto
        {
            idCategoriaProducto = this.idCategoriaProducto,
            nombre = this.nombre
        };
    }
}