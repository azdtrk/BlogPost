namespace AzadTurkSln.Application.Wrappers
{
    public class BaseResponse
    {
        public int Id { get; set; }

        public String Message { get; set; }

        public bool IsSuccess { get; set; } = true;
    }
}
