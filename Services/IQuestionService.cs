using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Quiz.WebApi.Services
{
    public interface IQuestionService
    {
        Task<string> UploadQuestionsAsync(IFormFile file);
    }
}
