using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.PictureModels;

namespace GameTrip.Domain.Extension;
public static class PictureLocationExtension
{
    public static PictureDto ToDto(this Picture picture)
    {
        return new PictureDto()
        {
            Name = picture.Name,
            Description = picture.Description,
            Picture = new(picture.Data, "image/jpeg")
        };
    }

    public static IEnumerable<ListPictureLocationDto> ToListPictureLocationDto(this IEnumerable<Picture> pictures)
    {
        List<ListPictureLocationDto> listPictureLocationDtos = new();

        IEnumerable<IGrouping<Guid?, Picture>> picturesGroupByLocation = pictures.GroupBy(p => p.LocationId);
        foreach (IGrouping<Guid?, Picture> group in picturesGroupByLocation)
        {
            ListPictureLocationDto listPictureLocationDto = new()
            {
                LocationId = (Guid)group.Key!,
                Location = group.First().Location.ToLocationNameDto(),
                Pictures = group.Select(g => g.ToDto())
            };

            listPictureLocationDtos.Add(listPictureLocationDto);
        }

        return listPictureLocationDtos.AsEnumerable();
    }
}
