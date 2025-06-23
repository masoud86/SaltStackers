using System;

namespace SaltStackers.Web.Models
{
    public class BreadcrumbItem
    {
        public BreadcrumbItem(string title, string currentController)
        {
            Title = title;
            CurrentController = currentController;
        }

        public BreadcrumbItem(string title, string areaName, string currentController, string currentAction,
            Type fromController, string fromAction, object? routeValues = null)
        {
            Title = title;
            AreaName = areaName;
            CurrentController = currentController;
            CurrentAction = currentAction;
            FromController = fromController;
            FromAction = fromAction;
            RouteValues = routeValues;
        }

        public string? Title { get; set; }

        public string? CurrentAction { get; set; }

        public string? CurrentController { get; set; }

        public string? FromAction { get; set; }

        public Type? FromController { get; set; }

        public string? AreaName { get; set; }

        public object? RouteValues { get; set; }
    }
}
