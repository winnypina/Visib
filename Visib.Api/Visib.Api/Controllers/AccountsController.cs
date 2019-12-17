using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Visib.Api.Data;
using Visib.Api.Helpers;
using Visib.Api.Models;
using Visib.Api.Services;
using Visib.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly ISmsService _smsService;
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor, IMediaService mediaService, ISmsService smsService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _mediaService = mediaService;
            _smsService = smsService;
            _emailSender = emailSender;
        }

        // POST api/accounts
        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAdmin()
        {
            var users = await _userManager.Users.Where(n => n.Type == UserType.Admin && n.Email != "admin@visib.com").ToListAsync();
            return new OkObjectResult(users.Select(n => new AdminAccountViewModel
            {
                Email = n.Email,
                Id = n.Id
            }));
        }

        [HttpGet]
        [Route("admin/{id}")]
        public async Task<IActionResult> GetAdminById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return new NotFoundResult();

            return new OkObjectResult(new AdminAccountViewModel
            {
                Email = user.Email,
                Id = user.Id
            });
        }

        // POST api/accounts
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null) return new NotFoundResult();

            var relations = await _appDbContext.Relations.Where(n => n.FollowedId == user.Id || n.FollowerId == user.Id).ToListAsync();

            var userVm = new AccountViewModel
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                DesiredLanguage = user.DesiredLanguage,
                Type = user.Type,
                HasSetPassword = user.HasSetPassword,
                FacebookId = user.FacebookId,
                MobilePhone = user.MobilePhone,
                Name = user.Name,
                Follows = relations.Count(n => n.FollowerId == user.Id),
                Followers = relations.Count(n => n.FollowedId == user.Id),
                About = user.About,
                Address = user.Address,
                AddressAddon = user.AddressAddon,
                BusinessPhone = user.BusinessPhone,
                Cep = user.Cep,
                City = user.City,
                AddressNumber = user.AddressNumber,
                Country = user.Country,
                State = user.State,
                Website = user.Website,
                Email = user.Email
            };

            return new OkObjectResult(userVm);
        }

        [HttpGet]
        [Authorize]
        [Route("followers/{userId}")]
        public async Task<IActionResult> GetFollowers(string userId)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(n => n.Id == userId);
            var followerIds = _appDbContext.Relations.Where(n => n.FollowedId == userId).Select(n => n.FollowerId).Distinct().ToList();

            return new OkObjectResult(_userManager.Users.Where(n => followerIds.Contains(n.Id)).Select(us =>
                new AccountViewModel
                {
                    Id = us.Id,
                    Name = us.Name
                }
            ));
        }

        [HttpPost]
        [Authorize]
        [Route("follow/{userid}")]
        public async Task<IActionResult> Follow(string userId)
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var user = await _userManager.Users.SingleAsync(n => n.Id == id);
            var userToFollow = await _userManager.Users.SingleAsync(n => n.Id == userId);
            if (userToFollow == null) return new NotFoundResult();


            var isFollowing = await _appDbContext.Relations.SingleOrDefaultAsync(n => n.FollowerId == id && n.FollowedId == userToFollow.Id);
            if (isFollowing == null)
            {
                await _appDbContext.Relations.AddAsync(new Relation { FollowerId = id, FollowedId = userToFollow.Id });
            }
            else
            {
                _appDbContext.Relations.Remove(isFollowing);
            }

            await _appDbContext.SaveChangesAsync();

            return new OkResult();
        }

        [HttpGet]
        [Authorize]
        [Route("follows/{userId}")]
        public async Task<IActionResult> GetFollows(string userId)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(n => n.Id == userId);
            var followedIds = _appDbContext.Relations.Where(n => n.FollowerId == userId).Select(n => n.FollowedId).Distinct();

            return new OkObjectResult(_userManager.Users.Where(n => followedIds.Contains(n.Id)).Select(us =>
                new AccountViewModel
                {
                    Id = us.Id,
                    Name = us.Name
                }
            ));
        }


        [HttpGet]
        [Authorize]
        [Route("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(n=>n.Id ==userId);

            var relations = await _appDbContext.Relations.Where(n => n.FollowedId == user.Id || n.FollowerId == user.Id).ToListAsync();

            if (user == null) return new NotFoundResult();

            var userVm = new AccountViewModel
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                DesiredLanguage = user.DesiredLanguage,
                HasSetPassword = user.HasSetPassword,
                Type = user.Type,
                FacebookId = user.FacebookId,
                MobilePhone = user.MobilePhone,
                Name = user.Name,
                Follows = relations.Count(n => n.FollowerId == user.Id),
                Followers = relations.Count(n => n.FollowedId == user.Id),
                About = user.About,
                Address = user.Address,
                AddressAddon = user.AddressAddon,
                BusinessPhone = user.BusinessPhone,
                Cep = user.Cep,
                City = user.City,
                AddressNumber = user.AddressNumber,
                Country = user.Country,
                State = user.State,
                Website = user.Website,
                Email = user.Email
            };

            return new OkObjectResult(userVm);
        }

        [HttpPost]
        [Route("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] CodeValidationViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName.Replace("(", "").Replace(")", "")
                .Replace("+", "").Replace("-", ""));
            if (user == null) return NotFound();
            if (user.Code != model.Code) return BadRequest();
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost]
        [Route("resend")]
        public async Task<IActionResult> ResendCode([FromBody] CodeValidationViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName.Replace("(", "").Replace(")", "")
                .Replace("+", "").Replace("-", ""));
            if (user == null) return NotFound();
            if (user.Email == "no@email.com")
                await _smsService.Send(
                    user.UserName.Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", ""),
                    $"Seu código de verificação para o Visib: {user.Code.ToString().PadLeft(4, '0')}");
            else
                await _emailSender.Send(user.Email, "Confirmação de cadastro - Visib",
                    $"Seu código de verificação para o Visib: {user.Code.ToString().PadLeft(4, '0')}");
            return Ok();
        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login.Replace("(", "").Replace(")", "")
                .Replace("+", "").Replace("-", ""));
            if (user == null) return NotFound();

            var newPassword = GenerateRandomPassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (user.Email == "no@email.com")
                await _smsService.Send(
                    user.UserName.Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", ""),
                    $"Sua nova senha para o Visib: {newPassword}");
            else
                await _emailSender.Send(user.Email, "Alteração de senha - Visib",
                    $"Sua nova senha para o Visib: {newPassword}");
            return Ok();
        }

        [HttpPost]
        [Route("admin")]
        public async Task<IActionResult> PostAdmin([FromBody] AdminAccountViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userIdentity = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Email,
                Type = UserType.Admin
            };


            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            var userVm = new AccountViewModel
            {
                Id = userIdentity.Id,
                BirthDate = userIdentity.BirthDate,
                DesiredLanguage = userIdentity.DesiredLanguage,
                Type = userIdentity.Type,
                HasSetPassword = userIdentity.HasSetPassword,
                FacebookId = userIdentity.FacebookId,
                MobilePhone = userIdentity.MobilePhone,
                Name = userIdentity.Name,
                About = userIdentity.About,
                Address = userIdentity.Address,
                AddressAddon = userIdentity.AddressAddon,
                AddressNumber = userIdentity.AddressNumber,
                BusinessPhone = userIdentity.BusinessPhone,
                Cep = userIdentity.Cep,
                City = userIdentity.City,
                Country = userIdentity.Country,
                State = userIdentity.State,
                Website = userIdentity.Website,
                Email = userIdentity.Email
            };

            return new OkObjectResult(userVm);
        }

        [HttpDelete]
        [Route("admin/{id}")]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var random = new Random();
            var code = random.Next(0, 9999);

            var userIdentity = _mapper.Map<AppUser>(model);
            userIdentity.Code = code;
            userIdentity.HasSetPassword = true;
            if (string.IsNullOrEmpty(model.Email) || model.Email == "no@email.com")
            {
                userIdentity.UserName = model.MobilePhone.Replace("(", "").Replace(")", "").Replace("+", "")
                    .Replace("-", "");
                await _smsService.Send(userIdentity.UserName,
                    $"Seu código de verificação para o Visib: {code.ToString().PadLeft(4, '0')}");
            }
            else
            {
                userIdentity.UserName = model.Email;
                await _emailSender.Send(userIdentity.Email, "Confirmação de cadastro - Visib",
                    $"Seu código de verificação para o Visib: {code.ToString().PadLeft(4, '0')}");
            }

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded)
            {
                var existingUser = await _userManager.FindByNameAsync(userIdentity.UserName);
                if(existingUser != null)
                {
                    existingUser.Code = code;
                    var existingUserVm = new AccountViewModel
                    {
                        Id = userIdentity.Id,
                        BirthDate = userIdentity.BirthDate,
                        HasSetPassword = userIdentity.HasSetPassword,
                        DesiredLanguage = userIdentity.DesiredLanguage,
                        Type = userIdentity.Type,
                        FacebookId = userIdentity.FacebookId,
                        MobilePhone = userIdentity.MobilePhone,
                        Name = userIdentity.Name,
                        About = userIdentity.About,
                        Address = userIdentity.Address,
                        AddressAddon = userIdentity.AddressAddon,
                        AddressNumber = userIdentity.AddressNumber,
                        BusinessPhone = userIdentity.BusinessPhone,
                        Cep = userIdentity.Cep,
                        Code = userIdentity.Code,
                        City = userIdentity.City,
                        Country = userIdentity.Country,
                        State = userIdentity.State,
                        Website = userIdentity.Website,
                        Email = userIdentity.Email
                    };

                    await _userManager.UpdateAsync(existingUser);

                    return new OkObjectResult(existingUserVm);
                }
            }

            var userVm = new AccountViewModel
            {
                Id = userIdentity.Id,
                BirthDate = userIdentity.BirthDate,
                HasSetPassword = userIdentity.HasSetPassword,
                DesiredLanguage = userIdentity.DesiredLanguage,
                Type = userIdentity.Type,
                FacebookId = userIdentity.FacebookId,
                MobilePhone = userIdentity.MobilePhone,
                Name = userIdentity.Name,
                About = userIdentity.About,
                Address = userIdentity.Address,
                AddressAddon = userIdentity.AddressAddon,
                AddressNumber = userIdentity.AddressNumber,
                BusinessPhone = userIdentity.BusinessPhone,
                Cep = userIdentity.Cep,
                City = userIdentity.City,
                Country = userIdentity.Country,
                State = userIdentity.State,
                Website = userIdentity.Website,
                Email = userIdentity.Email
            };

            return new OkObjectResult(userVm);
        }

        // PUT api/accounts
        [HttpPost]
        [Authorize]
        [Route("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(!user.HasSetPassword)
            {
                model.OldPassword = "teste@123";
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                user.HasSetPassword = true;
                await _userManager.UpdateAsync(user);
            }
            return new OkResult();
        }


        // PUT api/accounts
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] AccountViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            user.BirthDate = model.BirthDate;
            user.MobilePhone = model.MobilePhone;
            user.AddressNumber = model.AddressNumber;
            user.BusinessPhone = model.BusinessPhone;
            user.Cep = model.Cep;
            user.City = model.City;
            user.Country = model.Country;
            user.Email = model.Email;
            user.State = model.State;
            user.Website = model.Website;
            user.About = model.About;
            user.Address = model.Address;
            user.AddressAddon = model.AddressAddon;
            user.DesiredLanguage = model.DesiredLanguage;
            user.Name = model.Name;

            if (!string.IsNullOrEmpty(user.MobilePhone))
                user.UserName = user.MobilePhone.Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return new OkObjectResult(user);
        }
    }
}