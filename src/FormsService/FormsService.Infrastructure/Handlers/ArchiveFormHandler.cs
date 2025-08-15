namespace FormsService.Infrastructure.Handlers
{
    public class ArchiveFormHandler(FormsDbContext context, IHttpContextAccessor accessor) : 
            IRequestHandler<ArchiveFormCommand, BaseResponse>
    {
        private readonly FormsDbContext _context = context;
        private readonly IHttpContextAccessor _contextAccessor = accessor;

        public async Task<BaseResponse> Handle(ArchiveFormCommand request, CancellationToken cancellationToken)
        {
            BaseResponse response;
            try
            {
                var tenantId = _contextAccessor.HttpContext.Items[HeaderKeys.TenantId]!.ToString()!;
                var form = await _context.Forms.FirstOrDefaultAsync(x => x.Id == request.FormId && x.TenantId == tenantId, cancellationToken);
                if (form == null)
                {
                    return new BaseResponse
                    {
                        Errors = [],
                        ResponseCode = 404,
                        ResponseMessage = "Form Not Found."
                    };
                }

                if (form.State == FormState.Archived)
                {
                    return new BaseResponse
                    {
                        Errors = [],
                        ResponseCode = 400,
                        ResponseMessage = "The form is already archived."
                    };
                }

                form.State = FormState.Archived;
                form.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                response = new BaseResponse { Errors = [], ResponseCode = 200, ResponseMessage = "Form archived successfully" };
                Log.Information("Form {FormId} archived successfully", request.FormId); 
            }
            catch (Exception ex) 
            {
                Log.Error("Error archiving form {FormId}: {Message}", request.FormId, ex.Message);
                response = new BaseResponse
                {
                    Errors = [ex.Message],
                    ResponseCode = 500,
                    ResponseMessage = "An error occurred while processing your request. Please contact sysadmin."
                };
            }
            return response;
        }
    }
}
