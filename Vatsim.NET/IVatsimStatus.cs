using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal interface IVatsimStatus
    {
        IReadOnlyList<Uri> GetAllAtisUrls();
        IReadOnlyList<Uri> GetAllDataUrls();
        IReadOnlyList<Uri> GetAllMetarUrls();
        IReadOnlyList<Uri> GetAllServerUrls();
        IReadOnlyList<Uri> GetAllUserUrls();
        Uri GetAtisUrl();
        Uri GetDataUrl();
        IReadOnlyList<string> GetMessages();
        Uri GetMetarUrl();
        Uri GetServerUrl();
        Uri GetUserUrl();
    }
}