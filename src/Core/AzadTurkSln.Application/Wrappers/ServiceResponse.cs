namespace AzadTurkSln.Application.Wrappers
{
    public class ServiceResponse<T> : BaseResponse
    {
        public T? Value { get; set; }

        public ServiceResponse(T value)
        {
            Value = value;
            IsSuccess = true;
        }

        public ServiceResponse()
        {
            IsSuccess = true;
        }

        public ServiceResponse(string message)
        {
            IsSuccess = false;
            Message = message;
        }
    }
}
