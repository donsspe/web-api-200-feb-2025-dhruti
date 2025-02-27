using System.Text.Json.Serialization;


namespace VipIssue.Api;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(VipIssueResponseModel))]

internal partial class JsonSourceGenerationRules : JsonSerializerContext
{

}
