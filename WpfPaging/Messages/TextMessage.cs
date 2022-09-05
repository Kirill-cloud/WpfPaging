namespace WpfPaging.Messages
{
    public class TextMessage : IMessage
    {
        public TextMessage(string text, int id)
        {
            Text = text;
            Id = id;
        }
        public TextMessage(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
        public int? Id { get; set; }
    }
}
