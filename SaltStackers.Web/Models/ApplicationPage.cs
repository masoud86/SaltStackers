using System;
using System.Diagnostics.CodeAnalysis;

namespace SaltStackers.Web.Models
{
    public class ApplicationPage : IEquatable<ApplicationPage>
    {
        public string AreaName { get; set; }
        public string? ControllerName { get; set; }
        public string ActionName { get; set; }

        public bool Equals([AllowNull] ApplicationPage other)
        {
            return AreaName == other.AreaName &&
                ControllerName == other.ControllerName &&
                ActionName == other.ActionName;
        }

        public override int GetHashCode()
        {
            int hasArea = AreaName == null ? 0 : AreaName.GetHashCode();
            int hasController = ControllerName == null ? 0 : ControllerName.GetHashCode();
            int hasAction = ActionName == null ? 0 : ActionName.GetHashCode();

            return hasArea ^ hasController ^ hasAction;
        }
    }
}
