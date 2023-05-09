using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Extension;
public static class PictureLocationExtension
{
    public static PictureDto ToDto(this Picture picture)
    {
        return new PictureDto()
        {
            PictureId = picture.IdPicture,
            Name = picture.Name,
            Description = picture.Description,
            Picture = new(picture.Data, "image/jpeg"),
            LocationId = picture.LocationId ??= null,
            Location = picture.Location?.ToLocationNameDto()
        };
    }
    public static IEnumerable<PictureDto> ToList(this IEnumerable<Picture> pictures) => pictures.Select(p => p.ToDto()).AsEnumerable();

    public static ListPictureDto ToListDto(this Picture picture)
    {
        return new()
        {
            PictureId = picture.IdPicture,
            Name = picture.Name,
            Description = picture.Description,
            Picture = new(picture.Data, "image/jpeg")
        };
    }

    public static IEnumerable<ListPictureDto> ToListListDto(this ICollection<Picture> pictures) => pictures.Select(p => p.ToListDto()).AsEnumerable();
    public static IEnumerable<ListPictureDto> ToListListDto(this IEnumerable<Picture> pictures) => pictures.Select(p => p.ToListDto()).AsEnumerable();
}
