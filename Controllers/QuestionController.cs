using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Quiz.WebApi.Services;
using System;
using System.Threading.Tasks;

namespace Quiz.WebApi.Controllers
{
	[ApiController]
	[Route("admin/questions")]
	public class QuestionController : ControllerBase
	{
		private readonly IQuestionService _questionService;

		public QuestionController(IQuestionService questionService)
		{
			_questionService = questionService;
		}

		/// <summary>
		/// Endpoint to upload a CSV file containing quiz questions.
		/// </summary>
		/// <param name="file">The CSV file to upload.</param>
		/// <returns>HTTP 200 with success message or HTTP 400/500 with error details.</returns>
		[HttpPost("upload")]
		public async Task<IActionResult> UploadQuestions([FromForm] IFormFile file)
		{
			try
			{
				// Validate the file
				if (file == null || file.Length == 0)
					return BadRequest("No file uploaded or file is empty.");

				if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
					return BadRequest("Invalid file type. Only CSV files are allowed.");

				// Delegate to the service
				var result = await _questionService.UploadQuestionsAsync(file);

				// Return success response
				return Ok(new { Message = result });
			}
			catch (Exception ex)
			{
				// Handle unexpected errors
				return StatusCode(500, new
				{
					Error = "An error occurred while uploading the file.",
					Details = ex.Message
				});
			}
		}
	}
}
