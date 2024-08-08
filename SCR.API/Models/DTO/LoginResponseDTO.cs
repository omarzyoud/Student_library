namespace SCR.API.Models.DTO
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        public List<string> Roles { get; set; }
        public int StdId { get; set; }
        public  string name { get; set; }
        public string? EduInst { get; set; }
    }
}