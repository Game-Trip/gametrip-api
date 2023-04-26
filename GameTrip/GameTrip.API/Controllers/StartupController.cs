using GameTrip.API.Models;
using GameTrip.Domain.Interfaces;
using GameTrip.Domain.Models;
using GameTrip.Domain.Models.Email;
using GameTrip.Domain.Models.Email.Template;
using GameTrip.Platform.IPlatform;
using GameTrip.Provider.IProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class StartupController : ControllerBase
    {
        private readonly IStartupPlatform _startupPlatform;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailPlatform _mailPlatform;
        private readonly IEmailProvider _emailProvider;

        public StartupController(IStartupPlatform startupPlatform, IUnitOfWork unitOfWork, IMailPlatform mailPlatform, IEmailProvider emailProvider)
        {
            _startupPlatform = startupPlatform;
            _unitOfWork = unitOfWork;
            _mailPlatform = mailPlatform;
            _emailProvider = emailProvider;
        }

        [HttpGet]
        [Route("ping")]
        public ActionResult<TestModel> Ping()
        {
            return new TestModel()
            {
                Test = _startupPlatform.ping()
            };
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("sendMail")]
        public async Task<IActionResult> SendMailAsync()
        {
            try
            {
                string emailTemplateText = _emailProvider.GetTemplate(TemplatePath.Register)!;
                if (emailTemplateText== null) throw new FileNotFoundException();

                emailTemplateText = emailTemplateText.Replace("{0}", "Dercraker");
                emailTemplateText = emailTemplateText.Replace("{1}", "Dercraker");

                MailDTO mailDTO = new()
                {
                    Name = "Dercraker",
                    Email = "antoine.capitain@gmail.com",
                    Subject = "Bienvenue sur GameTrip",
                    Body = emailTemplateText
                };

                await _mailPlatform.SendMailAsync(mailDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
                throw;
            }
        }

        [HttpGet]
        [Route("locations")]
        public ActionResult<List<LocationDTO>> GetLocations()
        {
            List<LocationDTO> locations = new()
            {
                new LocationDTO("Tour Eiffel", "Monument emblématique de Paris, France", 48.8588443, 2.2943506),
                new LocationDTO("Statue de la Liberté", "Symbole de liberté à New York, États-Unis", 40.689247, -74.044502),
                new LocationDTO("Colisée", "Ancien amphithéâtre romain à Rome, Italie", 41.8902102, 12.4922315),
                new LocationDTO("Grande Muraille de Chine", "Merveille architecturale à Pékin, Chine", 40.431908, 116.570374),
                new LocationDTO("Opéra de Sydney", "Icone moderne de l'Australie à Sydney, Australie", -33.8568, 151.2153),
                new LocationDTO("Machu Picchu", "Site archéologique incas au Pérou", -13.1631, -72.5450),
                new LocationDTO("Pyramides de Gizeh", "Anciens monuments égyptiens près du Caire, Égypte", 29.9792, 31.1342),
                new LocationDTO("Cristo Redentor", "Statue du Christ rédempteur à Rio de Janeiro, Brésil", -22.9519, -43.2105),
                new LocationDTO("Acropole d'Athènes", "Site archéologique en Grèce", 37.9715, 23.7262),
                new LocationDTO("Mont Saint-Michel", "Monastère fortifié en France", 48.6361, -1.5113),
                new LocationDTO("Taj Mahal", "Mausolée à Agra, Inde", 27.1750, 78.04),
                new LocationDTO("Mur de Berlin", "Ancienne frontière divisant Berlin, Allemagne", 52.5200, 13.4050),
                new LocationDTO("Tour de Londres", "Château historique à Londres, Royaume-Uni", 51.5081, -0.0761),
                new LocationDTO("Place Rouge", "Place emblématique de Moscou, Russie", 55.7539, 37.6208),
                new LocationDTO("Château de Versailles", "Palais royal à Versailles, France", 48.8048, 2.1204),
                new LocationDTO("Central Park", "Parc emblématique de New York, États-Unis", 40.7851, -73.9683),
                new LocationDTO("Tour CN", "Gratte-ciel emblématique de Toronto, Canada", 43.6516, -79.3832),
                new LocationDTO("Angkor Wat", "Temple au Cambodge", 13.4125, 103.8660),
                new LocationDTO("Mont Everest", "Plus haute montagne du monde à la frontière du Népal et du Tibet", 27.9881, 86.9253)
            };

            return locations;
        }
    }
}