using System.IO;

namespace FileSharing.Models.FileModel
{
    public class FileHelper
    {
        private const string FILE_DIR = "/UploadedFiles/";

        public static string CreateFilePath(string rootPath, string fileName, string userName)
        {
            string dirPath = rootPath + FILE_DIR + userName + "/";
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            return dirPath + fileName;
        }
    }
}
