namespace FileSharing.Models
{
    public class LinkFactory
    {
        public static Link CreateLink(File file)
        {
            return new Models.Link { Uri = file.User.Login + "/" + file.FileName, AccessPassword = "accessPassword" };
        }
    }
}
