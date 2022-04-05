
namespace UV_Backend.Entities
{
    public class ApiResponse<T>
    {
        public string message { get; set; }
        public T data { get; set; }
    }
}
