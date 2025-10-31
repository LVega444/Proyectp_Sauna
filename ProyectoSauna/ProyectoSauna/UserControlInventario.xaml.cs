using Microsoft.Extensions.DependencyInjection;
using ProyectoSauna.Data;
using ProyectoSauna.Models.Entities;
using ProyectoSauna.Repositories.Interfaces;
using ProyectoSauna.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoSauna
{
    public partial class UserControlInventario : UserControl
    {
        // ======== CAMPOS ========
        private readonly IProductoRepository _productoRepo;
        private readonly CategoriaProductoRepository _categoriaRepo;

        private List<Producto> listaProductos = new();
        private List<CategoriaProducto> listaCategorias = new();

        // ======== CONSTRUCTOR ========
        public UserControlInventario()
        {
            InitializeComponent();

            _productoRepo = App.AppHost!.Services.GetRequiredService<IProductoRepository>();
            _categoriaRepo = App.AppHost!.Services.GetRequiredService<CategoriaProductoRepository>();

            Loaded += async (s, e) => await InicializarAsync();
        }

        // ======== MÉTODO PRINCIPAL ========
        private async Task InicializarAsync()
        {
            try
            {
                await CargarCategoriasAsync();
                await Task.Delay(200);
                await CargarProductosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ======== CARGAR CATEGORÍAS ========
        private async Task CargarCategoriasAsync()
        {
            try
            {
                listaCategorias = (await _categoriaRepo.GetAllAsync()).ToList();

                var listaParaFiltro = new List<CategoriaProducto>
                {
                    new CategoriaProducto { idCategoriaProducto = 0, nombre = "Todas las categorías" }
                };
                listaParaFiltro.AddRange(listaCategorias);

                cbCategorias.ItemsSource = listaParaFiltro;
                cbCategorias.DisplayMemberPath = "nombre";
                cbCategorias.SelectedValuePath = "idCategoriaProducto";
                cbCategorias.SelectedIndex = 0;

                cbCategoriaFormulario.ItemsSource = listaCategorias;
                cbCategoriaFormulario.DisplayMemberPath = "nombre";
                cbCategoriaFormulario.SelectedValuePath = "idCategoriaProducto";
                cbCategoriaFormulario.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ======== CARGAR PRODUCTOS ========
        private async Task CargarProductosAsync()
        {
            try
            {
                listaProductos = (await _productoRepo.GetAllAsync()).ToList();
                dataGridProductos.ItemsSource = listaProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ======== FILTRAR PRODUCTOS POR CATEGORÍA ========
        private async void cbCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCategorias.SelectedItem is CategoriaProducto categoriaSeleccionada)
            {
                if (categoriaSeleccionada.idCategoriaProducto == 0)
                {
                    await CargarProductosAsync();
                }
                else
                {
                    var filtrado = await _productoRepo.ObtenerPorCategoriaAsync(categoriaSeleccionada.idCategoriaProducto);
                    dataGridProductos.ItemsSource = filtrado.ToList();
                }
            }
        }

        // ======== SELECCIONAR PRODUCTO (LLENA FORMULARIO) ========
        private void dataGridProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridProductos.SelectedItem is Producto producto)
            {
                txtCodigo.Text = producto.codigo;
                txtNombre.Text = producto.nombre;
                txtDescripcion.Text = producto.descripcion;
                txtPrecioCompra.Text = producto.precioCompra.ToString();
                txtPrecioVenta.Text = producto.precioVenta.ToString();
                txtStockActual.Text = producto.stockActual.ToString();
                txtStockMinimo.Text = producto.stockMinimo.ToString();
                cbCategoriaFormulario.SelectedValue = producto.idCategoriaProducto;
            }
        }

        // ======== AGREGAR PRODUCTO ========
        private async void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecioCompra.Text) ||
                    string.IsNullOrWhiteSpace(txtStockActual.Text) ||
                    string.IsNullOrWhiteSpace(txtStockMinimo.Text) ||
                    cbCategoriaFormulario.SelectedValue == null)
                {
                    MessageBox.Show("Complete todos los campos obligatorios.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool precioVentaLleno = !string.IsNullOrWhiteSpace(txtPrecioVenta.Text);


                var nuevo = new Producto
                {
                    codigo = txtCodigo.Text,
                    nombre = txtNombre.Text,
                    descripcion = txtDescripcion.Text,
                    precioCompra = decimal.Parse(txtPrecioCompra.Text),
                    precioVenta = precioVentaLleno ? decimal.Parse(txtPrecioVenta.Text) : 0,
                    stockActual = int.Parse(txtStockActual.Text),
                    stockMinimo = int.Parse(txtStockMinimo.Text),
                    idCategoriaProducto = (int)cbCategoriaFormulario.SelectedValue
                };

                await _productoRepo.AddAsync(nuevo);
                MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                await CargarProductosAsync();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ======== MODIFICAR PRODUCTO ========
        private async void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProductos.SelectedItem is not Producto producto)
                return;

            try
            {
                producto.codigo = txtCodigo.Text;
                producto.nombre = txtNombre.Text;
                producto.descripcion = txtDescripcion.Text;
                producto.precioCompra = decimal.Parse(txtPrecioCompra.Text);
                producto.precioVenta = decimal.Parse(txtPrecioVenta.Text);
                producto.stockActual = int.Parse(txtStockActual.Text);
                producto.stockMinimo = int.Parse(txtStockMinimo.Text);
                producto.idCategoriaProducto = (int)cbCategoriaFormulario.SelectedValue;

                await _productoRepo.UpdateAsync(producto);
                MessageBox.Show("Producto modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                await CargarProductosAsync();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ======== ELIMINAR PRODUCTO ========
        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridProductos.SelectedItem is not Producto producto)
                return;

            if (MessageBox.Show("¿Desea eliminar este producto?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    await _productoRepo.DeleteAsync(producto.idProducto);
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    await CargarProductosAsync();
                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ======== BUSCAR PRODUCTO ========
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = txtBuscar.Text.ToLower();
            var filtrado = listaProductos
                .Where(p => p.nombre.ToLower().Contains(filtro) ||
                            p.codigo.ToLower().Contains(filtro))
                .ToList();

            dataGridProductos.ItemsSource = filtrado;
        }

        // ======== LIMPIAR FORMULARIO ========
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecioCompra.Clear();
            txtPrecioVenta.Clear();
            txtStockActual.Clear();
            txtStockMinimo.Clear();
            cbCategoriaFormulario.SelectedIndex = -1;
        }
    }
}
