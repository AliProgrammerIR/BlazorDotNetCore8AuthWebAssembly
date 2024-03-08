using BlazorDocManagerDotNet8.Server.AppDbContext;
using BlazorDocManagerDotNet8.Server.Dto.Entities;
using BlazorDocManagerDotNet8.Shared.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace BlazorDocManagerDotNet8.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration iconfiguration;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            IConfiguration _iconfiguration,
            ApplicationDbContext _applicationDbContext,
            ILogger<AccountController> _logger)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            iconfiguration = _iconfiguration;
            applicationDbContext = _applicationDbContext;
            logger = _logger;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserRegMdl model)
        {
            try
            {
                String ErrStr = "";
                var res1 = await userManager.FindByEmailAsync(model.Email);
                //var res2 = await applicationDbContext.Users.AnyAsync(n => n.PhoneNumber == model.PhoneNumber);

                if (res1 != null)
                {
                    ErrStr += "آدرس ایمیل وارد شده تکراری می باشد.";
                }

                //if (res2 == true)
                //{
                //    ErrStr += "تلفن همراه وارد شده تکراری می باشد.";
                //}

                if (ErrStr != "")
                {
                    UserToken ut = new UserToken();
                    ut.Responser.ResponsState = ResponserState.Fail;
                    ut.Responser.StrMessage = ErrStr;
                    return ut;
                }

                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    EmailConfirmed = true,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    #region اضافه کردن دسترسی ورود به سیستم
                    var resCreated = await userManager.FindByEmailAsync(model.Email);
                    //if (resCreated != null)
                    //{
                    //    TblUsersPermissions t = new();
                    //    t.PUserID = resCreated.Id;
                    //    t.PageId = 6;
                    //    t.PageNames = null;
                    //    t.PageCrcPerm = "----";

                    //    await applicationDbContext.AddAsync(t);
                    //    await applicationDbContext.SaveChangesAsync();
                    //}
                    #endregion
                    UserLoginMdl ln = new UserLoginMdl()
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    return await BuildToken(ln);
                }
                else
                {
                    UserToken ut = new UserToken();
                    ut.Responser.ResponsState = ResponserState.Fail;
                    ut.Responser.StrMessage = "خطا در هنگام ایجاد کاربر جدید" + result.Errors;
                    return ut;
                    //return BadRequest("Username Or Password is invalid");
                }
            }
            catch (Exception ex)
            {
                UserToken ut = new UserToken();
                ut.Responser.ResponsState = ResponserState.Fail;
                ut.Responser.StrMessage = "خطا در هنگام ایجاد کاربر جدید";
                ut.Responser.StrErrorMessage = ex.Message;
                return ut;
                //return BadRequest(ex.Message);
            }
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserLoginMdl userInfo)
        {
            UserToken ut = new UserToken();
            //حذف توکن دو مرحله ای در صورتی که کش شده باشد
            await signInManager.ForgetTwoFactorClientAsync();
            try
            {
                //var acc6Leve /*دسترسی ورود به سیستم*/ = await applicationDbContext.TblUsersPermissions
                //    .Include(m => m.AppUsers)
                //    .SingleAsync(n => n.AppUsers.Email == userInfo.Email && n.PageId == 6);

                var result = await signInManager
                    .PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);

                if (result.Succeeded)
                {//Logining ok.
                    ut = await BuildToken(userInfo);
                    ut.Responser.ResponsState = ResponserState.Successful;
                    ut.Responser.StrMessage = "ورود با موفقیت انجام شد . . .";
                    return ut;
                }
                else
                {
                    if (result.RequiresTwoFactor)
                    {
                        var user = await userManager.FindByEmailAsync(userInfo.Email);
                        var tokenMain = await userManager.GenerateTwoFactorTokenAsync(user, "Email");//.GetValidTwoFactorProvidersAsync(user);
                        ut.Responser.StrMessage = tokenMain.ToString();
                        ut.Responser.ResponsState = ResponserState.TwoVerification;
                        Console.WriteLine(ut.Responser.StrMessage);
                        return ut;
                    }
                    else
                    {
                        ut.Responser.ResponsState = ResponserState.Fail;
                        ut.Responser.StrMessage = "مشخصات کاربری صحیح نمی باشند . . . .";
                        //return BadRequest("Login Failed");
                        return ut;
                    }
                }
            }
            catch (Exception ex)
            {
                ut.Responser.ResponsState = ResponserState.Fail;
                ut.Responser.StrMessage = "ورود انجام نشد. لطفا مشخصات کاربری یا سطح دسترسی را بررسی فرمائید . . .";
                return ut;
            }
        }


        private async Task<UserToken> BuildToken(UserLoginMdl userInfo)
        {
            var appUser = await userManager.FindByEmailAsync(userInfo.Email);

            var claims = new List<Claim>(){
        new Claim(ClaimTypes.Name, userInfo.Email),
        new Claim(ClaimTypes.Email, userInfo.Email),
        new Claim(ClaimTypes.GivenName, $"{appUser.FirstName} {appUser.LastName}"),
        new Claim("MyClaim", "My Claim Value")};

            await userManager.AddClaimsAsync(appUser, claims);

            // تولید کلید‌های RSA
            using var rsa = RSA.Create();
            var rsaParams = rsa.ExportParameters(true);
            var key = new RsaSecurityKey(rsaParams);

            // ایجاد امضای RSA
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var expireDate = DateTime.Now.AddDays(30);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expireDate,
                signingCredentials: creds
            );

            return new UserToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireDate = expireDate,
                Responser = new PubResponser { StrMessage = "ورود موفقیت آمیز", ResponsState = ResponserState.Successful }
            };
        }


        [HttpPost("LogoutTotal")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                //الزامی - در غیر اینصورت برای کاربراین لاگین دو مرحله ای در صورت کش شدن به مشکل بر می خوریم
                await signInManager.ForgetTwoFactorClientAsync();
                await signInManager.SignOutAsync(); // ساین اوت کردن کاربر
                await HttpContext.SignOutAsync();
                HttpContext.User =
                    new GenericPrincipal(new GenericIdentity(string.Empty), null);
                return Ok(); // برگرداندن پاسخ 200 OK به مشتری
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError); // برگرداندن پاسخ 500 Internal Server Error به مشتری در صورت بروز خطا
            }
        }



        //private UserToken BuildToken(UserLoginMdl userInfo)
        //{
        //    var claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Name, userInfo.Email),
        //        new Claim(ClaimTypes.Email, userInfo.Email),
        //        new Claim("MyClaim", "My Claim Value")
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfiguration["jwt:key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expireDate = DateTime.Now.AddDays(30);

        //    JwtSecurityToken token = new JwtSecurityToken(
        //        issuer: null,
        //        audience: null,
        //        claims: claims,
        //        expires: expireDate,
        //        signingCredentials: creds
        //    );

        //    return new UserToken
        //    {
        //        Token = new JwtSecurityTokenHandler().WriteToken(token),
        //        ExpireDate = expireDate,
        //        Responser = new PubResponser { StrMessage = "ورود موفقیت آمیز", ResponsState = ResponserState.Successful }
        //    };
        //}
    }
}