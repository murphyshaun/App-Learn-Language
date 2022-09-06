namespace Common.Extension
{
    public static class StringExtension
    {
        public static T ToEnum<T>(this string strEnum) where T : struct
        {
            if (System.Enum.TryParse<T>(strEnum, out T result))
                return result;

            return default(T);
        }

        public static bool IsEquivalentTo(this string str1, string str2)
        {
            if (str1 == null && str2 == null) return true;

            if (str1 == null || str2 == null) return false;

            return str1.Equals(str2);
        }
    }
}