using System;

namespace DiarioMagna.Domain.Models;
public class ArticleNoticiero
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public Article Article { get; set; }
    public int NoticieroId { get; set; }
    public Noticiero Noticiero { get; set; }
    public bool IsSent { get; set; } = false;
    public DateTime? SentAt { get; set; }
}
