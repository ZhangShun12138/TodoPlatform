using System.ComponentModel.DataAnnotations;

namespace TodoBackend.Requests;

public class VerificationCodRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
}
