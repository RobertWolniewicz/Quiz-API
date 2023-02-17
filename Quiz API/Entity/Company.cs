namespace Quiz_API.Entity
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int solvedQuizs { get; set; }
        public List<CompanyUser> Users { get; set; }

    }
}
