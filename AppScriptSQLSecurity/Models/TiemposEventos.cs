namespace AppScriptSQLSecurity.Models
{
    public class TiemposEventos
    {
        public int Id { get; set; }
        public DateTime Inicio { get; set; }
        public string Categoria { get; set; }
        public int Duracion { get; set; }
        public string Sala { get; set; }
        public string Empresa { get; set; }
    }
}
