using Microsoft.AspNetCore.Mvc;

namespace POTracking.Api
{
    public class ValidationError : ProblemDetails
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
