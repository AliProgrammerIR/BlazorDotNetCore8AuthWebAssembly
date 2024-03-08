namespace BlazorDocManagerDotNet8.Client.Helpers
{
	public class HttpResponseWrapper<T>
	{
		public HttpResponseWrapper(T response, bool success, HttpResponseMessage httpResponseMessage)
		{
			IsSuccess = success;
			Response = response;
			HttpResponseMessage = httpResponseMessage;
		}

		public bool IsSuccess { get; set; }

		public T Response { get; set; }

		public HttpResponseMessage HttpResponseMessage { get; set; }

		public async Task<string> GetBody()
		{
			return await HttpResponseMessage.Content.ReadAsStringAsync();
		}
	}
}
