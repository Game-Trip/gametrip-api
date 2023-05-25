using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Extension;
public static class PictureLocationExtension
{
    public static PictureDto ToPictureDto(this Picture picture)
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
    public static IEnumerable<PictureDto> ToEnumerable_PictureDto(this IEnumerable<Picture> pictures) => pictures.Select(p => p.ToPictureDto()).AsEnumerable();

    public static ListPictureDto ToList_ListPictureDto(this Picture picture)
    {
        return new()
        {
            PictureId = picture.IdPicture,
            Name = picture.Name,
            Description = picture.Description,
            Picture = new(picture.Data, "image/jpeg")
        };
    }

    public static ICollection<ListPictureDto> ToCollection_ListPictureDto(this ICollection<Picture> pictures) => pictures.Select(p => p.ToList_ListPictureDto()).ToList();
    public static IEnumerable<ListPictureDto> ToEnumerable_ListPictureDto(this ICollection<Picture> pictures) => pictures.Select(p => p.ToList_ListPictureDto()).AsEnumerable();
    public static IEnumerable<ListPictureDto> ToEnumerable_ListPictureDto(this IEnumerable<Picture> pictures) => pictures.Select(p => p.ToList_ListPictureDto()).AsEnumerable();
}
