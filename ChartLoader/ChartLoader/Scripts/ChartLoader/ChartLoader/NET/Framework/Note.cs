using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartLoader.NET.Framework
{

    /// <summary>
    /// The note class stores all information related to a note.
    /// </summary>
    public class Note : NoteEvent
    {

        /// <summary>
        /// Checks if this note is a chord or not.
        /// </summary>
        public bool IsChord
        {
            get 
            {
                return CheckIfIsChord(); 
            }
        }
        
        /// <summary>
        /// Checks if the note is a hammer on or not.
        /// </summary>
        public bool IsHammerOn
        {
            get
            {
                return CheckIfIsHammerOn();
            }
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="chart">Requires the associated chart.</param>
        /// <param name="eventLine">Requires the associated event line.</param>
        /// <param name="keyParent">The key of the current difficulty.</param>
        public Note(Chart chart, EventLine eventLine, string keyParent)
            : base(chart, eventLine, keyParent)
        {
            /* TO DO
             */
        }

        /// <summary>
        /// Checks if this note is a hammer-on or not.
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckIfIsHammerOn()
        {
            Note previousNote;

            long QuantizedTick = Chart.QuantizeLong(Tick, SynchTrack.Measures);
            long difference;
            if (Index > 0 && Index < Notes.Length)
            {
                previousNote = Notes[Index - 1];

                if (previousNote != null)
                    difference = QuantizedTick - previousNote.Tick;
                else
                    difference = QuantizedTick;


                if (difference * 2 < Chart.Resolution)
                    if (
                            (
                                (HighestFret != previousNote.HighestFret) 
                                || (HighestFret == previousNote.HighestFret && previousNote.IsChord)
                            )
                            && !ForcedSolid
                        )
                        return true;
                    else
                        return false;
                else
                    return false;

            }
            return false;
        }

        /// <summary>
        /// Copies the current interface noteable to this reference object.
        /// </summary>
        /// <param name="parent">The INoteable parent.</param>
        /// <returns>Note</returns>
        internal static Note GetCopy(INoteable parent)
        {
            Note note = new Note(parent.Chart, parent.EventLine, parent.KeyParent);
            for (int i = 0; i < parent.ButtonIndexes.Length; i++)
                note.ButtonIndexes[i] = parent.ButtonIndexes[i];

            note.EventLine = parent.EventLine;
            note.Guid = parent.Guid;
            note.Index = parent.Index;
            note.HighestFret = parent.HighestFret;
            note.IsHOPO = parent.IsHOPO;
            note.ForcedSolid = parent.ForcedSolid;
            return note;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Note: \n"
                + " IsHammerOn: " + IsHammerOn
                + ", SP: " + IsHammerOn
                + ", " + base.ToString()
                ;
        }

        /// <summary>
        /// Checks if the current note is a chord. Returns true if it is a chord.
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckIfIsChord()
        {
            int totalNotes = 0;

            foreach(bool ans in ButtonIndexes)
                if (ans)
                    totalNotes++;

            return (totalNotes > 1) ? true : false;
        }
    }
}
