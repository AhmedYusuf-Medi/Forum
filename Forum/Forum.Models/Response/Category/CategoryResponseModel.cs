namespace Forum.Models.Response.Category
{
    public class CategoryResponseModel
    {
        public CategoryResponseModel(string category, long postsCount, long id)
        {
            this.Category = category;
            this.Posts = postsCount;
            this.Id = id;
        }

        public long Id { get; set; }

        public string Category { get; set; }

        public long Posts { get; set; }
    }
}