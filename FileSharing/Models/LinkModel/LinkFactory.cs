namespace FileSharing.Models.FileModel
{
    public class LinkFactory
    {
        public static LinkModel.Link CreateLink(File file, string accessPassword = null)
        {
            string createdDate = file.CreatedDate.ToString();
            string uri = file.User.Login + "-" + createdDate + "-" + file.FileName;
            return new LinkModel.Link
            {
                Uri = uri.Replace(" ", "_"),
                AccessPassword = accessPassword
            };
        }
    }
}