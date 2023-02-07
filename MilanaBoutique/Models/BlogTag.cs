namespace MilanaBoutique.Models
{
    public class BlogTag
    {
        public int Id { get; set; }
        public Blog Blog { get; set; }
        public int BlogId { get; set; }

        public Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
