using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FileController : ControllerBase
	{
		private readonly KnightsAndDiamondsContext context;
		public FileController(KnightsAndDiamondsContext context)
		{
			this.context = context;
		}

		[Route("UploadPhoto")]
		[HttpPost, DisableRequestSizeLimit]
		public async Task<IActionResult> Upload()
		{
			try
			{
				var formCollection = await Request.ReadFormAsync();
				var file = formCollection.Files.First();
				/*var folderName = Path.Combine("Resources", "Images");*/
				var folderName = "Resources/Images";

				var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
				if (file.Length > 0)
				{
					var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
					var fullPath = Path.Combine(pathToSave, fileName);
					/*var dbPath = Path.Combine(folderName, fileName);*/
					var dbPath = folderName + '/' + fileName;
					Console.WriteLine("pathara" + dbPath);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					return Ok(new { dbPath });
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
	}
}
