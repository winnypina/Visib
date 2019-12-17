using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visib.Api.Data;
using Visib.Api.Models;
using Visib.Api.ViewModels;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Get()
        {
            return new OkObjectResult(await _dbContext.Categories.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<CategoryViewModel>> GetById(Guid id)
        {
            var data = await _dbContext.Categories.SingleOrDefaultAsync(n => n.Id == id);
            if (data == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(data);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryViewModel request)
        {
            var customer = _mapper.Map<Category>(request);
            await _dbContext.Categories.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            request.Id = customer.Id;
            return new OkObjectResult(request);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] CategoryViewModel request)
        {
            var customer = await _dbContext.Categories.SingleOrDefaultAsync(n => n.Id == request.Id);
            if (customer == null)
            {
                return new NotFoundResult();
            }
            customer.Name = request.Name;
            customer.NameEnUs = request.NameEnUs;
            customer.NameEs = request.NameEs;
            _dbContext.Categories.Update(customer);
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult(request);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var customer = await _dbContext.Categories.SingleOrDefaultAsync(n=>n.Id == id);
            if (customer == null)
            {
                return new NotFoundResult();
            }
            _dbContext.Categories.Remove(customer);
            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }
    }
}