using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenWith
{
    public static class Extensions
    {
        static readonly IEnumerable<LinkedResourceType> linkedResourceTypes = new List<LinkedResourceType> { LinkedResourceType.AttachmentFile, LinkedResourceType.AbsoluteFileUri, LinkedResourceType.RelativeFileUri, LinkedResourceType.RemoteUri };

        public static string GetPathOfSelectedElectronicLocations(this MainForm mainForm)
        {
            return GetPathesOfSelectedElectronicLocations(mainForm).FirstOrDefault();
        }

        public static IEnumerable<string> GetPathesOfSelectedElectronicLocations(this MainForm mainForm)
        {
            var locations = mainForm.GetSelectedElectronicLocations();
            var pathes = new List<string>();

            for (int i = 0; i < locations.Count; i++)
            {
                var location = locations[i];
                var linkedResourceType = location.Address.LinkedResourceType;
                var cachingStatus = location.Address.CachingStatus;

                if (location.LocationType != LocationType.ElectronicAddress) continue;
                if ((linkedResourceType == LinkedResourceType.AttachmentRemote && cachingStatus == CachingStatus.Available) || linkedResourceTypes.Any(l => l == linkedResourceType))
                {
                    var path = location.Address.LinkedResourceType != LinkedResourceType.RemoteUri ? location.Address.Resolve().GetLocalPathSafe() : location.Address.UriString;
                    if (location.Address.LinkedResourceType == LinkedResourceType.RemoteUri || File.Exists(path)) pathes.Add(path);
                }
            }

            return pathes.GroupBy(path => Path.GetExtension(path)).SelectMany(g => g).ToList();
        }

        public static bool IsNotNullOrEmptyOrWhiteSpace(this string @string)
        {
            return !string.IsNullOrEmpty(@string) && !string.IsNullOrWhiteSpace(@string);
        }
    }
}
