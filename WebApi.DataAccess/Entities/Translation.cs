namespace WebApi.DataAccess.Entities
{
    public class Translation
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Locale { get; set; }
    }
}
