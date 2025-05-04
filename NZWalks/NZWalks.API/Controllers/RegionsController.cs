using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionsController(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        //Get All Regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //var regions = new List<Region>
            //{
            //    new Region
            //    {
            //        Id=Guid.NewGuid(),
            //        Name="Auckland Region",
            //        Code="AKL",
            //    },
            //    new Region
            //    {
            //        Id=Guid.NewGuid(),
            //        Name="Wellington Region",
            //        Code="WLG",
            //    },
            //};

            //get Data from database
            var regionsDomain = nZWalksDbContext.Regions.ToList();

            //map domain models to DTO
            var regionsDTO = new List<RegionDTO>();
            foreach (var region in regionsDomain)
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id= region.Id,
                    Code= region.Code,
                    Name= region.Name,
                    RegionImageUrl= region.RegionImageUrl,
                });
            }

            return Ok(regionsDTO);
        }

        //Get single region(get region by id)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{Id:Guid}")]
        public IActionResult GetById([FromRoute] Guid Id)
        {
            //var region = nZWalksDbContext.Regions.Find(Id);
            var regionDomain = nZWalksDbContext.Regions.FirstOrDefault(x => x.Id == Id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionsDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            return Ok(regionsDTO);
        }

    }
}
