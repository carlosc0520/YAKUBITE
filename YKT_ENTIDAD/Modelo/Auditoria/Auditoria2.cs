namespace YKT.ENTIDAD.Modelo.Auditoria
{
    public class Auditoria2 : IAuditoria2
    {
        public int ID { get; set; }
        public DateTime? FCRCN { get; set; }
        public string? UCRCN { get; set; } 

    }
    public interface IAuditoria2
    {
        public int ID { get; set; }
        public DateTime? FCRCN { get; set; }
        public string UCRCN { get; set; }
    }
}
