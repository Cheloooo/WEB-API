namespace WEB_DOMAIN.Entity;

public class BaseEntity
{
  //ito yung inherit lahat ng entity na iccreate or default columns
    
    public bool IsActive { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}