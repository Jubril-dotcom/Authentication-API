namespace Authentication_API
{
    public class User
    {
        internal string Username;

        public String UserName { get; set; } = String.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
