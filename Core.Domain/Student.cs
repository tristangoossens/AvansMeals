namespace Core.Domain
{
    public class Student : EntityBase
    {
        public string Email { get; set; }
        public int StudentNr { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phonenumber { get; set; }
        public City City { get; set; }
        public IEnumerable<Reservation>? Reservations { get; set; }
    }
}