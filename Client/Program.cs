namespace Client
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			const string file = @"search.jpg";
			const string URL = "https://localhost:7071/fileupload/UploadLargeFile";
			await Send(URL, file);
			
			const string file2 = @"search2.jpg";
			const string URL2 = "https://localhost:7071/fileupload/UploadLargeFileCancel";
			await Send(URL2, file2);
		}




		private static async Task Send(string URL, params string[] files)
		{
			MultipartFormDataContent multipartContent = new MultipartFormDataContent();

			foreach (string file in files)
			{
				var streamContent = new StreamContent(File.Open(file, FileMode.Open));
				multipartContent.Add(streamContent, Path.GetFileNameWithoutExtension(file));
			}
			HttpClient httpClient = new HttpClient();
			HttpResponseMessage response = await httpClient.PostAsync(URL, multipartContent,new CancellationToken());

			response.EnsureSuccessStatusCode();

		}


	}
}