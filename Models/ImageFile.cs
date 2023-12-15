namespace Marketplace.Models;

public class ImageFile
{
    public int Id { get; set; }
    public string FileName { get; set; }
   

    public string Url() {
        return "/itemImg/" + FileName;
    }
}
