using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("Users")]
    public class MainUserDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UserInfosDtoExport[] UsersInfo { get; set; }
    }
}
