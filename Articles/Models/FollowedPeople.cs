namespace Articles.Models
{
    public class FollowedPeople
    {
        public int ObserveId { get; set; }
        public Person Observer { get; set; }
        public int TargetId { get; set; }
        public Person Target { get; set; }
    }
}