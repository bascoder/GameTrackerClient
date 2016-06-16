namespace GameTrackerClient.model
{
    /// <summary>
    ///     Game model object
    /// </summary>
    public class Game
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public long? Id { get; set; }

        public override string ToString()
        {
            return Id + " " + Title;
        }

        public override bool Equals(object that)
        {
            if (that == this) return true;
            Game o = that as Game;
            if (o == null) return false;

            return Title?.Equals(o.Title) != false
                   && o.Title?.Equals(Title) != false
                   && Id?.Equals(o.Id) != false
                   && o.Id?.Equals(Id) != false
                   && Icon?.Equals(o.Icon) != false
                   && o.Icon?.Equals(Icon) != false;
        }

        public override int GetHashCode()
        {
            return Title?.GetHashCode() ?? 0 
                + Id?.GetHashCode() ?? 0 
                + Icon?.GetHashCode() ?? 0;
        }
    }
}