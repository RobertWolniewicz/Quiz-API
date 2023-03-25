namespace Quiz_API.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CorrectAnswersPercent { get; set; }
        public List<Question>? Questions { get; set; } = new List<Question>();
    }
}
