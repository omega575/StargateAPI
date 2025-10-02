namespace StargateAPI.Business.Requests
{
    public class UpdatePersonRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Title { get; set; }
    }
}
