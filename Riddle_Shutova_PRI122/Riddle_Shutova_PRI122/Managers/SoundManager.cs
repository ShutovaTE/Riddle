using NAudio.Wave;
using System;
using System.IO;

namespace Riddle_Shutova_PRI122.Managers
{
    public class SoundManager : IDisposable
    {
        private WaveOutEvent musicPlayer;
        private AudioFileReader musicReader;
        private bool isMusicPlaying = false;
        private float currentVolume = 1.0f;

        private string musicPath;
        private string coinPath;
        private string gameOverPath;

        public SoundManager(string musicFile, string coinFile, string gameOverFile)
        {
            musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, musicFile);
            coinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, coinFile);
            gameOverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, gameOverFile);
        }

        public void PlayBackgroundMusic(bool loop = true)
        {
            if (!File.Exists(musicPath))
            {
                return;
            }

            try
            {
                StopBackgroundMusic();

                musicReader = new AudioFileReader(musicPath);
                musicPlayer = new WaveOutEvent();
                musicPlayer.Init(musicReader);
                musicPlayer.Volume = currentVolume;   
                musicPlayer.Play();

                if (loop)
                {
                    musicPlayer.PlaybackStopped += OnMusicPlaybackStopped;
                }
                isMusicPlaying = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void OnMusicPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (isMusicPlaying && musicPlayer != null)
            {
                musicReader.Position = 0;
                musicPlayer.Volume = currentVolume;   
                musicPlayer.Play();
            }
        }

        public void StopBackgroundMusic()
        {
            if (musicPlayer != null)
            {
                musicPlayer.Stop();
                musicPlayer.Dispose();
                musicPlayer = null;
            }
            if (musicReader != null)
            {
                musicReader.Dispose();
                musicReader = null;
            }
            isMusicPlaying = false;
        }

        public void SetVolume(float volume) 
        {
            currentVolume = volume;
            if (musicPlayer != null)
            {
                musicPlayer.Volume = currentVolume;
            }
        }

        public void PlayCoinSound()
        {
            PlaySoundEffect(coinPath);
        }

        public void PlayGameOverSound()
        {
            PlaySoundEffect(gameOverPath);
        }

        private void PlaySoundEffect(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                var reader = new AudioFileReader(filePath);
                var player = new WaveOutEvent();
                player.Init(reader);
                player.Play();

                player.PlaybackStopped += (s, e) =>
                {
                    player.Dispose();
                    reader.Dispose();
                };
            }
            catch (Exception ex)
            {
            }
        }

        public void Dispose()
        {
            StopBackgroundMusic();
        }
    }
}