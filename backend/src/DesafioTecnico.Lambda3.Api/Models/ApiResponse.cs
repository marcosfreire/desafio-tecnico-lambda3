namespace DesafioTecnico.Lambda3.Api.Models
{
    public class ApiResponse
    {
        public object Data { get; set; }
        public string[] Errors { get; set; }
        public string ApplicationError { get; set; }

        public bool Success
        {
            get
            {
                return (Errors?.Length ?? 0) == 0 && string.IsNullOrEmpty(ApplicationError);
            }
        }
    }
}
