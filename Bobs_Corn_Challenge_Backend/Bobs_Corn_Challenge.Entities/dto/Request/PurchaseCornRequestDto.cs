using System.ComponentModel.DataAnnotations;

namespace Bobs_Corn_Challenge.Entities.dto.Request
{
    public class PurchaseCornRequestDto
    {
        [Required(ErrorMessage = "ClientId is required")]
        [StringLength(50, ErrorMessage = "ClientId cannot exceed 50 characters")]
        [RegularExpression("^[a-zA-Z0-9-_]+$", ErrorMessage = "ClientId can only contain letters, numbers, hyphens and underscores")]
        public string ClientId { get; set; }
    }
}
