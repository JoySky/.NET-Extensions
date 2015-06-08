using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Represent a class to implements Paging functionality.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedList<T> : List<T>
{
    /// <summary>
    /// Gets a value that indicate current page index (Starts by Zero).
    /// </summary>
    public int PageIndex { get; private set; }

    /// <summary>
    /// Gets a value that indicate each page size.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Gets a value that indicate count of all rows in data source.
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Gets a value that indicate count of pages in data source.
    /// </summary>
    public int TotalPages { get; private set; }

    public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
    }

    /// <summary>
    /// Gets a value that indicate that does previous page exists or not.
    /// </summary>
    public bool HasPreviousPage
    {
        get
        {
            return (PageIndex > 0);
        }
    }

    /// <summary>
    /// Gets a value that indicate that does next page exists or not.
    /// </summary>
    public bool HasNextPage
    {
        get
        {
            return (PageIndex + 1 < TotalPages);
        }
    }
}

public static class PaginatedListExtensions
{
    /// <summary>
    /// Returns a paginated list.
    /// </summary>
    /// <typeparam name="T">Type of items in list.</typeparam>
    /// <param name="q">A IQueryable instance to apply.</param>
    /// <param name="pageIndex">Page number that starts with zero.</param>
    /// <param name="pageSize">Size of each page.</param>
    /// <returns>Returns a paginated list.</returns>
    /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
    /// <example>
    ///     Following example shows how use this extension method in ASP.NET MVC web application.
    ///     <code>
    ///     public ActionResult Customers(int? page, int? size)
    ///     {
    ///         var q = from p in customers
    ///                 select p;
    ///     
    ///         return View(q.ToPaginatedList(page.HasValue ? page.Value : 1, size.HasValue ? size.Value : 15));
    ///     }
    ///     </code>
    /// </example>
    public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> q, int pageIndex, int pageSize)
    {
        return new PaginatedList<T>(q, pageIndex, pageSize);
    }

    /// <summary>
    /// Returns a paginated list. This function returns 15 rows from specific pageIndex.
    /// </summary>
    /// <typeparam name="T">Type of items in list.</typeparam>
    /// <param name="q">A IQueryable instance to apply.</param>
    /// <param name="pageIndex">Page number that starts with zero.</param>    
    /// <returns>Returns a paginated list.</returns>
    /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
    public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> q, int pageIndex)
    {
        return new PaginatedList<T>(q, pageIndex, 15);
    }

    /// <summary>
    /// Returns a paginated list. This function returns 15 rows from page one.
    /// </summary>
    /// <typeparam name="T">Type of items in list.</typeparam>
    /// <param name="q">A IQueryable instance to apply.</param>    
    /// <returns>Returns a paginated list.</returns>
    /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
    public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> q)
    {
        return new PaginatedList<T>(q, 1, 15);
    }
}
