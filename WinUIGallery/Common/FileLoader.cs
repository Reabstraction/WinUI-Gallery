using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using AppUIBasics.Helper;
namespace AppUIBasics.Common
{
    internal class FileLoader
    {
        public static async Task<string> LoadText(string relativeFilePath)
        {
            StorageFile file = null;
            if (!WindowHelper.IsAppPackaged)
            {
                var sourcePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), relativeFilePath));
                file = await StorageFile.GetFileFromPathAsync(sourcePath);

            }
            else
            {
                Uri sourceUri = new Uri("ms-appx:///" + relativeFilePath);
                file = await StorageFile.GetFileFromApplicationUriAsync(sourceUri);

            }
            return await FileIO.ReadTextAsync(file);
        }

        public static async Task<IList<string>> LoadLines(string relativeFilePath)
        {
            string fileContents = await LoadText(relativeFilePath);
            return fileContents.Split(Environment.NewLine).ToList();
        }
    }
}
