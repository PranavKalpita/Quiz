using Microsoft.AspNetCore.Http;
using Quiz.WebApi.Repository;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Quiz.WebApi.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _repository;

        public QuestionService(IQuestionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles the uploading of questions from a CSV file by delegating the task to the repository.
        /// </summary>
        /// <param name="file">The CSV file containing questions.</param>
        /// <returns>A string message indicating the result of the operation.</returns>
        public async Task<string> UploadQuestionsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty.");

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Only CSV files are allowed.");

            try
            {
                return await _repository.UploadQuestionsAsync(file);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                throw new Exception($"An error occurred while uploading questions: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates the content of the CSV file to ensure proper structure.
        /// This method is optional and can be extended to include additional checks.
        /// </summary>
        /// <param name="file">The CSV file to validate.</param>
        /// <returns>Boolean indicating whether the file is valid.</returns>
        private bool ValidateCsvFile(IFormFile file)
        {
            // Optional validation logic (e.g., checking column count or headers)
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var headerLine = stream.ReadLine();
                    var columns = headerLine.Split(',');
                    return columns.Length == 7; // Expecting 7 columns: Id, Question, Option1, Option2, Option3, Option4, Result
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
