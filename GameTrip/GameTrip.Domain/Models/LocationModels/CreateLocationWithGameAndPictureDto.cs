using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTrip.Domain.Models.LocationModels;

public class CreateLocationWithGameAndPictureDto
{
    public CreateLocationWithGameAndPictureDto(CreateLocationDto location, IEnumerable<Guid> gamesIds)
    {
        Location = location;
        GamesIds = gamesIds;
    }

    public CreateLocationDto Location { get; set; }
    public IEnumerable<Guid> GamesIds { get; set; }
}
