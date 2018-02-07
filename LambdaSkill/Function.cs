using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using LambdaSkill.Models;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaSkill
{
    public class Function
    {
        private static HttpClient _httpClient;

        public Function()
        {
            _httpClient = new HttpClient();
        }
        public const string INVOCATION_NAME = "Volleyball England";

        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            try
            {
                var requestType = input.GetRequestType();
                if (requestType == typeof(LaunchRequest))
                {
                    return MakeSkillResponse("Welcome. You can ask me about volleyball club, or league in England.", false);
                }
                else if (requestType == typeof(IntentRequest))
                {
                    var intentRequest = input.Request as IntentRequest;
                    bool? isMen = null;

                    switch (intentRequest.Intent.Name)
                    {
                        case "GetClubIntent":
                            var clubRequested = intentRequest?.Intent?.Slots["Club"].Value;
                            isMen = GetIsMenValue(intentRequest?.Intent?.Slots["IsMen"].Value);
                            if (clubRequested != null && isMen != null)
                            {
                                var response = await SearchClub(clubRequested, isMen.Value, context);
                                return MakeSkillResponse(response, true);
                            }
                            break;
                        case "GetLeagueIntent":
                            var leagueRequested = intentRequest?.Intent?.Slots["League"].Value;
                            isMen = GetIsMenValue(intentRequest?.Intent?.Slots["IsMen"].Value);
                            if (leagueRequested != null && isMen != null)
                            {
                                var response = await SearchLeague(leagueRequested, isMen.Value, context);
                                return MakeSkillResponse(response, true);
                            }
                            break;
                        case "AMAZON.CancelIntent":
                        case "AMAZON.StopIntent":
                            return MakeSkillResponse("Goodbye!", true);
                        case "AMAZON.HelpIntent":
                            return MakeSkillResponse("You can say find a club named Polonia men, or show me Division 2 North women table. After you say club or league name follow it by specifying wether it is men's or women's", false);
                        default:
                            break;
                    }
                    return MakeSkillResponse("I'm sorry, but I didn't understand the question. Please try again", false);
                }
                else
                {
                    return MakeSkillResponse(
                            $"I don't know how to handle this request. Please say something like: find a club named Blyth Valley men, or show me Super 8 women table.",
                            true);
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"\nException: {ex.Message}");
                context.Logger.LogLine($"\nStack Trace: {ex.StackTrace}");
                return MakeSkillResponse(
                            $"I am sorry but error occurred within the application. We are working on fixing it.",
                            true);
            }
        }

        private SkillResponse MakeSkillResponse(string outputSpeech,
            bool shouldEndSession,
            string repromptText = "Ask me about volleyball club, or league in England. To exit, say, exit.")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

            if (repromptText != null)
            {
                response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromptText } };
            }

            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };
            return skillResponse;
        }


        private async Task<string> SearchClub(string searchPhrase, bool isMen, ILambdaContext context)
        {
            List<Club> clubs = new List<Club>();
            var uri = new Uri($"http://englandvolleyball.com/api/search/club?searchPhrase={searchPhrase}&isMen={isMen}");
            var response = await _httpClient.GetStringAsync(uri);
            clubs = JsonConvert.DeserializeObject<List<Club>>(response);

            return BuildSearchClubResponse(clubs);
        }

        private string BuildSearchClubResponse(List<Club> clubs)
        {
            string result = string.Empty;
            if (clubs.Count == 0)
            {
                result = "I didn't find any clubs matching this description.";
            }
            else if (clubs.Count > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("I found more than one club matching your description. ");
                sb.Append("The one playing in the highest league is  ");

                sb.Append(clubs[0].Name);
                sb.Append(" which is currently ");
                sb.Append(clubs[0].LeaguePosition + "th");
                sb.Append(" in ");
                sb.Append(clubs[0].League);
                sb.Append(" with ");
                sb.Append(clubs[0].LeaguePoints);
                sb.Append(" points. ");
                sb.Append("They have played ");
                sb.Append(clubs[0].GamesPlayed);
                sb.Append(" games. Won ");
                sb.Append(clubs[0].GamesWon);
                sb.Append(" and lost ");
                sb.Append(clubs[0].GamesLost);

                sb.Append(". Other clubs are: ");

                for (int i = 1; i < clubs.Count; i++)
                {
                    sb.Append(clubs[i].Name);
                    if ((i + 1) != clubs.Count)
                    {
                        sb.Append(", ");
                    }
                }

                sb.Append(".");
                result = sb.ToString();
            }

            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(clubs[0].Name);
                sb.Append(" is currently ");
                sb.Append(clubs[0].LeaguePosition + "th");
                sb.Append(" in ");
                sb.Append(clubs[0].League);
                sb.Append(" with ");
                sb.Append(clubs[0].LeaguePoints);
                sb.Append(" points. ");
                sb.Append("They have played ");
                sb.Append(clubs[0].GamesPlayed);
                sb.Append(" games. Won ");
                sb.Append(clubs[0].GamesWon);
                sb.Append(" and lost ");
                sb.Append(clubs[0].GamesLost);
                result = sb.ToString();
            }

            return result;
        }

        private async Task<string> SearchLeague(string searchPhrase, bool isMen, ILambdaContext context)
        {
            List<League> leagues = new List<League>();
            var uri = new Uri($"http://englandvolleyball.com/api/search/league?searchPhrase={searchPhrase}&isMen={isMen}");

            var response = await _httpClient.GetStringAsync(uri);
            leagues = JsonConvert.DeserializeObject<List<League>>(response);

            return BuildSearchLeagueResponse(leagues);
        }

        private string BuildSearchLeagueResponse(List<League> leagues)
        {
            string result = string.Empty;
            if (leagues.Count == 0)
            {
                result = "I didn't find any leagues matching this description.";
            }
            else if (leagues.Count > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("I found more than one league matching your description: ");

                for (int i = 0; i < leagues.Count; i++)
                {
                    sb.Append(leagues[i].Name);
                    if ((i + 1) < leagues.Count)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(".");
                result = sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Here is the table for ");
                sb.Append(leagues[0].Name);
                sb.Append(". ");

                foreach (Top3Club t3c in leagues[0].Top3Clubs)
                {
                    sb.Append(t3c.LeaguePosition + "th");
                    sb.Append(" is ");
                    sb.Append(t3c.Name);
                    sb.Append(" with ");
                    sb.Append(t3c.LeaguePoints);
                    sb.Append(" points. ");
                    sb.Append("They have played ");
                    sb.Append(t3c.GamesPlayed);
                    sb.Append(" games. Won ");
                    sb.Append(t3c.GamesWon);
                    sb.Append(" and lost ");
                    sb.Append(t3c.GamesLost);
                    sb.Append(". ");
                }

                foreach (ClubSnippet oc in leagues[0].OtherClubs)
                {
                    sb.Append(oc.LeaguePosition + "th");
                    sb.Append(" is ");
                    sb.Append(oc.Name);
                    sb.Append(" with ");
                    sb.Append(oc.LeaguePoints);
                    sb.Append(" points. ");
                }

                result = sb.ToString();
            }

            return result;
        }

        private bool? GetIsMenValue(string slotValue)
        {
            if (slotValue == "men")
            {
                return true;
            }
            else if (slotValue == "women")
            {
                return false;
            }
            return null;
        }

    }
}
