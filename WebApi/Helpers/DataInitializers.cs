using System.Reflection;
using System.Text.Json;
using WebApi.Models;

namespace WebApi.Helpers
{
    /// <summary>
    /// Helper method to add initial data.
    /// </summary>
    public static class DataInitializers
    {
        /// <summary>
        /// Add classificators if none exist in database.
        /// </summary>
        /// <param name="context"></param>
        public static async Task AddClassificators(AppDbContext context)
        {
            if (context.CciEePps?.FirstOrDefault() == null)
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SampleData/classificators.json");
                var lines = File.ReadAllText(path);
                var classificators = JsonSerializer.Deserialize<List<CciEePp>>(lines);
                if (classificators != null)
                {
                    await context.CciEePps!.AddRangeAsync(classificators);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
