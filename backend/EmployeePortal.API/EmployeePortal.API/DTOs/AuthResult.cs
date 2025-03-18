namespace EmployeePortal.API.DTOs
{
    public class AuthResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        public AuthResult() { }

        public AuthResult(bool success, string message, string token)
        {
            Success = success;
            Message = message;
            Token = token;
        }
    }
}
