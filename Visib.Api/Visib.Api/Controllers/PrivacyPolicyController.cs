using System.Threading.Tasks;
using Visib.Api.Data;
using Visib.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivacyPolicyController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public PrivacyPolicyController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<PrivacyPolicyViewModel>> Get()
        {
            var useTerm = await _appDbContext.PrivacyPolicies.FirstAsync();
            return new OkObjectResult(new PrivacyPolicyViewModel { Text = useTerm.Value, TextEnUs = useTerm.ValueEnUs, TextEs = useTerm.ValueEs });
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody]PrivacyPolicyViewModel viewModel)
        {
            var useTerm = await _appDbContext.PrivacyPolicies.FirstAsync();
            useTerm.Value = viewModel.Text;
            useTerm.ValueEnUs = viewModel.TextEnUs;
            useTerm.ValueEs = viewModel.TextEs;
            _appDbContext.Update(useTerm);
            await _appDbContext.SaveChangesAsync();
            return new OkObjectResult(true);
        }
    }
}