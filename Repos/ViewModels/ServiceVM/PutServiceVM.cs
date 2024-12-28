namespace Repos.ViewModels.ServiceVM
{
    public class PutServiceVM
    {
        public string Name { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
