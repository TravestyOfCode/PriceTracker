using Microsoft.AspNetCore.Http;

namespace PriceTracker.Web.Utilities;

public static class HXExtensions
{
    public static void HXRetarget(this IHeaderDictionary header, string retargetId, string reswap = "")
    {
        if (header == null)
        {
            return;
        }

        header.Add("HX-Retarget", retargetId);

        if (!string.IsNullOrWhiteSpace(reswap))
        {
            header.Add("HX-Reswap", reswap);
        }
    }

    public static void HXRefresh(this IHeaderDictionary header)
    {
        header.Add("HX-Refresh", "true");
    }
}
