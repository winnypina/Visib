using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Visib.Api.Data;
using Visib.Api.Models;
using Visib.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextTranslationController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public TextTranslationController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextTranslationViewModel>>> Get()
        {
            var textTranslations = await _dbContext.TextTranslations.ToListAsync();
            return new OkObjectResult(textTranslations.Select(textTranslation => new TextTranslationViewModel
            {
                Screen = textTranslation.Screen,
                Key = textTranslation.Key,
                Id = textTranslation.Id,
                Value = textTranslation.Value,
                ValueEnUs = textTranslation.ValueEnUs,
                ValueEs = textTranslation.ValueEs                
            }));

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<TextTranslationViewModel>> GetById(Guid id)
        {
            var data = await _dbContext.TextTranslations.SingleOrDefaultAsync(n => n.Id == id);
            if (data == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(new TextTranslationViewModel
            {
                Screen = data.Screen,
                Key = data.Key,
                Id = data.Id,
                Value = data.Value,
                ValueEnUs = data.ValueEnUs,
                ValueEs = data.ValueEs
            });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TextTranslationViewModel request)
        {
            var textTranslation = _mapper.Map<TextTranslation>(request);
            await _dbContext.TextTranslations.AddAsync(textTranslation);
            await _dbContext.SaveChangesAsync();
            request.Id = textTranslation.Id;
            return new OkObjectResult(request);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] TextTranslationViewModel request)
        {
            var textTranslation = await _dbContext.TextTranslations.SingleOrDefaultAsync(n => n.Id == request.Id);
            if (textTranslation == null)
            {
                return new NotFoundResult();
            }
            textTranslation.Screen = request.Screen;
            textTranslation.Key = request.Key;
            textTranslation.Value = request.Value;
            textTranslation.ValueEnUs = request.ValueEnUs;
            textTranslation.ValueEs = request.ValueEs;
            _dbContext.TextTranslations.Update(textTranslation);
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult(request);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var textTranslation = await _dbContext.TextTranslations.SingleOrDefaultAsync(n => n.Id == id);
            if (textTranslation == null)
            {
                return new NotFoundResult();
            }
            _dbContext.TextTranslations.Remove(textTranslation);
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
    }
}