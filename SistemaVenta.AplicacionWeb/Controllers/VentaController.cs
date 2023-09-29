using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;
using System.Security.Claims;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class VentaController : Controller
    {
        private readonly ITipoDocumentoVentaService _tipodocumentoVentaService;
        private readonly IVentaService _VentaService;
        private readonly IMapper _Mapper;
        //private readonly IConverter _converter;

        public VentaController(ITipoDocumentoVentaService tipodocumentoVentaService, IVentaService ventaService, IMapper mapper /*, IConverter converter */)
        {
            _tipodocumentoVentaService = tipodocumentoVentaService;
            _VentaService = ventaService;
            _Mapper = mapper;
            //_converter = converter;
        }

        public IActionResult NuevaVenta()
        {
            return View();
        }

        public IActionResult HistorialVenta()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaTipoDocumentoVenta()
        {
            List<VMTipoDocumentoVenta> vmListaTipoDocumento = _Mapper.Map<List<VMTipoDocumentoVenta>>(await _tipodocumentoVentaService.Lista());

            return StatusCode(StatusCodes.Status200OK, vmListaTipoDocumento);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMProducto> vmListaProduto = _Mapper.Map<List<VMProducto>>(await _VentaService.ObtenerProductos(busqueda));

            return StatusCode(StatusCodes.Status200OK, vmListaProduto);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VMVenta modelo)
        {
            GenericResponse<VMVenta> gResponse = new GenericResponse<VMVenta>();

            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string idUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                modelo.IdUsuario = int.Parse(idUsuario); //temporal hasta imlementar LOGIN

                Venta ventaCreada = await _VentaService.Registrar(_Mapper.Map<Venta>(modelo));
                modelo = _Mapper.Map<VMVenta>(ventaCreada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;


            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            List<VMVenta> vmHistorialVenta = _Mapper.Map<List<VMVenta>>(await _VentaService.Historial(numeroVenta, fechaInicio, fechaFin));

            return StatusCode(StatusCodes.Status200OK, vmHistorialVenta);
        }
        
        /*
        public IActionResult MostrarPDFVenta(string numeroVenta)
        {
            string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFVenta?numeroVenta={numeroVenta}";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page=urlPlantillaVista
                    }
                }
            };

            var archivoPDF = _converter.Convert(pdf);

            return File(archivoPDF, "application/pdf");
        }  */
    }
}
