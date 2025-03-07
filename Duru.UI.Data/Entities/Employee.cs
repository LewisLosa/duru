using System.ComponentModel.DataAnnotations.Schema;
using Duru.Library;

namespace Duru.UI.Data.Entities;

[Table("Employee", Schema = "Duru")]
public class Employee : CommonBase
{
    private int _employeeId;
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _phoneNumber;
    private string _address;
    private string _password;
    private string _role;
}