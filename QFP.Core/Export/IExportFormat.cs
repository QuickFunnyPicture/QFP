using System.IO;

namespace QFP.Core.Export;

internal interface IExportFormat
{
    bool Export(string path);

    bool Export(MemoryStream memoryStream);
}
