namespace WebApi.Models
{
    public class CciEePp
    {
        public int CciEePpId { get; set; }
        public string? Level1 { get; set; }
        public string? Level2 { get; set; }
        public string? Level3 { get; set; }
        public string? Level4 { get; set; }
        public string? TermEe { get; set; }
        public string? DefinitionEe { get; set; }
        public string? TermEn { get; set; }
        public string? DefinitionEn { get; set; }
        public List<WorkPackage>? WorkPackages { get; set; }
    }
}
