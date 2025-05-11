using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FNFToOsuManiaConverter
{
    public class FNFChart
    {
        public SongData songData;
        public List<Note> actualNotes = new List<Note>();
        public FNFChart(string filePath)
        {
            songData = Song.LoadFromJson(filePath);
        }

        //0 is opponent only, 1 is bf only, 2 or else is both but its broken and notes can overlap each other
        public void ParseShitAndSaveFile(string filePath, int whichNotesToUse = 0)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FNFChart fnfChart = new FNFChart(filePath);
                var noteData = fnfChart.songData.song.notes;

                for (int i = 0; i < noteData.Count; i++)
                {
                    NoteSection section = noteData[i];

                    if (section.sectionNotes != null && section.sectionNotes.Count > 0)
                    {
                        for (int v = 0; v < section.sectionNotes.Count; v++)
                        {
                            decimal strumTime = section.sectionNotes[v][0];
                            int noteDataIndex = (int)section.sectionNotes[v][1];
                            bool gottaHitNote = section.mustHitSection;

                            if (noteDataIndex > 3 && whichNotesToUse == 1) //only bf notes
                            {
                                gottaHitNote = !section.mustHitSection;
                            }

                            // Used to detect sustains
                            Note oldNote = null;
                            if (actualNotes.Count > 0)
                            {
                                oldNote = actualNotes[actualNotes.Count - 1];
                            }

                            Note newNote = Note.Create(strumTime, noteDataIndex, gottaHitNote, false, oldNote);
                            newNote.ConvertToMania();

                            if(whichNotesToUse == 0)
                            {
                                newNote.mustPress = !newNote.mustPress;
                            }

                            actualNotes.Add(newNote);

                            //broken for now
                            /*
                            decimal sustainLength = section.sectionNotes[v][2];
                            int flooredSus = decimal.ToInt32(sustainLength);

                            if (flooredSus > 0 && actualNotes.Count > 0)
                            {
                                int osuFormattedLength = (int)(strumTime + sustainLength);

                                oldNote = actualNotes[actualNotes.Count - 1];

                                Note sustainNote = Note.Create(strumTime, noteDataIndex, gottaHitNote, true, oldNote);
                                sustainNote.ConvertToMania();
                                sustainNote.osuSustainTime = osuFormattedLength;
                                actualNotes.Add(sustainNote);
                            }
                            */
                        }
                    }
                }
                // Sort from least StrumTime to greatest StrumTime
                actualNotes = actualNotes.Distinct().ToList();
                actualNotes.Sort((a, b) => a.strumTime.CompareTo(b.strumTime));

                using (var writer = new StreamWriter(Path.GetFullPath(filePath) + $"{songData.song.songName}-Converted.osu", false))
                {
                    //gonna hardcode the structure for now, in forms version i'm gonna improve it

                    writer.WriteLine("osu file format v14\n");

                    writer.WriteLine("[General]");
                    writer.WriteLine($"AudioFilename: Instt.mp3");
                    writer.WriteLine($"AudioLeadIn: 0");
                    writer.WriteLine($"PreviewTime: 25000");
                    writer.WriteLine($"Countdown: 0");
                    writer.WriteLine($"SampleSet: Normal");
                    writer.WriteLine($"StackLeniency: 0.7");
                    writer.WriteLine($"Mode: 3\n");

                    writer.WriteLine("[Metadata]");
                    writer.WriteLine($"Title: {songData.song.songName}");
                    writer.WriteLine($"Artist: Example Artist");
                    writer.WriteLine($"Creator: Example Creator");
                    writer.WriteLine($"Version: Example Difficulty\n");

                    writer.WriteLine("[Difficulty]");
                    writer.WriteLine($"HPDrainRate: 4");
                    writer.WriteLine($"CircleSize: 4");
                    writer.WriteLine($"OverallDifficulty: 4");
                    writer.WriteLine($"ApproachRate: 4");
                    writer.WriteLine($"SliderMultiplier: 1");
                    writer.WriteLine($"SliderTickRate: 1\n");

                    writer.WriteLine("[TimingPoints]");
                    var timingShit = new OsuTimingPoint();
                    timingShit.time = -20; //maybe its individual idk rly
                    timingShit.beatLength = 60000M / (decimal)songData.song.bpm;
                    timingShit.uninherited = 1;
                    writer.WriteLine($"{timingShit.time},{timingShit.beatLength},{timingShit.meter},{timingShit.sampleSet},{timingShit.sampleIndex},{timingShit.volume},{timingShit.uninherited},{timingShit.effects}\n");

                    writer.WriteLine("[HitObjects]");
                    for (int i = 0; i < actualNotes.Count; i++)
                    {
                        if (actualNotes[i].isHoldNote)
                        {
                            writer.WriteLine($"{actualNotes[i].osuX},{actualNotes[i].osuY},{actualNotes[i].osuStrumTime - 20},{actualNotes[i].osuType},0,{actualNotes[i].osuSustainTime}");
                        }
                        writer.WriteLine($"{actualNotes[i].osuX},{actualNotes[i].osuY},{actualNotes[i].osuStrumTime - 20},{actualNotes[i].osuType},0");
                    }
                }

                Console.ReadLine();
            }
        }
    }

    public class OnlyDecimalConverter : JsonConverter
    {
        public static bool DisableCustomNotes = true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<List<decimal>>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            List<List<decimal>> onlyDecimals = new List<List<decimal>>();
            List<decimal> listOfDecimals = new List<decimal>();

            foreach (JToken token in array.Children())
            {
                if (DisableCustomNotes)
                    foreach (var item in token.ToArray())
                    {
                        if (item.Type == JTokenType.String)
                        {
                            //token[1] = int.MaxValue; // set non existing NoteType if custom note has been found
                            array.Remove(item);
                        }
                    }

                foreach (var item in token)
                {
                    if (item.Type == JTokenType.Float || item.Type == JTokenType.Integer)
                        listOfDecimals.Add(item.ToObject<decimal>()); // filter all non compatible types
                }

                onlyDecimals.Add(listOfDecimals.ToList());
                listOfDecimals.Clear();
            }

            return onlyDecimals;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var notes = (List<List<decimal>>)value;

            writer.WriteStartArray();

            foreach (var note in notes)
            {
                writer.WriteStartArray();

                foreach (var n in note)
                {
                    writer.WriteValue(n);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }
    }
}