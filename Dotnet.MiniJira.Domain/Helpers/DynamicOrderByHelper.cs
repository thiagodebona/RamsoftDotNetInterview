namespace Dotnet.MiniJira.Domain.Helpers
{
    public static class DynamicOrderByHelper
    {
        public static List<T> CustomSortBy<T>(this List<T> input, string orderByField, bool orderByDesc, string sortFavoritesInTopFieldName = "")
        {
            var type = typeof(T);
            var favoriteField = type.GetProperty(sortFavoritesInTopFieldName);
            var orderBy = type.GetProperty(orderByField);

            if (!string.IsNullOrEmpty(sortFavoritesInTopFieldName) && favoriteField == null)
                throw new Exception($"The field sortFavoritesInTopFieldName {sortFavoritesInTopFieldName} does not exists in the object {type}");

            if (orderBy == null)
                throw new Exception($"The field orderBy {orderBy} does not exists in the object {type}");

            var result = new List<T>();
            if (favoriteField != null)
            {
                if (orderByDesc)
                    result = input.OrderByDescending(p => favoriteField.GetValue(p, null)).ThenByDescending(p => orderBy.GetValue(p, null)).ToList();
                else
                    result = input.OrderByDescending(p => favoriteField.GetValue(p, null)).ThenBy(p => orderBy.GetValue(p, null)).ToList();
            }
            else
            {
                if (orderByDesc)
                    result = input.OrderByDescending(p => orderBy.GetValue(p, null)).ToList();
                else
                    result = input.OrderBy(p => orderBy.GetValue(p, null)).ToList();
            }

            return result.ToList();
        }
    }
}
