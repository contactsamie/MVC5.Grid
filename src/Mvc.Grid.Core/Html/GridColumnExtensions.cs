﻿using System;
using System.Web;

namespace NonFactors.Mvc.Grid
{
    public static class GridColumnExtensions
    {
        public static IGridColumn<T> RenderedAs<T>(this IGridColumn<T> column, Func<T, Object> value)
        {
            column.RenderValue = value;

            return column;
        }

        public static IGridColumn<T> MultiFilterable<T>(this IGridColumn<T> column, Boolean isMultiple)
        {
            if (isMultiple && column.Filter.IsEnabled == null)
                column.Filter.IsEnabled = true;

            column.Filter.IsMulti = isMultiple;

            return column;
        }
        public static IGridColumn<T> Filterable<T>(this IGridColumn<T> column, Boolean isFilterable)
        {
            column.Filter.IsEnabled = isFilterable;

            return column;
        }
        public static IGridColumn<T> FilteredAs<T>(this IGridColumn<T> column, String filterName)
        {
            column.Filter.Name = filterName;

            return column;
        }

        public static IGridColumn<T> InitialSort<T>(this IGridColumn<T> column, GridSortOrder order)
        {
            column.InitialSortOrder = order;

            return column;
        }
        public static IGridColumn<T> FirstSort<T>(this IGridColumn<T> column, GridSortOrder order)
        {
            column.FirstSortOrder = order;

            return column;
        }
        public static IGridColumn<T> Sortable<T>(this IGridColumn<T> column, Boolean isSortable)
        {
            column.IsSortable = isSortable;

            return column;
        }

        public static IGridColumn<T> Encoded<T>(this IGridColumn<T> column, Boolean isEncoded)
        {
            column.IsEncoded = isEncoded;

            return column;
        }
        public static IGridColumn<T> Formatted<T>(this IGridColumn<T> column, String format)
        {
            column.Format = format;

            return column;
        }
        public static IGridColumn<T> Css<T>(this IGridColumn<T> column, String cssClasses)
        {
            column.CssClasses = cssClasses;

            return column;
        }
        public static IGridColumn<T> Titled<T>(this IGridColumn<T> column, Object value)
        {
            column.Title = value as IHtmlString ?? new HtmlString(value?.ToString());

            return column;
        }
        public static IGridColumn<T> Named<T>(this IGridColumn<T> column, String name)
        {
            column.Name = name;

            return column;
        }
    }
}
