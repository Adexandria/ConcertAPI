
namespace Concert.Domain.Entities
{
    public class Mail
    {
        public string  To { get; set; }
        public string Subject { get; set; } = "Verify your email";
        public string Text { get; set; }
    }
}
