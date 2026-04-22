namespace MyPersonalWebServer
{
    public class DataShare
    {
        public string Nome { get; set; }
        public static DataShare Instance { get; } = new DataShare();
    }
}
