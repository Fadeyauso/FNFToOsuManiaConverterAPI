using Newtonsoft.Json;

namespace FNFToOsuManiaConverter
{
    public class Song
    {
        public List<NoteSection> notes;
        public string songName;
        public float speed;
        public float bpm;

        public Song(string song, List<NoteSection> notes, float bpm)
        {
            this.songName = song;
            this.notes = notes;
            this.bpm = bpm;
        }

        public static SongData LoadFromJson(string jsonInput)
        {
            string rawJson = File.ReadAllText(jsonInput);

            while (!rawJson.EndsWith("}"))
            {
                rawJson = rawJson.Substring(0, rawJson.Length - 1);
            }

            Console.WriteLine("json loaded");
            return ParseJSON(rawJson);
        }

        public static SongData ParseJSON(string rawJson)
        {
            SongData swagSong = JsonConvert.DeserializeObject<SongData>(rawJson);

            Console.WriteLine("json parsed");

            return swagSong;
        }
    }
}
