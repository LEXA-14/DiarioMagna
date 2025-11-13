namespace DiarioMagna.Domain.Models
{
    public class ArticleReference
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int ReferencedArticleId { get; set; }
        public Article ReferencedArticle { get; set; }
    }
}
