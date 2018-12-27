namespace FileSharing.Models.FileModel
{
    public class LinkFactory
    {
        public static LinkModel.Link CreateLink(File file)
        {
            return new LinkModel.Link { Uri = file.User.Login + "/" + file.FileName, AccessPassword = "accessPassword" };
        }
    }
}
