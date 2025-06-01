namespace Blog.Domain.Entities;

public class Reader : User
{
    public string? Preferences { get; set; }
    public bool ReceiveNotifications { get; set; } = true;
    public DateTime LastLoginDate { get; set; }
    
    #region Navigation Properties
    public List<BlogPost>? SavedPosts { get; set; }
    #endregion
}