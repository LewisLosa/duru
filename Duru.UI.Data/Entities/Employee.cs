using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duru.Library;

namespace Duru.UI.Data.Entities;

[Table("Employee", Schema = "Duru")]
public class Employee : CommonBase
{
    private int _employeeId;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _address = string.Empty;
    private DateTime _birthDate = DateTime.Now;
    private DateTime _hireDate = DateTime.Now;
    private DateTime _createdAt = DateTime.Now;
    private int _roleId;
    private bool _isLoggedIn = false;
    private DateTime _lastLoginDate = DateTime.Now;
    private DateTime _lastLogoutDate = DateTime.Now;

    [Required]
    [Key]
    public int EmployeeId
    {
        get { return _employeeId; }
        set
        {
            _employeeId = value;
            RaisePropertyChanged("EmployeeId");
        }
    }

    [Required]
    public string Email
    {
        get { return _email; }
        set
        {
            _email = value;
            RaisePropertyChanged("Email");
        }
    }

    [Required]
    public string Password
    {
        get { return _password; }
        set
        {
            _password = value;
            RaisePropertyChanged("Password");
        }
    }

    [Required]
    public string FirstName
    {
        get { return _firstName; }
        set
        {
            _firstName = value;
            RaisePropertyChanged("FirstName");
        }
    }

    [Required]
    public string LastName
    {
        get { return _lastName; }
        set
        {
            _lastName = value;
            RaisePropertyChanged("LastName");
        }
    }

    [Required]
    public string PhoneNumber
    {
        get { return _phoneNumber; }
        set
        {
            _phoneNumber = value;
            RaisePropertyChanged("PhoneNumber");
        }
    }

    [Required]
    public string Address
    {
        get { return _address; }
        set
        {
            _address = value;
            RaisePropertyChanged("Address");
        }
    }

    [Required]
    public DateTime BirthDate
    {
        get { return _birthDate; }
        set
        {
            _birthDate = value;
            RaisePropertyChanged("BirthDate");
        }
    }

    [Required]
    public DateTime HireDate
    {
        get { return _hireDate; }
        set
        {
            _hireDate = value;
            RaisePropertyChanged("HireDate");
        }
    }

    [NotMapped]
    public DateTime CreatedAt
    {
        get { return _createdAt; }
        set
        {
            _createdAt = value;
            RaisePropertyChanged("CreatedAt");
        }
    }

    [Required]
    public int RoleId
    {
        get { return _roleId; }
        set
        {
            _roleId = value;
            RaisePropertyChanged("RoleId");
        }
    }
    
    [ForeignKey("RoleId")]
    public virtual Role? Role{ get; set; }

    [NotMapped]
    public bool IsLoggedIn
    {
        get { return _isLoggedIn; }
        set
        {
            _isLoggedIn = value;
            RaisePropertyChanged("IsLoggedIn");
        }
    }

    [NotMapped]
    public DateTime LastLoginDate
    {
        get { return _lastLoginDate; }
        set
        {
            _lastLoginDate = value;
            RaisePropertyChanged("LastLoginDate");
        }
    }

    [NotMapped]
    public DateTime LastLogoutDate
    {
        get { return _lastLogoutDate; }
        set
        {
            _lastLogoutDate = value;
            RaisePropertyChanged("LastLogoutDate");
        }
    }
}