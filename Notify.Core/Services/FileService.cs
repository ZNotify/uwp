using System.Text;
using Tommy.Extensions;
using Notify.Core.Contracts.Services;
using Tommy;

namespace Notify.Core.Services;

public class FileService : IFileService
{
    public IDictionary<string, string> Read(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (!File.Exists(path))
        {
            return default;
        }

        using var reader = File.OpenText(path);
        using var parser = new TOMLParser(reader);
        if (!parser.TryParse(out var rootNode, out _))
        {
            return default;
        }

        var ret = new Dictionary<string, string>();

        var table = rootNode.AsTable["App"];

        if (!table.IsTable)
        {
            return default;
        }

        var appTable = table.AsTable;

        // get table keys
        foreach (var (key, value) in appTable.RawTable)
        {
            if (value.IsTable)
            {
                continue;
            }

            ret[key] = value.ToString() ?? "";
        }

        return ret;
    }

    public void Save(string folderPath, string fileName, IDictionary<string, string> content)
    {
        if (folderPath == null)
        {
            return;
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var path = Path.Combine(folderPath, fileName);

        var fullTable = new TomlTable();
        var oldAppTable = new TomlTable();

        if (File.Exists(path))
        {
            try
            {
                var toml = TOML.Parse(File.OpenText(path));
                fullTable = (toml.AsTable);
                var appTable = fullTable["App"].AsTable;
                if (appTable is { IsTable: true })
                {
                    oldAppTable = appTable;
                }
            }
            catch
            {
                // ignored
            }
        }

        foreach (var (key, value) in content)
        {
            oldAppTable[key] = value;
        }

        fullTable["App"] = oldAppTable;

        using var writer = new StreamWriter(path, false, Encoding.UTF8);
        fullTable.WriteTo(writer);
        writer.Flush();
    }
}