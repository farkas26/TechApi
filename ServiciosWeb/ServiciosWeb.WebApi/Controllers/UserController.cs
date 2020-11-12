using ServiciosWeb.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using ServiciosWeb.WebApi.Communications.Responses;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using ServiciosWeb.WebApi.Models;
using System.Web;
using Newtonsoft.Json;

namespace ServiciosWeb.WebApi.Controllers
{
    public class UserController : ApiController
    {
        private string urlDomain = "http://localhost:61658";

        [HttpGet]
        [Route("Catalogo/User/ObtenerUsuarios")]
        [ActionName("ObtenerUsuarios")]
        public GenericListResponse<Login> GetAll([FromUri]PagingParameterModel pagingparametermodel)
        {
            GenericListResponse<Login> response;
            BPM_SIEEntities BD = new BPM_SIEEntities();
            var source = BD.Login.ToList();
            int count = source.Count();
            int CurrentPage = pagingparametermodel.pageNumber;
            int PageSize = pagingparametermodel.pageSize;
            int TotalCount = count;
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            var previousPage = CurrentPage > 1 ? "Yes" : "No";
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            try
            {
                response = new GenericListResponse<Login>
                {
                    Status = new ResponseStatus { HttpCode = HttpStatusCode.OK },
                    Items = items
                };
            }
            catch (Exception ex)
            {
                response = new GenericListResponse<Login>
                {
                    Status = new ResponseStatus { HttpCode = HttpStatusCode.InternalServerError, Message = ex.Message },
                    Items = new List<Login>()
                };
            }
            return response;
        }
        [HttpPost]
        [Route("Catalogo/User/Login")]
        [ActionName("Login")]
        public GenericResponse<string> Login(string email, string password)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                using (BPM_SIEEntities db = new BPM_SIEEntities()) {

                    var registro = db.Login.Where(x => x.email == email && x.password == password);
                    if (registro.Count() > 0)
                    {
                        var token = Guid.NewGuid().ToString();
                        Login user = registro.First();
                        user.token = token;
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        mensaje = "Ingreso de forma correcta";                        
                    }
                    else {
                        mensaje = "datos incorrectos";
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
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
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
        [Route("Catalogo/User/Registro")]
        [ActionName("Registro")]
        public GenericResponse<string> Registro(Login user)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                using (BPM_SIEEntities db = new BPM_SIEEntities()) {

                    var registro = db.Login.Where(x => x.username == user.username || x.email == user.email).Any();
                    if (registro)
                    {
                       
                        mensaje = "Usuario ya existe registrado en la base de datos";                        
                    }
                    else {

                        var usuario = new Login();
                        usuario.name = user.name;
                        usuario.number =user.number;
                        usuario.username = user.username;
                        usuario.birthdate = user.birthdate;
                        usuario.email = user.email;
                        usuario.password = user.password;
                        db.Login.Add(usuario);
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
                            mensaje = validationError.ErrorMessage );
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
        [Route("Catalogo/User/Recuperacion")]
        [ActionName("Recuperacion")]
        public GenericResponse<string> RecuperacionCuenta(string emailDestino,string emailOrigen, string password)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                string token = Sha256Hash(Guid.NewGuid().ToString());
                using (BPM_SIEEntities db = new BPM_SIEEntities())
                {

                    var registro = db.Login.Where(x => x.email == emailDestino).FirstOrDefault();
                    if (registro is null)
                    {

                        mensaje = "Usuario no existe en la base de datos";
                    }
                    else
                    {
                        registro.token = token;
                        registro.number = registro.number.TrimEnd();
                        db.Entry(registro).State =  System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        //Se especifica EmailOrigen y Password ya que se necesita un correo gmail para enviarlo
                        SendEmail(emailDestino,token, emailOrigen, password);
                        mensaje = "Se ha enviado un enlace a su correo para poder recuperar la contraseña";
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
        [Route("Catalogo/User/ModificarUsuario")]
        [ActionName("ModificarUsuario")]
        public GenericResponse<string> EditUser(Login user)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                using (BPM_SIEEntities db = new BPM_SIEEntities())
                {

                    var registro = db.Login.Where(x => x.id == user.id).FirstOrDefault<Login>();
                    if (registro is null)
                    {

                        mensaje = "Usuario no existe en la base de datos";
                    }
                    else
                    {

                        registro.name = user.name;
                        registro.number = user.number;
                        registro.password = registro.password;
                        registro.username = user.username;
                        registro.email = user.email;
                        registro.birthdate = user.birthdate;
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
        [Route("Catalogo/User/EliminarUsuario")]
        [ActionName("EliminarUsuario")]
        public GenericResponse<string> DeleteUser(int id)
        {
            var mensaje = "";
            GenericResponse<string> response = new GenericResponse<string>();

            try
            {
                using (BPM_SIEEntities db = new BPM_SIEEntities())
                {

                    var registro = db.Login.Where(x => x.id == id).FirstOrDefault();
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

        static string Sha256Hash(string rawData)
        {
 
            using (SHA256 sha256Hash = SHA256.Create())
            { 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                  
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void SendEmail(string EmailDestino, string token,string CorreoOrigen, string Contra)
        {
            string EmailOrigen = CorreoOrigen;
            string Contraseña = Contra;
            string url = urlDomain + "/Catalogo/Recovery/?token=" + token;
            MailMessage oMailMessage = new MailMessage(EmailOrigen, EmailDestino, "Recuperación de contraseña",
                "<p>Correo para recuperación de contraseña</p><br>" +
                "<a href='" + url + "'>Click para recuperar</a>");

            oMailMessage.IsBodyHtml = true;

            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
            oSmtpClient.EnableSsl = true;
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.Port = 587;
            oSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            oSmtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);

            oSmtpClient.Send(oMailMessage);

            oSmtpClient.Dispose();
        }

    }
}
