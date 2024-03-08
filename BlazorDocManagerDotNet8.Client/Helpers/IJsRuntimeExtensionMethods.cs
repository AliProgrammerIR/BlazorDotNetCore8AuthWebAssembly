using Microsoft.JSInterop;

namespace BlazorDocManagerDotNet8.Client.Helpers
{
	public static class IJsRuntimeExtensionMethods
	{
		public static async ValueTask<bool> Confirm(this IJSRuntime js, string message)
		{
			await js.InvokeVoidAsync("console.log", "example", "toplearn blazor course");
			return await js.InvokeAsync<bool>("confirm", message);
		}

		public static async ValueTask ShowMessage(this IJSRuntime js, string message)
		{
			await js.InvokeVoidAsync("show_message", message);
		}

		public static ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content) =>
			js.InvokeAsync<object>("localStorage.setItem", key, content);

		public static ValueTask<string> GetFromLocalStorage(this IJSRuntime js, string key) =>
			js.InvokeAsync<string>("localStorage.getItem", key);

		public static ValueTask<object> RemoveItem(this IJSRuntime js, string key) =>
			js.InvokeAsync<object>("localStorage.removeItem", key);
	}
}
