using System;

namespace CookBook.Domain.ResultDtos.CommentDtos
{
    public class CommentDetailsDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? EditDate { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }
    }
}
