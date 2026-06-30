using LyricsPracticeProject.Models;
using LyricsPracticeProject.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LyricsPracticeProject
{
    public partial class Form1 : Form
    {
        private readonly HttpClient httpClient;
        private readonly SongSearchService songSearchService;
        private readonly LyricsService lyricsService;
        private readonly LyricsFormatter lyricsFormatter;
        public Form1()
        {
            InitializeComponent();
            httpClient = new HttpClient();

            songSearchService = new SongSearchService(httpClient);
            lyricsService = new LyricsService(httpClient);
            lyricsFormatter = new LyricsFormatter();

            labelStatus.Text = "Готово к работе.";
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string query = textBoxSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(query))
                {
                    labelStatus.Text = "Введите название песни или исполнителя.";
                    return;
                }

                buttonSearch.Enabled = false;
                labelStatus.Text = "Выполняется поиск песен...";
                listBoxSongs.Items.Clear();

                List<SongItem> songs = await songSearchService.SearchSongsAsync(query);

                if (songs.Count == 0)
                {
                    labelStatus.Text = "Песни не найдены.";
                    return;
                }

                foreach (SongItem song in songs)
                {
                    listBoxSongs.Items.Add(song);
                }

                labelStatus.Text = $"Найдено песен: {songs.Count}. Выберите песню из списка.";
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Ошибка поиска, попробуйте ещё раз";
            }
            finally
            {
                buttonSearch.Enabled = true;
            }
        }

        private async void buttonGetLyrics_Click(object sender, EventArgs e)
        {
            try
            {
                string artist = textBoxArtist.Text.Trim();
                string title = textBoxTitle.Text.Trim();

                if (string.IsNullOrWhiteSpace(artist) ||
                    string.IsNullOrWhiteSpace(title))
                {
                    labelStatus.Text = "Введите исполнителя и название песни.";
                    return;
                }

                buttonGetLyrics.Enabled = false;
                textBoxLyrics.Clear();
                labelStatus.Text = "Получение текста песни...";

                string lyrics = await lyricsService.GetLyricsAsync(artist, title);
                string formattedLyrics = lyricsFormatter.Format(lyrics);

                textBoxLyrics.Text = formattedLyrics;
                labelStatus.Text = "Текст песни успешно получен.";
            }
            catch (Exception exception)
            {
                textBoxLyrics.Text = exception.Message;
                labelStatus.Text = "Не удалось получить текст песни.";
            }
            finally
            {
                buttonGetLyrics.Enabled = true;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
            textBoxArtist.Clear();
            textBoxTitle.Clear();
            textBoxLyrics.Clear();
            listBoxSongs.Items.Clear();

            labelStatus.Text = "Форма очищена.";
        }

        private void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            SongItem selectedSong = listBoxSongs.SelectedItem as SongItem;

            if (selectedSong == null)
            {
                return;
            }

            textBoxArtist.Text = selectedSong.ArtistName;
            textBoxTitle.Text = selectedSong.TrackName;

            labelStatus.Text = "Песня выбрана. Теперь можно получить текст.";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            httpClient.Dispose();
            base.OnFormClosed(e);
        }
    }
}