namespace Domain.ResourceParameters
{
    public class UsersResourceParameters : ResourceParameters
    {
        public UsersResourceParameters()
        {
            OrderBy = "UserName";
        }

        public string Role { get; set; }
    }
}