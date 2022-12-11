using Microsoft.Extensions.Options;
using Notify.Contracts.Services;
using Notify.Core.Contracts.Services;
using Notify.Helpers;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Notify.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private readonly IFileService _fileService;
    
    private readonly string _applicationDataFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".znotify");

    private const string LocalSettingsFile = "config.toml";

    private IDictionary<string, string> _settings;

    private bool _isInitialized;

    public LocalSettingsService(IFileService fileService)
    {
        _fileService = fileService;
        
        _settings = new Dictionary<string, string>();
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings = await Task.Run(() =>
                            _fileService.Read(_applicationDataFolder,
                                LocalSettingsFile)) ??
                        new Dictionary<string, string>();

            _isInitialized = true;
        }
    }

    public async Task<string?> ReadSettingAsync(string key)
    {
        await InitializeAsync();

        if (_settings.TryGetValue(key, out var obj))
        {
            return (string)obj;
        }


        return default;
    }

    public async Task SaveSettingAsync(string key, string value)
    {
        await InitializeAsync();

        _settings[key] = value;

        await Task.Run(() => _fileService.Save(_applicationDataFolder, LocalSettingsFile, _settings));
    }
}