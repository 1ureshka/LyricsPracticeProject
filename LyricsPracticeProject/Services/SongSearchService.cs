using LyricsPracticeProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LyricsPracticeProject.Services
{
    public class SongSearchService
    {
        private readonly HttpClient httpClient;

        public SongSearchService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<SongItem>> SearchSongsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<SongItem>();
            }

            string url = "https://itunes.apple.com/search?term="
                + Uri.EscapeDataString(query)
                + "&entity=song&limit=10";

            string json = await httpClient.GetStringAsync(url);

            ItunesSearchResponse response = JsonConvert.DeserializeObject<ItunesSearchResponse>(json);

            if (response == null || response.Results == null)
            {
                return new List<SongItem>();
            }

            List<SongItem> songs = new List<SongItem>();

            foreach (ItunesTrack track in response.Results)
            {
                if (string.IsNullOrWhiteSpace(track.ArtistName) ||
                    string.IsNullOrWhiteSpace(track.TrackName))
                {
                    continue;
                }

                SongItem song = new SongItem
                {
                    ArtistName = track.ArtistName,
                    TrackName = track.TrackName,
                    CollectionName = track.CollectionName,
                    PreviewUrl = track.PreviewUrl
                };

                songs.Add(song);
            }

            return songs;
        }
    }
}