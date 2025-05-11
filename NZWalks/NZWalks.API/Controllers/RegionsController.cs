using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        private readonly IRegionRepositoy regionRepositoy;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext nZWalksDbContext, IRegionRepositoy regionRepositoy, IMapper mapper)
        {
            this.nZWalksDbContext = nZWalksDbContext;
            this.regionRepositoy = regionRepositoy;
            this.mapper = mapper;
        }

        //Get All Regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepositoy.GetAllAsync();

            //map domain models to DTO
            //var regionsDTO = new List<RegionDTO>();
            //foreach (var region in regionsDomain)
            //{
            //    regionsDTO.Add(new RegionDTO()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl,
            //    });
            //}
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }

        //Get single region(get region by id)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            //var region = nZWalksDbContext.Regions.Find(Id);
            //var regionDomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            var regionDomain = await regionRepositoy.GetByIdAsync(Id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //var regionsDTO = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl,
            //};

            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        //POST To create New Region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map or convert DTO to Domain model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDTO.Code,
            //    Name = addRegionRequestDTO.Name,
            //    RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            //};

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);
            //Use Domain Model to Create Region
            regionDomainModel = await regionRepositoy.CreateAsync(regionDomainModel);

            //map domain model to DTO
            //var regionDto = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        //Update region
        //PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //map DTO to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequestDTO.Code,
            //    Name = updateRegionRequestDTO.Name,
            //    RegionImageUrl = updateRegionRequestDTO.RegionImageUrl
            //};
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

            //Check if region exists
            regionDomainModel = await regionRepositoy.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain Model To DTO
            //var RegionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};


            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }

        //Delete Region
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepositoy.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //map domain model to DTO
            //var RegionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }
    }
}
