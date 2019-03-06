namespace CookBook.Domain.ResultDtos
{
    public class ErrorDto
    {
        public int Code { get; set; }

        public string[] Errors { get; set; }

        public ErrorDto(int code, params string[] errors)
        {
            Code = code;
            Errors = errors;
        }
    }
}
