namespace ProRanked.Axiom.Sinks;

using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

public class AxiomSink : ILogEventSink
{
    private readonly ITextFormatter _textFormatter;
    private readonly string _dataset;
    private readonly string _token;

    public AxiomSink(ITextFormatter textFormatter, string dataset, string token)
    {
        _textFormatter = textFormatter;
        _dataset = dataset;
        _token = token;
    }

    public void Emit(LogEvent logEvent)
    {
        var logMessage = new JsonLogMessage(logEvent.Level.ToString(), logEvent.RenderMessage());
        LogToAxiom(logMessage, _token).Wait();
    }

    private async Task LogToAxiom(JsonLogMessage logMessage, string token)
    {
        RestClient client = new RestClient();
        List<JsonLogMessage> rawdata = new List<JsonLogMessage> { logMessage };
        string json = JsonConvert.SerializeObject(rawdata);
        RestRequest request = new RestRequest($"https://api.axiom.co/v1/datasets/{_dataset}/ingest", Method.Post);
        request.AddParameter("application/json", json, ParameterType.RequestBody);
        request.AddHeader("Authorization", $"Bearer {token}");

        RestResponse response = await client.ExecuteAsync(request);
        Console.WriteLine(response.Content);
    }
}

public static class LoggerConfigurationAxiomExtensions
{
    public static LoggerConfiguration Axiom(
        this LoggerSinkConfiguration loggerConfiguration,
        string dataset,
        string token,
        ITextFormatter textFormatter = null)
    {
        return loggerConfiguration.Sink(new AxiomSink(textFormatter ?? new Serilog.Formatting.Json.JsonFormatter(), dataset, token));
    }
}

public class JsonLogMessage
{
    public string LogLevel { get; set; }

    public string? Message { get; set; }


    public JsonLogMessage(string logLevel, string message)
    {
        LogLevel = logLevel;
        Message = message;
    }
}