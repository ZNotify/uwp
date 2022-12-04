namespace Notify.Contracts.Services;

public interface ILocalSettingsService
{
    Task<string?> ReadSettingAsync(string key);

    Task SaveSettingAsync(string key, string value);
}
