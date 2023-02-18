namespace Quiz_API.Models
{
    public class DtoQuestion
    {
        public string Text { get; set; }
        public List<string> Categorys { get; set; }
        public List<string> Answers { get; set; }
        public string CorrectAnswer { get; set; }
        public bool EasyAnswer { get; set; }
    }
}
