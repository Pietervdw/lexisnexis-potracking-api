namespace POTracking.Domain.Dtos
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}