using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartLoader.NET.Framework
{
    /// <summary>
    /// All the basic details and methods for manipulating 
    /// note events such as notes and star power.
    /// </summary>
    public class NoteEvent : INoteable
    {
        private Guid _guid;

        /// <inheritdoc />
        public Guid Guid
        {
            get
            {
                return _guid;
            }
            protected set
            {
                _guid = value;
            }
        }

        private Chart _chart;

        /// <inheritdoc />
        public Chart Chart
        {
            get
            {
                return _chart;
            }
        }

        private SynchTrack _synchTrack;

        /// <inheritdoc />
        public SynchTrack SynchTrack
        {
            get
            {
                return _synchTrack;
            }
        }

        private EventLine _eventLine;

        /// <inheritdoc />
        public EventLine EventLine
        {
            get 
            { 
                return _eventLine; 
            }
            protected set
            {
                _eventLine = value;
            }
        }

        private bool _forcedSolid;

        /// <inheritdoc />
        public bool ForcedSolid
        {
            get
            {
                return _forcedSolid;
            }
            set
            {
                _forcedSolid = value;
            }
        }

        private bool _isHOPO;

        /// <inheritdoc />
        public bool IsHOPO
        {
            get
            {
                return _isHOPO;
            }
            set
            {
                _isHOPO = value;
            }
        }

        /// <inheritdoc />
        public long Tick
        {
            get
            {
                return EventLine.Tick;
                // Chart.QuantizeLong(EventLine.Tick, SynchTrack.Measures);
            }
        }

        private int _index;

        /// <inheritdoc />
        public int Index
        {
            get
            {
                return _index;
            }
            internal set
            {
                _index = value;
            }
        }

        private int _highestFret;

        /// <inheritdoc />
        public int HighestFret
        {
            get
            {
                return _highestFret;
            }
            protected set
            {
                _highestFret = value;
            }
        }

        /// <inheritdoc />
        public long Duration
        {
            get
            {
                return EventLine.Duration;
            }
        }

        /// <inheritdoc />
        public string Line
        {
            get
            {
                return EventLine.Line;
            }
        }

        /// <inheritdoc />
        public string Type
        {
            get
            {
                return EventLine.Type;
            }
        }

        /// <inheritdoc />
        public long Microseconds
        {
            get
            {
                return Chart.CalculateTickToTimeRelative(SynchTrack, Tick) + Chart.Offset;
            }
        }

        /// <inheritdoc />
        public float Seconds
        {
            get
            {
                return Microseconds / 1000000.0f;
            }
        }

        /// <inheritdoc />
        public Note[] Notes 
        { 
            get
            {
                return Chart.GetNotes(KeyParent);
            }
        }

        /// <inheritdoc />
        public StarPower[] StarPowers
        {
            get
            {
                return Chart.GetStarPower(KeyParent);
            }
        }

        /// <summary>
        /// Retrieves the delta duration relative to this note in microseconds.
        /// </summary>
        public long DurationMicroseconds
        {
            get
            {
                long totalTicks;
                SynchTrack synchTrack;

                totalTicks = Duration + Tick;

                if (totalTicks > 0)
                {
                    synchTrack = Chart.FindCurrentSynch(totalTicks);
                    return (
                        Chart.CalculateTickToTimeRelative(synchTrack, totalTicks) 
                        - Microseconds
                        ) + Chart.Offset;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Retrieves the delta duration relative to this note in seconds.
        /// </summary>
        public float DurationSeconds
        {
            get
            {
                return DurationMicroseconds / 1000000.0f;
            }
        }

        private bool[] _buttonIndexes;

        /// <inheritdoc />
        public bool[] ButtonIndexes
        {
            get
            {
                return _buttonIndexes;
            }
            internal set
            {
                _buttonIndexes = value;
            }
        }


        private string _keyParent;
        /// <inheritdoc />
        public string KeyParent
        {
            get
            {
                return _keyParent;
            }
            set
            {
                _keyParent = value;
            }
        }


        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="chart">The parent chart.</param>
        /// <param name="eventLine">The associated event line.</param>
        /// <param name="keyParent">The key aprent associated to this event.</param>
        public NoteEvent(Chart chart, EventLine eventLine, string keyParent)
        {
            _chart = chart;
            _keyParent = keyParent;
            _synchTrack = chart.FindCurrentSynch(eventLine.Tick);
            _eventLine = eventLine;
            _highestFret = eventLine.Index;
            _buttonIndexes = new bool[5];
            CheckNoteIndex();
            _guid = Guid.NewGuid();
        }

        /// <summary>
        /// Checks to see if the input index is within the range of 0 and 5, 
        /// also applies other elements such as HOPO checks and forced solids.
        /// </summary>
        private void CheckNoteIndex()
        {
            if (_eventLine.Index >= 0 && _eventLine.Index < 5)
                _buttonIndexes[_eventLine.Index] = true;
            else if (_eventLine.Index == 5)
            {
                _forcedSolid = true;
            }
            else if (_eventLine.Index == 6)
            {
                _isHOPO = true;
            }
        }

        /// <summary>
        /// Clones and copies the current reference.
        /// </summary>
        /// <returns>object</returns>
        public object Clone()
        {
            NoteEvent note = new NoteEvent(Chart, EventLine, KeyParent);
            note._eventLine = _eventLine;
            note.IsHOPO = _isHOPO;
            note.ForcedSolid = _forcedSolid;
            note.HighestFret = _highestFret;
            for (int i = 0; i < _buttonIndexes.Length; i++)
                note._buttonIndexes[i] = _buttonIndexes[i];
            return note;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string result;
            result = string.Empty;

            result = ""
                + "Time: " + Seconds
                + ", Highest Fret: " + _highestFret
                + ", Is HOPO: " + _isHOPO
                + ", Forced Solid: " + _forcedSolid
                + ", Index: " + Index
                + "\n\nFrets: \n {"
                + "Gr: " + (_buttonIndexes[0] ? "O" : "X")
                + ", Re: " + (_buttonIndexes[1] ? "O" : "X")
                + ", Ye: " + (_buttonIndexes[2] ? "O" : "X")
                + ", Bl: " + (_buttonIndexes[3] ? "O" : "X")
                + ", Or: " + (_buttonIndexes[4] ? "O" : "X")
                + "}"
                + "\n\n" + EventLine
                + "\n\n" + SynchTrack
                ;

            return result;
        }

        /// <inheritdoc />
        public void AppendFret(EventLine eventLine)
        {
            // Set new highest index
            if (eventLine.Index > HighestFret && eventLine.Index < 5)
                HighestFret = eventLine.Index;

            if (eventLine.Index >= 0 && eventLine.Index < 5)
                ButtonIndexes[eventLine.Index] = true;

            if (eventLine.Index == 5)
            {
                _forcedSolid = true;
                
            }
            else if (eventLine.Index == 6)
            {
                _isHOPO = true;
            }

            
        }
    }
}
