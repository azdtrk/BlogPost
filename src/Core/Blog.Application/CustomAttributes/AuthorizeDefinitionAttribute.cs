using Blog.Application.Enums;

namespace Blog.Application.CustomAttributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        public string Definition { get; set; }
        public ActionType ActionType { get; set; }
    }
}
