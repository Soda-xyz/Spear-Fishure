namespace SpearFishure
{
    using System;
    using Avalonia.Controls;
    using Avalonia.Controls.Templates;
    using SpearFishure.ViewModels;

    /// <summary>
    /// Provides view location for view models in the SpearFishure application.
    /// </summary>
    public class ViewLocator : IDataTemplate
    {
    /// <summary>
    /// Builds a view for the given view model parameter.
    /// </summary>
    /// <param name="param">The view model instance.</param>
    /// <returns>The corresponding view control, or a not found message.</returns>
    public Control? Build(object? param)
        {
            if (param is null)
            {
                return null;
            }

            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

    /// <summary>
    /// Determines whether the data is a ViewModelBase.
    /// </summary>
    /// <param name="data">The data to check.</param>
    /// <returns>True if the data is a ViewModelBase; otherwise, false.</returns>
    public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
