using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using WebApplicationStoreProcedure.WebApi.DatabaseConfigure;

namespace Quiz.WebApi.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public QuestionRepository(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        public async Task<string> UploadQuestionsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0 || !file.FileName.EndsWith(".csv"))
                throw new ArgumentException("Invalid file. Only CSV files are allowed.");

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var line = await stream.ReadLineAsync(); // Skip header row
                while ((line = await stream.ReadLineAsync()) != null)
                {
                    var values = line.Split(',');

                    if (values.Length != 7)
                        continue; // Skip invalid rows

                    // Parse values
                    var id = int.Parse(values[0]);
                    var question = values[1];
                    var option1 = values[2];
                    var option2 = values[3];
                    var option3 = values[4];
                    var option4 = values[5];
                    var result = values[6];

                    // Insert into database
                    using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
                    {
                        using (var command = new SqlCommand("sp_InsertQuestion", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Id", id);
                            command.Parameters.AddWithValue("@Question", question);
                            command.Parameters.AddWithValue("@Option1", option1);
                            command.Parameters.AddWithValue("@Option2", option2);
                            command.Parameters.AddWithValue("@Option3", option3);
                            command.Parameters.AddWithValue("@Option4", option4);
                            command.Parameters.AddWithValue("@Result", result);

                            connection.Open();
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }

            return "Uploaded successfully";
        }
    }
}
