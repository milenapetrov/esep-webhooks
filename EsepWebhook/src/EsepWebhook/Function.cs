using Amazon.Lambda.Core;
using Newtonsoft.Json; // for JsonConvert
using System.Text; // for Encoding

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EsepWebhook;

public class Function
{
    
    // / <summary>
    // / A simple function that takes a string and does a ToUpper
    // / </summary>
    // / <param name="input"></param>
    // / <param name="context"></param>
    // / <returns></returns>
    var SLACK_URL = "https://hooks.slack.com/services/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"

    
    public string FunctionHandler(string input, ILambdaContext context)
    {
        dynamic json = JsonConvert.DeserializeObject<dynamic>(input.ToString());
        
        string payload = $"{{'text':'Issue Created: {json.issue.html_url}'}}";
        
        var client = new HttpClient();
        var webRequest = new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("SLACK_URL")) //"SLACK_URL"
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };

        var response = client.Send(webRequest);
        using var reader = new StreamReader(response.Content.ReadAsStream());
            
        return reader.ReadToEnd();
        //return input.ToUpper();
    }
}
