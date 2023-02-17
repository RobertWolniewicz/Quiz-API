namespace Quiz_API.Entity
{
    public class CompanyUser: User
    {
        public Company Company { get; set; }
        public int CompanyId { get; set; }
    }
}
