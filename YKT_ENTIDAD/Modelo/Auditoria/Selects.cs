namespace YKT.ENTIDAD.Modelo.Auditoria
{
    public class Selects
    {
        public string VALUE { get; set; }
        public string DSCRPCN { get; set; }
        public string? ALS1 { get; set; } = null;
        public int? LVLR { get; set; } = null;
        public double? TCC { get; set; } = null;
        public double? TCV { get; set; } = null;

    }

    public class Params
    {
        public int? IDAPLCCN { get; set; } = null;
        public string? DPRMTRO { get; set; } = null;
        public string? OPRMTRO { get; set; } = null;
        public string? VLR1 { get; set; } = null;
        public string? VLR2 { get; set; } = null;
        public string? VLR3 { get; set; } = null;
        public string? VLR4 { get; set; } = null;
        public string? VLR5 { get; set; } = null;
    }
}
