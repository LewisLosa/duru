using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duru.Library;
using Duru.UI.Data.Entities.Enums;

namespace Duru.UI.Data.Entities;

[Table ("Role", Schema = "Duru")]
public class Role : CommonBase
{
    private int _roleId;
    private RoleName _roleName;
    private string _description = string.Empty;

    [Required]
    [Key]
    public int RoleId
    {
        get { return _roleId; }
        set
        {
            _roleId = value;
            RaisePropertyChanged("RoleId");
        }
    }
    
    [Required]
    public RoleName RoleName
    {
        get { return _roleName; }
        set
        {
            _roleName = value;
            RaisePropertyChanged("RoleName");
        }
    }
    
    [Required]
    public string Description
    {
        get { return _description; }
        set
        {
            _description = value;
            RaisePropertyChanged("Description");
        }
    }
}