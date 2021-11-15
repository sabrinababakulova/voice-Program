using System;
using System.ComponentModel;
using System.Media;
using System.Threading;
using NAudio;
using NAudio.Utils;
using NAudio.Wave;


namespace voice_program
{
    class Program
    {
        static void Main(string[] args)
        {
            recording();  
            Console.ReadLine();
        }      
        static WaveFileWriter waveFile;
       static void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveFile.Write(e.Buffer, 0, e.BytesRecorded);
            short[] sampleData = new short[e.BytesRecorded / 2];
            Buffer.BlockCopy(e.Buffer, 0, sampleData, 0, e.BytesRecorded);
            Double decibels = processAudioFrame(sampleData);
            Console.WriteLine(Convert.ToInt32(decibels));
            if (Convert.ToInt32(decibels) <= 31000) playback();
        }
        public static Double processAudioFrame(short[] audioFrame)
        {
            double rms = 0;
            for (int i = 0; i < audioFrame.Length; i++) rms += audioFrame[i] * audioFrame[i];
            rms = Math.Sqrt(rms / audioFrame.Length);
            return rms;
        }
        public static void playback()
        {
            string[] songs = 
            {
                "господи прекрати орать.mp3",
                "for god's sake.mp3",
                "ты че разоралась.mp3",
                "omg why are you screaming there are people.mp3",
                "jesus christ why are you so loud.mp3",
                "закрой свою варешку.mp3",
                "боже мой заткнись.mp3",
            };
            Random random = new Random();
            int index = random.Next(songs.Length);
            Mp3FileReader mp3 = new Mp3FileReader(songs[index]);
            WaveStream mp3toWAVE = WaveFormatConversionStream.CreatePcmStream(mp3);
            WaveFileWriter.CreateWaveFile("russianone.wav", mp3);
            SoundPlayer player = new SoundPlayer("russianone.wav");
            player.Play();
        }
        public static void recording()
        {
            WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(0);
            Console.WriteLine("Now recording...");
            WaveInEvent waveSource = new WaveInEvent();
            waveSource.DeviceNumber = 0;
            waveSource.WaveFormat = new WaveFormat(8000, 8, 2);
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            string tempFile = ("test1.wav");
            waveFile = new WaveFileWriter(tempFile, waveSource.WaveFormat);
            waveSource.StartRecording();
            Console.ReadLine();
            waveSource.StopRecording();
            waveFile.Dispose();
        }
    }
}
