using GetChoicesFunction.DiHelpers;
using GetChoicesFunction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GetChoicesFunction
{
    public static class Function
    {
        [FunctionName("Function")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "adventures/{adventureid}/choices/{choiceid?}")] HttpRequest req,
            string adventureId,
            string choiceId,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function processed a request.");

            try
            {
                using (var servicesProvider = DiHelper.ConfigureServices())
                {
                    //TODO: Select container by adventure Id
                    var itemService = servicesProvider.GetService<IItemService>();

                    string status = req.Query["status"];
                    var items = (status?.ToUpper() == "FIRST") ?
                        await itemService.GetRoot() :
                        await itemService.Get(choiceId);

                    log.LogInformation("HTTP trigger function has been completed.");

                    return items != null
                        ? (ActionResult)new OkObjectResult(items)
                        : new NotFoundResult();
                }
            }
            catch (System.Exception ex)
            {
                log.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}
