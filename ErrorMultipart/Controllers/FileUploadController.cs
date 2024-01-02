using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;

namespace MetropolisNetwork.Metropolis.Tests.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[DisableFormValueModelBindingAttribute]
	public class FileUploadController : ControllerBase
	{
		// Get the default form options so that we can use them to set the default 
		// limits for request body data.
		private static readonly FormOptions _defaultFormOptions = new FormOptions();
		private readonly long _fileSizeLimit;
		private readonly string[] _permittedExtensions = { ".txt" };
		private readonly ILogger<FileUploadController> _logger;
		private readonly string _targetFilePath;

		public FileUploadController(ILogger<FileUploadController> logger, IConfiguration config)
		{
			_fileSizeLimit = 100;// config.GetValue<long>("FileSizeLimit");

			// To save physical files to a path provided by configuration:
			_targetFilePath = @"D:\VM\Temp";// config.GetValue<string>("StoredFilesPath")!;
			_logger = logger;
		}


		[HttpPost]
		[Route(nameof(UploadLargeFile))]
		[DisableRequestSizeLimitAttribute()]
		[DisableFormValueModelBindingAttribute]
		public async Task<IActionResult> UploadLargeFile()
		{
			var boundary = Request.GetMultipartBoundary();
			var reader = new MultipartReader(boundary, Request.Body);
			var section = await reader.ReadNextSectionAsync();
			while (section != null)
				section = await reader.ReadNextSectionAsync();

			return Ok();
		}

		[HttpPost]
		[Route(nameof(UploadLargeFileCancel))]
		[DisableRequestSizeLimitAttribute()]
		[DisableFormValueModelBindingAttribute]
		public async Task<IActionResult> UploadLargeFileCancel(CancellationToken cancellationToken)
		{
			var boundary = Request.GetMultipartBoundary();
			var reader = new MultipartReader(boundary, Request.Body);
			var section = await reader.ReadNextSectionAsync();
			while (section != null)
				section = await reader.ReadNextSectionAsync();
			return Ok();
		}




	}


	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
	{
		public void OnResourceExecuting(ResourceExecutingContext context)
		{
			var factories = context.ValueProviderFactories;
			factories.RemoveType<FormValueProviderFactory>();
			factories.RemoveType<JQueryFormValueProviderFactory>();
		}

		public void OnResourceExecuted(ResourceExecutedContext context)
		{
		}
	}
}
