namespace FileSharing.Models.FileModel
{
    public class LinkFactory
    {
        public const string FILE_GET_CONTROLLER_ACTION = "/File/Get/";

        public static LinkModel.Link CreateLink(File file, string accessPassword = null)
        {
            string createdDate = file.CreatedDate.ToString();
            string uri = file.User.Login + "-" + createdDate + "-" + file.FileName.Replace(" ", "_");
            return new LinkModel.Link
            {
                Uri = uri,
                Url = FILE_GET_CONTROLLER_ACTION + uri,
                AccessPassword = accessPassword
            };
        }
    }
}