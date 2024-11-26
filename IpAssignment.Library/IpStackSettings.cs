using System.ComponentModel.DataAnnotations;

namespace IpAssignment.Library;

public class IpStackSettings
{
    [Required (AllowEmptyStrings = false)]
    public string ApiKey { get; set; } = string.Empty;
}