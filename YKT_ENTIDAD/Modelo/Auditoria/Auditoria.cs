namespace YKT.ENTIDAD.Modelo.Auditoria
{
    public class EntidadAuditoria : IAuditoria
    {
        public int? ID { get; set; } = null;
        public DateTime? FCREACION { get; set; } = null;
        public DateTime? FEDICION { get; set; } = null;
        public string? USUARIO { get; set; } = null;
        public string? USUARIOC { get; set; } = null;
        public string? USUARIOE { get; set; } = null;
        public string? ESTADO { get; set; } = null;
        public int? RN { get; set; } = null;
        public int? TOTALROWS { get; set; } = null;
        public string? DESC { get; set; } = null;
        public int? INIT { get; set; } = null;
        public int? ROWS { get; set; } = null;
        public string? DRAW { get; set; } = null;
    }
    public interface IAuditoria
    {
        public int? ID { get; set; }
        public DateTime? FCREACION { get; set; }
        public DateTime? FEDICION { get; set; }
        public string? USUARIOC { get; set; }
        public string? USUARIOE { get; set; } 
        public string? ESTADO { get; set; } 
        public int? RN { get; set; } 
        public int? TOTALROWS { get; set; } 
        public string? DESC { get; set; }
        public int? INIT { get; set; }
        public int? ROWS { get; set; }
        public string? DRAW { get; set; }
    }
}
