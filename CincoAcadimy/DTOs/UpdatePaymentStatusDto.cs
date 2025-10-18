using System.ComponentModel.DataAnnotations;

public class UpdatePaymentStatusDto
{
    [Required]
    public int PaymentId { get; set; }

    [Required]
    public string Status { get; set; }
}