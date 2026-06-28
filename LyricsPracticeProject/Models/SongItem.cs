using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricsPracticeProject.Models
{
    public class SongItem
    {
        public string ArtistName { get; set; } = "";
        public string TrackName { get; set; } = "";
        public string CollectionName { get; set; } = "";
        public string PreviewUrl { get; set; } = "";

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(CollectionName))
            {
                return $"{ArtistName} - {TrackName} ({CollectionName})";

            }

            return $"{ArtistName} - {TrackName}";
        }
    }
}
