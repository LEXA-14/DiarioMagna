using System;
using System.Collections.Generic;

namespace DiarioMagna.Domain.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorEmail { get; set; } = string.Empty;
        public string AuthorPhone { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? AttachmentPath { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? PlanId { get; set; }
        public Plan Plan { get; set; }

        public ArticleStatus Status { get; set; }

        public string SecretaryUserId { get; set; } = string.Empty;
        public string PublicRelationsUserId { get; set; } = string.Empty;
        public string SystemManagerUserId { get; set; } = string.Empty;

        public string ReferenceText { get; set; } = string.Empty;
        public string UploadedFilePath { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime? SentAt { get; set; }

        public ICollection<ArticleReference> References { get; set; } = new List<ArticleReference>();
        public ICollection<ArticleNoticiero> ArticleNoticieros { get; set; } = new List<ArticleNoticiero>();
    }
}
