using System.Text.Json;
namespace BuildingBlocks.Utils;

//ref: https://stackoverflow.com/questions/71295864/how-to-validate-a-list-of-json-objects-using-fluent-validation-and-azure-functio
//ref: https://code-maze.com/csharp-how-to-ensure-a-string-is-valid-json/
public static class ValidateJsonObject
{
    public static bool IsValidJson(string input)
    {
        try
        {
            JsonDocument.Parse(input);
            return true;
        }
        catch(JsonException)
        {
            return false;
        }
    }
}
