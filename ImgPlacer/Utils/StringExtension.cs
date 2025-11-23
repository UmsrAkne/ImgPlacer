namespace ImgPlacer.Utils
{
    public static class StringExtension
    {
        public static string ToTopLower(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return char.ToLower(value[0]) + value[1..];
        }
    }
}