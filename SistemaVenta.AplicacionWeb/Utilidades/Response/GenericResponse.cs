namespace SistemaVenta.AplicacionWeb.Utilidades.Response
{
    //esta clase lo vamos a utilizar como respuestas a todas las solicitudes que se hagan a nuestro sitio web
    public class GenericResponse<TObject>
    {
        public bool Estado { get; set; }
        public string? Mensaje { get; set; }
        public TObject? Objeto { get; set; }
        public List<TObject>? ListaObjeto { get; set; }

    }
}
