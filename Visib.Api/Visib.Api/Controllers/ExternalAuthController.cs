using System;
using System.Net.Http;
using System.Threading.Tasks;
using Visib.Api.Auth;
using Visib.Api.Data;
using Visib.Api.Helpers;
using Visib.Api.Models;
using Visib.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Visib.Api.Services;
using System.IO;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly ApplicationDbContext _appDbContext;
        private readonly FacebookAuthSettings _fbAuthSettings;
        private readonly IJwtFactory _jwtFactory;
        private readonly IMediaService mediaService;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;

        public ExternalAuthController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
            UserManager<AppUser> userManager, ApplicationDbContext appDbContext, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions, IMediaService mediaService)
        {
            _fbAuthSettings = fbAuthSettingsAccessor.Value;
            _userManager = userManager;
            _appDbContext = appDbContext;
            _jwtFactory = jwtFactory;
            this.mediaService = mediaService;
            _jwtOptions = jwtOptions.Value;
        }

        // POST api/externalauth/facebook
        [HttpPost]
        [Route("facebook")]
        public async Task<IActionResult> Facebook([FromBody] FacebookAuthViewModel model)
        {
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation =
                JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/v3.3/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            // 4. ready to create the local user account (if necessary) and jwt
            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user == null)
            {
                var appUser = new AppUser
                {
                    Name = userInfo.Name,
                    FacebookId = userInfo.Id,
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    //PictureUrl = userInfo.Picture.Data.Url
                };

                var result = await _userManager.CreateAsync(appUser,"teste@123");

                if (!result.Succeeded)
                    return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }

            // generate the jwt for the local user...
            var localUser = await _userManager.FindByNameAsync(userInfo.Email);

            if (localUser == null)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.",
                    ModelState));


            var pictureResponse = await Client.GetByteArrayAsync(
               $"https://graph.facebook.com/v3.3/{userInfo.Id}/picture?type=large");

           
                    
            await mediaService.UploadAsync($"{localUser.Id}.png", new MemoryStream(pictureResponse));
                    
          
            var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id),
                _jwtFactory, localUser.UserName, _jwtOptions,
                new JsonSerializerSettings {Formatting = Formatting.Indented});

            return new OkObjectResult(jwt);
        }
    }
}