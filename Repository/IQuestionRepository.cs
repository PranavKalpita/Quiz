using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Quiz.WebApi.Repository
{
    public interface IQuestionRepository
    {
        Task<string> UploadQuestionsAsync(IFormFile file);
    }
}
