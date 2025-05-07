namespace Duru.UI.Core.Models;

public class User
{
    public User()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        IsActive = true;
    }

    public int Id
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string Email
    {
        get;
        set;
    }

    public string HashedPassword
    {
        get;
        set;
    }

    public string PhoneNumber
    {
        get;
        set;
    }

    public Role Role
    {
        get;
        set;
    }

    public DateTime CreatedAt
    {
        get;
        set;
    }

    public DateTime UpdatedAt
    {
        get;
        set;
    }

    public bool IsActive
    {
        get;
        set;
    }
}