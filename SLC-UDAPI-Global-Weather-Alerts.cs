using System.Linq;

using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Net.Apps.UserDefinableApis;
using Skyline.DataMiner.Net.Apps.UserDefinableApis.Actions;

namespace SLCUDAPIGlobalWeatherAlerts
{
    /// <summary>
    /// Represents a DataMiner user-defined API.
    /// </summary>
    public class Script
    {
        private readonly int receiverParameter = 1000000;
        private readonly string SkylineUniversalWeatherProtocolName = "Skyline Universal Weather";

        /// <summary>
        /// The API trigger.
        /// </summary>
        /// <param name="engine">Link with SLAutomation process.</param>
        /// <param name="requestData">Holds the API request data.</param>
        /// <returns>An object with the script API output data.</returns>
        [AutomationEntryPoint(AutomationEntryPointType.Types.OnApiTrigger)]
        public ApiTriggerOutput OnApiTrigger(IEngine engine, ApiTriggerInput requestData)
        {
            var method = requestData.RequestMethod;
            var route = requestData.Route;
            var body = requestData.RawBody;

            if (method != RequestMethod.Post)
            {
                return new ApiTriggerOutput
                {
                    ResponseBody = "Only POST requests are supported.",
                    ResponseCode = (int)StatusCode.MethodNotAllowed,
                };
            }

            var element = engine.FindElementsByProtocol(SkylineUniversalWeatherProtocolName).FirstOrDefault();
            if (element == null)
            {
                engine.GenerateInformation($"No element found in DMS using {SkylineUniversalWeatherProtocolName}.");
            }
            else
            {
                element.SetParameter(receiverParameter, body);
            }

            return new ApiTriggerOutput
            {
                ResponseBody = $"Received {method} request for route: '{route}' with body: '{body}'",
                ResponseCode = (int)StatusCode.Ok,
            };
        }
    }
}