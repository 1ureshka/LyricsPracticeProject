using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricsPracticeProject.Models
{
    public class ItunesSearchResponse
    {
        public int ResultCount { get; set; }
        public List<ItunesTrack> Results { get; set; }
    public ItunesSearchResponse()
        {
            Results = new List<ItunesTrack>();
        }
    }
}