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
            if (TryGetEnumByDescription<PopupMessages>(messageText, out var matchedMessage))
            {
                return matchedMessage;
            }

            throw new ArgumentException($"Oops! The text '{messageText}' does not match any official popup message in the Enum.");
        }

        [StepArgumentTransformation]
        public MenuItems TransformToMenuItem(string sectionName)
        {
            if (TryGetEnumByDescription<MenuItems>(sectionName, out var matchedMenu))
            {
                return matchedMenu;
            }

            throw new ArgumentException($"Error: Menu '{sectionName}' does not match any known menu items. Please check the feature file for typos.");
        }

        /// <summary>
        /// Reusable generic helper that looks up any Enum value by its [Description] attribute value.
        /// </summary>
        private static bool TryGetEnumByDescription<TEnum>(string description, out TEnum result) where TEnum : struct, Enum
        {
            foreach (TEnum val in Enum.GetValues<TEnum>())
            {
                FieldInfo? fieldInfo = val.GetType().GetField(val.ToString());
                var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();

                if (attribute != null && string.Equals(attribute.Description, description, StringComparison.OrdinalIgnoreCase))
                {
                    result = val;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}