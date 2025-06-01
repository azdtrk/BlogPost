namespace Blog.Domain.Entities;

public class Author : User
{
    public string? About { get; set; }
    
    #region Navigation Properties
    
    public List<BlogPost>? BlogPosts { get; set; }

    public Guid? ProfilePhotoId { get; set; }
    public Image? ProfilePhoto { get; set; }
    
    #endregion
}