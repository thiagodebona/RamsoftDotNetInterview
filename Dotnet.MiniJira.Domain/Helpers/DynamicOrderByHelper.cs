namespace Dotnet.MiniJira.Domain.Helpers
{
    public static class DynamicOrderByHelper
    {
        public static List<T> CustomSort<T>(this List<T> input, string property, bool desc)
        {
            var type = typeof(T);
            var sortProperty = type.GetProperty(property);

            if (sortProperty == null)
                throw new Exception($"The field {property} does not exists in the object {type}");

            return desc ? input.OrderByDescending(p => sortProperty.GetValue(p, null)).ToList() : input.OrderBy(p => sortProperty.GetValue(p, null)).ToList();
        }
    }
}
