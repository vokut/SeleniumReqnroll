using Reqnroll;
using Selenium.Reqnroll.Enums;
using System.ComponentModel;
using System.Reflection;

namespace Selenium.Reqnroll.Support
{
    [Binding]
    public class StepTransformations
    {
        [StepArgumentTransformation]
        public PopupMessages TransformToPopupMessage(string messageText)
        {
            var match = Enum.GetValues<PopupMessages>()
                .Cast<PopupMessages?>()
                .FirstOrDefault(e => e!.Value.GetType()
                    .GetField(e.Value.ToString())?
                    .GetCustomAttribute<DescriptionAttribute>()?.Description == messageText);

            if (match == null)
            {
                throw new ArgumentException($"Oops! The text '{messageText}' does not match any official popup message in the Enum.");
            }

            return match.Value;
        }

        [StepArgumentTransformation]
        public MenuItems TransformToMenuItem(string sectionName)
        {
            // Search for a match by the [Description] attribute (returns a nullable)
            var matchByDescription = Enum.GetValues<MenuItems>()
                .Cast<MenuItems?>()
                .FirstOrDefault(e => e!.Value.GetType()
                    .GetField(e!.Value.ToString())?
                    .GetCustomAttribute<DescriptionAttribute>()?.Description == sectionName);

            if (matchByDescription != null)
            {
                return matchByDescription.Value;
            }

            // If there is no description, try matching by the enum name directly (safer than a plain TryParse)
            var matchByName = Enum.GetValues<MenuItems>()
                .Cast<MenuItems?>()
                .FirstOrDefault(e => string.Equals(e!.Value.ToString(), sectionName, StringComparison.OrdinalIgnoreCase));

            if (matchByName != null)
            {
                return matchByName.Value;
            }

            // If neither description nor name matches (possible typo in the feature file)
            throw new ArgumentException($"Error: Menu '{sectionName}' does not match any known menu items. Please check the feature file for typos or update the enum with a matching description.");
        }
    }
}
