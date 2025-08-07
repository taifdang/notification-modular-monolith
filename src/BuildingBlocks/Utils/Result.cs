namespace BuildingBlocks.Utils;

public class Result<T>
{
    public bool success { get; set; }
    private string? message;
    public T? data { get; set; }

    public string Message
    {
        get { return string.IsNullOrEmpty(message) ? "failure" : message; }
        set { message = value; }
    }
    public static Result<T> Success(T data = default!)
    {
        return new Result<T> { success = true,message= "success", data = data };
    }
    public static Result<T> Failure(string error = default!)
    {
        return new Result<T> { success = false, message = error };
    }
}
