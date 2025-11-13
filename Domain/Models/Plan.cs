using System.Collections.Generic;

namespace DiarioMagna.Domain.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
