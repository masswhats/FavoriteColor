using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MachOneSoftware.FavoriteColor
{
    public class Function
    {
        private const string HelpMessage = "Try asking for a color.";
        private const string StopMessage = "Goodbye color seeker";
        private const string Error_Unknown = "Sorry, something went wrong. Try asking for a color.";

        private readonly string[] coolColors =
        {
            "baby throw up green",
            "dog poop brown",
            "Nebraska Cornhusker scarlet",
            "Nebraska Cornhusker cream",
            "South Dakota Coyotes red",
            "South Dakota Coyotes white",
            "red red wine",
            "purple haze purple",
            "da ba dee blue",
            "white Christmas white",
            "orange but it's all star by smash mouth",
            "green but it's we are number one",
            "blue but every time it's blue it turns green",
            "fuzzy wuzzy brown",
            "carrot parrot",
            "fuzzy caterpillar brown",
            "goth black",
            "house fly black",
            "green dot com",
            "rocket's red glare red",
            "communist red",
            "libertarian gold",
            "purple mountain majesties purple",
            "stupid yellow",
            "delorean gray",
            "corsica red",
            "nomad red",
            "real men wear pink pink",
            "orange is the new black black",
            "razzmatazz",
            "the grass is greener on the other side green",
            "grey with an e.",
            "gray with an a.",
            "as dark as my soul black",
            "kitty cat orange",
            "kitty cat calico",
            "kitty cat black",
            "kitty cat tortoiseshell",
            "this color will offend you, so I won't say it",
            "emoji yellow",
            "simpsons yellow",
            "Amazon orange",
            "I guess that's why they call it the blues blue",

            "red","blue","green","yellow","orange"
        };

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            Shared.RequestId = context.AwsRequestId;
            Shared.Logger = context.Logger;
            var response = new SkillResponse()
            {
                Version = "1.0.0",
                Response = new ResponseBody() { ShouldEndSession = true }
            };
            IOutputSpeech output = null;

            try
            {
                var requestType = input.GetRequestType();
                if (requestType == typeof(LaunchRequest))
                    response.Response.ShouldEndSession = HandleRandomColorIntent(out output);
                else if (requestType == typeof(SessionEndedRequest))
                    output = Shared.GetOutput(StopMessage);
                else if (requestType == typeof(IntentRequest))
                {
                    var intentRequest = (IntentRequest)input.Request;
                    switch (intentRequest.Intent.Name)
                    {
                        case "AMAZON.CancelIntent":
                            output = Shared.GetOutput(StopMessage);
                            break;
                        case "AMAZON.StopIntent":
                            output = Shared.GetOutput(StopMessage);
                            break;
                        case "AMAZON.HelpIntent":
                            response.Response.ShouldEndSession = false;
                            output = Shared.GetOutput(HelpMessage);
                            break;
                        case "RandomColorIntent":
                            response.Response.ShouldEndSession = HandleRandomColorIntent(out output);
                            break;
                        default:
                            response.Response.ShouldEndSession = false;
                            output = Shared.GetOutput(HelpMessage);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Shared.LogError("FunctionHandler", $"input = {input}; context = {context}", ex);
                output = Shared.GetOutput(Error_Unknown);
                response.Response.ShouldEndSession = false;
            }
            finally
            {
                response.Response.OutputSpeech = output;
            }
            return response;
        }

        /// <summary>
        /// Handler for RandomColorIntent. Returns a value indicating whether to end the session. Returns the inner response as an out parameter.
        /// </summary>
        /// <param name="output">Response output speech.</param>
        /// <returns></returns>
        private bool HandleRandomColorIntent(out IOutputSpeech output)
        {
            try
            {
                var r = new Random((int)DateTime.Now.Ticks);
                output = Shared.GetOutput(coolColors[r.Next(coolColors.Length)]);
                return true;
            }
            catch (Exception ex)
            {
                Shared.LogError("HandleRandomColorIntent", "output", ex);
                output = Shared.GetOutput(Error_Unknown);
                return false;
            }
        }
    }
}
