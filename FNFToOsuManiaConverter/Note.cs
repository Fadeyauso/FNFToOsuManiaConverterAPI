using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNFToOsuManiaConverter
{
    public class Note
    {
        public decimal strumTime;
        public int noteData;
        public bool mustPress;
        public bool isHoldNote;
        public Note prevNote;
        public int osuX = 0;
        public int osuY = 192;
        public int osuStrumTime = 0;
        public int osuType = 1;
        public int osuSustainTime = 0;

        public static Note Create(decimal strumTime, int noteData, bool mustPress, bool isHoldNote, Note prevNote)
        {
            Note newNote = new Note();

            newNote.strumTime = strumTime;
            newNote.noteData = noteData;
            newNote.mustPress = mustPress;
            newNote.isHoldNote = isHoldNote;
            newNote.prevNote = prevNote;

            return newNote;
        }

        public void ConvertToMania()
        {
            osuStrumTime = (int)strumTime;
            switch (noteData)
            {
                case 0:
                    osuX = 64;
                    break;
                case 1:
                    osuX = 128;
                    break;
                case 2:
                    osuX = 256;
                    break;
                case 3:
                    osuX = 448;
                    break;
                case 4:
                    osuX = 64;
                    break;
                case 5:
                    osuX = 128;
                    break;
                case 6:
                    osuX = 256;
                    break;
                case 7:
                    osuX = 448;
                    break;
            }

            if (isHoldNote)
            {
                osuType = 128;
            }
        }
    }
}
