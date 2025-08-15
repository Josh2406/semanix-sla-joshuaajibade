namespace RenderingService.Domain.Models
{
    public class BaseResponse<T> where T : class, new()
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = default!;
        public List<string> Errors { get; set; } = [];
        public T Data { get; set; } = null!;
    }
}
