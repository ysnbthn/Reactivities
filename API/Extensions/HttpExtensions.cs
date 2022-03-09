using System.Text.Json;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
                int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };
            // custom header yap ve headera ekle
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            // Browserın paginationı okuması için onu expose et
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}