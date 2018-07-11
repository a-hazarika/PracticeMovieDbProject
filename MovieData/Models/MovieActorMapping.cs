namespace MovieData.Models
{
    public class MovieActorMapping
    {
        public int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public int MovieId { get; set; }
        public virtual Actor Actor { get; set; }
        public int ActorId { get; set; }
    }
}
