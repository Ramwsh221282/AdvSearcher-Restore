using AdvSearcher.Parser.SDK.Options;

namespace AdvSearcher.Backend.TauriPlugIn.Controllers;

// Request/Response values
public sealed record SettingsRequest(SettingsSection Section, List<SettingsSectionItem> Settings);

// Section of settings that contains section name and file name of this section
public sealed record SettingsSection(string SectionName, string FileName);

// Settings section item. Represents key and value of the setting.
public sealed record SettingsSectionItem(string Key, string Value);

public class SettingsController(IOptionManager manager)
{
    // Settings manager.
    private readonly IOptionManager _manager = manager;

    // Method for creating section of settings.
    public void SetSettings(SettingsRequest request)
    {
        try
        {
            IOptionProcessor processor = _manager.CreateWrite(request.Section.FileName);
            List<Option> options = request.CreateOptionsFromRequest();
            options.ForEach(option => processor.Process(option).Wait());
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }

    // Method for reading settings from section.
    public SettingsRequest ReadSettings(SettingsRequest request)
    {
        try
        {
            IOptionProcessor processor = _manager.CreateReader(request.Section.FileName);
            List<Option> options = request.CreateOptionsFromRequest();
            List<Option> readedOptions = new List<Option>(options.Count);
            options.ForEach(option => readedOptions.Add(processor.Process(option).Result));
            List<SettingsSectionItem> responseItems = readedOptions.CreateSettingsFromOptions();
            return new SettingsRequest(request.Section, responseItems);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
            return null!;
        }
    }

    // Method for flushing (deleting) section of settings.
    public void FlushSettings(SettingsRequest request)
    {
        try
        {
            IOptionProcessor processor = _manager.CreateFlusher(request.Section.FileName);
            List<Option> options = request.CreateOptionsFromRequest();
            options.ForEach(option => processor.Process(option).Wait());
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.Source);
        }
    }
}

// Bunch of helper methods.
public static class SettingsControllerExtensions
{
    // Converts request to Option List.
    public static List<Option> CreateOptionsFromRequest(this SettingsRequest request)
    {
        List<Option> options = new List<Option>(request.Settings.Count);
        request.Settings.ForEach(item => options.Add(new Option(item.Key, item.Value)));
        return options;
    }

    // Converts options to SettingsSectionItem response part.
    public static List<SettingsSectionItem> CreateSettingsFromOptions(this List<Option> options)
    {
        List<SettingsSectionItem> settings = new List<SettingsSectionItem>(options.Count);
        options.ForEach(option => settings.Add(new SettingsSectionItem(option.Key, option.Value)));
        return settings;
    }
}
