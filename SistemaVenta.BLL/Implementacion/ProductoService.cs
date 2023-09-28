using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Implementacion
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _repositorio;
        private readonly IFireBaseService _firebaseService;

        public ProductoService(IGenericRepository<Producto> repositorio, IFireBaseService firebaseService)
        {
            _repositorio = repositorio;
            _firebaseService = firebaseService;
        }

        public async Task<List<Producto>> Lista()
        {
            IQueryable<Producto> query = await _repositorio.Consultar();
            return query.Include(c => c.IdCategoriaNavigation).ToList();
        }
        public async Task<Producto> Crear(Producto entidad, Stream imagen = null, string NombreImagen = "")
        {
            Producto productoExiste = await _repositorio.Obtener(p => p.CodigoBarra == entidad.CodigoBarra);

            if (productoExiste != null)
            {
                throw new TaskCanceledException("El codigo de barra ya existe");
            }

            try
            {
                entidad.NombreImagen = NombreImagen;
                if (imagen != null)
                {
                    string urlImagen = await _firebaseService.SubirStorage(imagen, "carpeta_producto", NombreImagen);
                    entidad.UrlImagen = urlImagen;
                }

                Producto producto_creado = await _repositorio.Crear(entidad);

                if (producto_creado.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el producto");
                }

                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == producto_creado.IdProducto);

                producto_creado = query.Include(c => c.IdCategoriaNavigation).First();

                return producto_creado;

            }
            catch
            {
                throw;
            }

        }

        public async Task<Producto> Editar(Producto entidad, Stream imagen = null, string NombreImagen = "")
        {
            Producto productoExiste = await _repositorio.Obtener(p => p.CodigoBarra == entidad.CodigoBarra && p.IdProducto != entidad.IdProducto);

            if (productoExiste != null)
            {
                throw new TaskCanceledException("El codigo de barra ya existe");
            }

            try
            {
                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == entidad.IdProducto);

                Producto productoEditar = query.First();

                productoEditar.CodigoBarra = entidad.CodigoBarra;
                productoEditar.Marca = entidad.Marca;
                productoEditar.Descripcion = entidad.Descripcion;
                productoEditar.IdCategoria = entidad.IdCategoria;
                productoEditar.Stock = entidad.Stock;
                productoEditar.Precio = entidad.Precio;
                productoEditar.EsActivo = entidad.EsActivo;

                if (productoEditar.NombreImagen == "")
                {
                    productoEditar.NombreImagen = NombreImagen;
                }

                if (imagen != null)
                {
                    string urlImagen = await _firebaseService.SubirStorage(imagen, "carpeta_producto", productoEditar.NombreImagen);
                    productoEditar.UrlImagen = urlImagen;
                }

                bool respuesta = await _repositorio.Editar(productoEditar);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar el producto");
                }

                Producto productoEditado = query.Include(c => c.IdCategoriaNavigation).First();

                return productoEditado;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                Producto producto_encontrado = await _repositorio.Obtener(p => p.IdProducto == idProducto);

                if (producto_encontrado == null)
                {
                    throw new TaskCanceledException("El producto no existe");
                }

                string nombreImagen = producto_encontrado.NombreImagen;

                bool respuesta = await _repositorio.Eliminar(producto_encontrado);

                if (respuesta)
                {
                    await _firebaseService.EliminarStorage("carpeta_producto", nombreImagen);
                }

                return respuesta;
            }
            catch
            {
                throw;
            }
        }


    }
}
