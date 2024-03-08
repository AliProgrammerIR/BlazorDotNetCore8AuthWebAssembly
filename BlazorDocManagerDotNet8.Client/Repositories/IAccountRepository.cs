using BlazorDocManagerDotNet8.Client.Helpers;
using BlazorDocManagerDotNet8.Shared.Helpers;

namespace BlazorDocManagerDotNet8.Client.Repositories
{
	public interface IAccountRepository
	{
		Task<UserToken> Login(UserLoginMdl userInfo);
        Task LogoutTotal();
    }

	public class AccountRepository : IAccountRepository
	{
		private readonly IHttpService httpService;
		private readonly string baseUrl = "api/account";

		public AccountRepository(IHttpService httpService)
		{
			this.httpService = httpService;
		}

		public async Task<UserToken> Login(UserLoginMdl userInfo)
		{
			var response = await httpService.Post<UserLoginMdl, UserToken>($"{baseUrl}/Login", userInfo);

			if (!response.IsSuccess)
			{
				throw new ApplicationException(await response.GetBody());
			}

			return response.Response;
		}

        public async Task LogoutTotal()
        {
            try
            {
                var response = await httpService.Post<object>($"{baseUrl}/LogoutTotal", null);
                if (response.IsSuccess)
                {
                    // انجام عملیات مربوط به خروج موفقیت‌آمیز کاربر
                    // مثلاً نمایش پیام خروج موفقیت‌آمیز و ریدایرکت به صفحه‌ای دیگر
                }
                else
                {
                    // انجام عملیات مربوط به خطا در خروج
                    // مثلاً نمایش پیام خطا
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@2" + ex.Message);
                // انجام عملیات مربوط به خطا در پردازش خطا
                // مثلاً نمایش پیام خطا به کاربر
            }
        }
    }
}
