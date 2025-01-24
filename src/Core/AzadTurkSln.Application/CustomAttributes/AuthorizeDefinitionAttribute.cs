using AzadTurkSln.Application.Enums;

namespace AzadTurkSln.Application.CustomAttributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        public string Definition { get; set; }
        public ActionType ActionType { get; set; }
    }
}
