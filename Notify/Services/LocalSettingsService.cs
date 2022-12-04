using Microsoft.Extensions.Options;
using Notify.Contracts.Services;
using Notify.Core.Contracts.Services;
using Notify.Core.Helpers;
using Notify.Helpers;
using Notify.Models;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Notify.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string DefaultApplicationDataFolder = "Notify/ApplicationData";
    private const string DefaultLocalSettingsFile = "LocalSettings.json";

    private readonly IFileService _fileService;

    private readonly string _localApplicationData =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    // home directory of the user
    private readonly string _applicationDataFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".znotify");

    private readonly string _localSettingsFile = "config.toml";

    private IDictionary<string, object> _settings;

    private bool _isInitialized;

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;

        _settings = new Dictionary<string, object>();
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings = await Task.Run(() =>
                            _fileService.Read<IDictionary<string, object>>(_applicationDataFolder,
                                _localSettingsFile)) ??
                        new Dictionary<string, object>();

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

        await Task.Run(() => _fileService.Save(_applicationDataFolder, _localSettingsFile, _settings));
    }
}