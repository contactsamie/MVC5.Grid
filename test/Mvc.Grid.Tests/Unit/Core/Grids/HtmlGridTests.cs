﻿using NSubstitute;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class HtmlGridTests
    {
        private HtmlHelper html;
        private IGrid<GridModel> grid;
        private HtmlGrid<GridModel> htmlGrid;

        public HtmlGridTests()
        {
            html = HtmlHelperFactory.CreateHtmlHelper("id=3&name=jim");
            grid = new Grid<GridModel>(new GridModel[8]);

            htmlGrid = new HtmlGrid<GridModel>(html, grid);
            grid.Columns.Add(model => model.Name);
            grid.Columns.Add(model => model.Sum);
        }

        #region HtmlGrid(HtmlHelper html, IGrid<T> grid)

        [Fact]
        public void HtmlGrid_DoesNotChangeQuery()
        {
            NameValueCollection query = grid.Query = new NameValueCollection();

            Object actual = new HtmlGrid<GridModel>(html, grid).Grid.Query;
            Object expected = query;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGridQuery()
        {
            grid.Query = null;

            NameValueCollection expected = html.ViewContext.HttpContext.Request.QueryString;
            NameValueCollection actual = new HtmlGrid<GridModel>(html, grid).Grid.Query;

            foreach (String key in expected)
                Assert.Equal(expected[key], actual[key]);

            Assert.Equal(expected.AllKeys, actual.AllKeys);
            Assert.NotSame(expected, actual);
        }

        [Fact]
        public void HtmlGrid_DoesNotChangeHttpContext()
        {
            HttpContextBase httpContext = grid.HttpContext = HttpContextFactory.CreateHttpContextBase("");

            Object actual = new HtmlGrid<GridModel>(html, grid).Grid.HttpContext;
            Object expected = httpContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsHttpContext()
        {
            grid.HttpContext = null;

            HttpContextBase actual = new HtmlGrid<GridModel>(html, grid).Grid.HttpContext;
            HttpContextBase expected = html.ViewContext.HttpContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsPartialViewName()
        {
            String actual = new HtmlGrid<GridModel>(null, grid).PartialViewName;
            String expected = "MvcGrid/_Grid";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsHtml()
        {
            HtmlHelper actual = new HtmlGrid<GridModel>(html, grid).Html;
            HtmlHelper expected = html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGrid()
        {
            IGrid<GridModel> actual = new HtmlGrid<GridModel>(null, grid).Grid;
            IGrid<GridModel> expected = grid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region ToHtmlString()

        [Fact]
        public void ToHtmlString_RendersPartialView()
        {
            IView view = Substitute.For<IView>();
            IViewEngine engine = Substitute.For<IViewEngine>();
            ViewEngineResult result = Substitute.For<ViewEngineResult>(view, engine);
            engine.FindPartialView(Arg.Any<ControllerContext>(), htmlGrid.PartialViewName, Arg.Any<Boolean>()).Returns(result);
            view.When(sub => sub.Render(Arg.Any<ViewContext>(), Arg.Any<TextWriter>())).Do(sub =>
            {
                Assert.Equal(htmlGrid.Grid, sub.Arg<ViewContext>().ViewData.Model);
                sub.Arg<TextWriter>().Write("Rendered");
            });

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);

            String actual = htmlGrid.ToHtmlString();
            String expected = "Rendered";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
