namespace Notify.Core.Contracts.Services;

public interface IFileService
{
    IDictionary<string, string> Read(string folderPath, string fileName);

    void Save(string folderPath, string fileName, IDictionary<string, string> content);
}
