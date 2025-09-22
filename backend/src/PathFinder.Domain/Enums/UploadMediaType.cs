using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum UploadMediaType
    {
        [Description(".png|.jpg|.jpeg")]
        Image,
        [Description(".pdf|.doc|.docx")]
        Document
    }
}
