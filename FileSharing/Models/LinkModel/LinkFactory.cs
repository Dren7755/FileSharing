namespace FileSharing.Models.FileModel
{
    public class LinkFactory
    {
        public static LinkModel.Link CreateLink(File file)
        {
            string createdDate = file.CreatedDate.ToString();
            string uri = file.User.Login + "-" + createdDate + "-" + file.FileName;
            return new LinkModel.Link
            {
                Uri = uri.Replace(" ", "_")
            };
        }
    }
}