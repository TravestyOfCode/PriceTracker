using PriceTracker.Data.Results;
using System;

namespace PriceTracker.Web.Utilities;

public static class PageUtilities
{
    public static int StartPage(this IPagedResult result, int pageWindow = 2)
    {
        if (result.Page <= 0 || result.TotalPages <= pageWindow)
        {
            return 1;
        }

        return Math.Max(1, result.Page - pageWindow);
    }

    public static int EndPage(this IPagedResult result, int pageWindow = 2)
    {
        if (result.Page <= 0)
        {
            return 1;
        }

        if (result.Page >= result.TotalPages || result.TotalPages <= (pageWindow * 2))
        {
            return result.TotalPages;
        }

        var startPage = result.StartPage(pageWindow);

        return Math.Max(startPage + (pageWindow * 2), Math.Min(result.Page + pageWindow, result.TotalPages));
    }
}
