using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;
public class Producer
{
    public Producer()
    {
        Albums = new HashSet<Album>();
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(Validations.ProducerNameLength)]
    public string Name { get; set; } = null!;

    public string? Pseudonym { get; set; }

    [MaxLength(Validations.PhoneNumberLength)]
    public string? PhoneNumber { get; set; }

    public virtual ICollection<Album> Albums { get; set; }



}
