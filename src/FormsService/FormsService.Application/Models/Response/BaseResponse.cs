namespace FormsService.Application.Models.Response
{
    public class BaseResponse<T> where T : class, new()
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = default!;
        public List<string> Errors { get; set; } = [];
        public T Data { get; set; } = null!;
    }

    public class BaseResponse
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = default!;
        public List<string> Errors { get; set; } = [];
    }
}
