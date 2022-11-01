using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.Cosmos.Sql.Function.Test.Model;
using Sample.Cosmos.Sql.Function.Test.DataAccess;

namespace Sample.Cosmos.Sql.Function.Test;

public class ReportFunction
{
    private readonly IReportRepository reportRepository;

    public ReportFunction(IReportRepository reportRepository)
    {
        this.reportRepository = reportRepository;
    }

    [FunctionName(nameof(ReportFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = "{id?}")] HttpRequest req,
        int? id,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        if (req.Method == HttpMethods.Get)
        {
            if (id is null)
            {
                var results = await reportRepository.GetAllRecordAsync();
                return new OkObjectResult(results);
            }
            else
            {
                var result = await reportRepository.GetByAsync(id.Value);
                return new OkObjectResult(result);
            }
        }
        else if (req.Method == HttpMethods.Post)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Report data = JsonConvert.DeserializeObject<Report>(requestBody);
            await reportRepository.AddAsync(data);
        }
        else if (req.Method == HttpMethods.Put)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Report data = JsonConvert.DeserializeObject<Report>(requestBody);
            var result = reportRepository.Update(data);
            return new OkObjectResult(result);
        }
        return new OkResult();
    }
}
