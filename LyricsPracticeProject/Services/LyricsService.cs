using LyricsPracticeProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LyricsPracticeProject.Services
{
    public class LyricsService
    {
        private readonly HttpClient httpClient;

        public LyricsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetLyricsAsync(string artist, string title)
        {
            if (string.IsNullOrWhiteSpace(artist))
            {
                throw new ArgumentException("Не указан исполнитель.");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Не указано название песни.");
            }

            string url = "https://api.lyrics.ovh/v1/" + Uri.EscapeDataString(artist) + "/" + Uri.EscapeDataString(title);

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new InvalidOperationException("Текст песни не найден.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Ошибка API. Код ответа: " + (int)response.StatusCode);
                }

                string json = await response.Content.ReadAsStringAsync();

                LyricsResponse lyricsResponse =
                    JsonConvert.DeserializeObject<LyricsResponse>(json);

                if (lyricsResponse == null ||
                    string.IsNullOrWhiteSpace(lyricsResponse.Lyrics))
                {
                    throw new InvalidOperationException("API вернул пустой текст.");
                }

                return lyricsResponse.Lyrics;
            }
        }
    }
}