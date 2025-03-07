﻿using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public class File : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
    }
}
