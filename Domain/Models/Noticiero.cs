using System.Collections.Generic;

namespace DiarioMagna.Domain.Models
{
    public class Noticiero
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<ArticleNoticiero> ArticleNoticieros { get; set; } = new List<ArticleNoticiero>();
    }
}
