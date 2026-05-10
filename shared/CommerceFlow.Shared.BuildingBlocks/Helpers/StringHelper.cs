namespace CommerceFlow.Shared.Helpers;

public static class StringHelper
{
    public static string Normalize(string str) => str.Trim().ToUpperInvariant(); 
}