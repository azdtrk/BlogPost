using AzadTurkSln.Domain.Common;
using System;

namespace AzadTurkSln.Domain.Entities
{
    public class ExceptionLog : BaseEntity
    {
        public string? Message { get; set; }
        public string? Details { get; set; }
        public string? ErrorType { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
        public string? RequestPath { get; set; }
        public string? UserName { get; set; }
        public string? ClientIp { get; set; }
    }
} 