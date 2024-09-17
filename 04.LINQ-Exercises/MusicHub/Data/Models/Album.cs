using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Album
{
    public Album()
    {
        Songs = new HashSet<Song>();
    }
    [Key]
    public int Id { get; set; }

    [MaxLength(Validations.AlbumNameLength)]
    public string Name { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public decimal Price => Songs.Sum(s => s.Price);

    public int? ProducerId { get; set; }

    public virtual Producer? Producer { get; set; }


    public virtual ICollection<Song> Songs { get; set; }

}
