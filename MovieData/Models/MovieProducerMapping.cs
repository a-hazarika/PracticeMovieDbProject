namespace MovieData.Models
{
    public class MovieProducerMapping
    {
        public int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Producer Producer { get; set; }
    }
}
