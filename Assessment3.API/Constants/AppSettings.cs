namespace Assessment3.API.Constants
{
    public static class AppSettings
    {
        public static string ConnectionString { get; set; }

        static AppSettings()
        {
            ConnectionString = Path.Combine(Directory.GetCurrentDirectory(), "mydatabase.db");
        }
    }
}
