

using System.Text.Json.Serialization;

namespace School_View_Models
{
    public class GroupViewModel
    {

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("group_Image_Url")]
        public string Group_Image_Url { get; set; }

        [JsonPropertyName("members")]
        public List<int> Members { get; set; }

        [JsonPropertyName("admin")]
        public int Admin { get; set; }
    }
}
