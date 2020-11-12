using ServiciosWeb.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using ServiciosWeb.WebApi.Communications.Responses;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Text;
namespace ServiciosWeb.WebApi.Controllers
{
    public class ProductoController : ApiController
    {
        [HttpGet]
        [Route("Catalogo/Producto/ObtenerProductos")]
        [ActionName("ObtenerProductos")]
        public GenericListResponse<Producto> GetAll()
        {
            GenericListResponse<Producto> response;
            BPM_SIEEntities BD = new BPM_SIEEntities();
            try
            {
                response = new GenericListResponse<Producto>
                {
                    Status = new ResponseStatus { HttpCode = HttpStatusCode.OK },
                    Items = BD.Producto.ToList()
                };
            }
            catch (Exception ex)
            {
                response = new GenericListResponse<Producto>
                {
                    Status = new ResponseStatus { HttpCode = HttpStatusCode.InternalServerError, Message = ex.Message },
                    Items = new List<Producto>()
                };
            }
            return response;
        }
        [HttpPost]
        [Route("Catalogo/Producto/Registro")]
        [ActionName("Registro")]
        public GenericResponse<string> Registro(Producto product)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                using (BPM_SIEEntities db = new BPM_SIEEntities())
                {

                    var registro = db.Producto.Where(x => x.SKU == product.SKU).Any();
                    if (registro)
                    {
                        mensaje = "Usuario ya existe registrado en la base de datos";
                    }
                    else
                    {

                        var producto = new Producto();
                        producto.nombre = product.nombre;
                        producto.SKU = product.SKU;
                        producto.cantidad = product.cantidad;
                        producto.descripcion = product.descripcion;
                        producto.precio = product.precio;
                        producto.imagen = product.imagen;
                        db.Producto.Add(producto);
                        db.SaveChanges();
                        mensaje = "Registro completado con exito";
                    }

                }
                {
                    response.Status = new ResponseStatus { HttpCode = HttpStatusCode.OK };
                    response.Item = mensaje;
                };
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Mensaje: {0} Error: {1}",
                            validationError.PropertyName,
                            mensaje = validationError.ErrorMessage);
                    }
                }
                {
                    response.Status = new ResponseStatus { HttpCode = HttpStatusCode.InternalServerError, Message = dbEx.Message };
                    response.Item = mensaje;
                };
            }
            return response;
        }

        [HttpPost]
        [Route("Catalogo/Producto/ModificarProducto")]
        [ActionName("ModificarProducto")]
        public GenericResponse<string> EditProduct(Producto product)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                using (BPM_SIEEntities db = new BPM_SIEEntities())
                {

                    var registro = db.Producto.Where(x => x.id == product.id).FirstOrDefault<Producto>();
                    if (registro is null)
                    {

                        mensaje = "El Producto no existe en la base de datos";
                    }
                    else
                    {

                        registro.nombre = product.nombre;
                        registro.SKU = product.SKU;
                        registro.cantidad = product.cantidad;
                        registro.descripcion = product.descripcion;
                        registro.precio = product.precio;
                        registro.imagen = product.imagen;
                        db.SaveChanges();
                        mensaje = "Registro Actualizado";
                    }

                }
                {
                    response.Status = new ResponseStatus { HttpCode = HttpStatusCode.OK };
                    response.Item = mensaje;
                };
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Mensaje: {0} Error: {1}",
                            validationError.PropertyName,
                            mensaje = validationError.ErrorMessage);
                    }
                }
                {
                    response.Status = new ResponseStatus { HttpCode = HttpStatusCode.InternalServerError, Message = dbEx.Message };
                    response.Item = mensaje;
                };
            }
            return response;
        }

        [HttpPost]
        [Route("Catalogo/Producto/EliminarUsuario")]
        [ActionName("EliminarProducto")]
        public GenericResponse<string> DeleteProduct(int id)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                using (BPM_SIEEntities db = new BPM_SIEEntities())
                {

                    var registro = db.Producto.Where(x => x.id == id).FirstOrDefault();
                    if (registro is null)
                    {

                        mensaje = "Usuario no existe en la base de datos";
                    }
                    else
                    {
                        db.Entry(registro).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        mensaje = "Registro borrado con exito";
                    }

                }
                {
                    response.Status = new ResponseStatus { HttpCode = HttpStatusCode.OK };
                    response.Item = mensaje;
                };
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Mensaje: {0} Error: {1}",
                            validationError.PropertyName,
                            mensaje = validationError.ErrorMessage);
                    }
                }
                {
                    response.Status = new ResponseStatus { HttpCode = HttpStatusCode.InternalServerError, Message = dbEx.Message };
                    response.Item = mensaje;
                };
            }
            return response;
        }

        [HttpPost]
        [Route("Catalogo/Producto/BusquedaProductos")]
        [ActionName("BusquedaProducto")]
        public GenericListResponse<Producto> GetProduct(string nombre , string SKU)
        {
            GenericListResponse<Producto> response;
            BPM_SIEEntities BD = new BPM_SIEEntities();
            try
            {
                var registro = BD.Producto.Where(x => x.nombre.Equals(nombre, StringComparison.CurrentCultureIgnoreCase) || x.SKU.Equals(SKU, StringComparison.CurrentCultureIgnoreCase));
                response = new GenericListResponse<Producto>
                {
                    Status = new ResponseStatus { HttpCode = HttpStatusCode.OK },
                    Items = registro.ToList()
                };
            }
            catch (Exception ex)
            {
                response = new GenericListResponse<Producto>
                {
                    Status = new ResponseStatus { HttpCode = HttpStatusCode.InternalServerError, Message = ex.Message },
                    Items = new List<Producto>()
                };
            }
            return response;
        }
    }
}
