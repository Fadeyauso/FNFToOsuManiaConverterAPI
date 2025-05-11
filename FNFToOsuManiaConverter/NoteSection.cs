using Newtonsoft.Json;

namespace FNFToOsuManiaConverter
{
    public class NoteSection
    {
        [JsonProperty("lengthInSteps")]
        public int lengthInSteps;

        [JsonProperty("sectionNotes")]
        [JsonConverter(typeof(OnlyDecimalConverter))]
        public List<List<decimal>> sectionNotes;

        [JsonProperty("sectionBeats")]
        public int sectionBeats;

        [JsonProperty("mustHitSection")]
        public bool mustHitSection;

        [JsonProperty("changeBPM")]
        public bool changeBPM;

        [JsonProperty("bpm")]
        public float bpm;

        [JsonProperty("typeOfSection")]
        public long typeOfSection;
    }
}
