using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartLoader.NET.Framework
{
    /// <summary>
    /// This class contains all the necessary details of the current loaded chart file.
    /// </summary>
    public class Chart
    {
        private Guid _guid;
        /// <summary>
        /// The unique Identification number associated to this reference.
        /// </summary>
        public Guid Guid
        {
            get
            {
                return _guid;
            }
        }

        private string _chartName;
        /// <summary>
        /// The name of this chart.
        /// </summary>
        public string ChartName
        {
            get
            {
                return _chartName;
            }
        }

        private string _chartArtist;
        /// <summary>
        /// The chart's artist.
        /// </summary>
        public string ChartArtist
        {
            get
            {
                return _chartArtist;
            }
        }

        private string _chartCharter;
        /// <summary>
        /// The chart charter.
        /// </summary>
        public string ChartCharter
        {
            get
            {
                return _chartCharter;
            }
        }

        private double _offset;
        /// <summary>
        /// The track's offset in seconds.
        /// </summary>
        public long Offset
        {
            get
            {
                return (long)(_offset * 1000000.0);
            }
        }

        private long _resolution;
        /// <summary>
        /// The chart's resolution.
        /// </summary>
        public long Resolution
        {
            get
            {
                return _resolution;
            }
        }

        private string _player2;
        /// <summary>
        /// The player two type.
        /// </summary>
        public string Player2
        {
            get
            {
                return _player2;
            }
        }

        private int _difficulty;
        /// <summary>
        /// The chart's difficulty
        /// </summary>
        public int Difficulty
        {
            get
            {
                return _difficulty;
            }
        }

        private double _previewStart;
        /// <summary>
        /// The song preview starting point in seconds.
        /// </summary>
        public double PreviewStart
        {
            get
            {
                return _previewStart;
            }
        }

        private double _previewEnd;
        /// <summary>
        /// The song preview ending point in seconds.
        /// </summary>
        public double PreviewEnd
        {
            get
            {
                return _previewEnd;
            }
        }

        private string _genre;
        /// <summary>
        /// The song genre.
        /// </summary>
        public string Genre
        {
            get
            {
                return _genre;
            }
        }

        private string _mediaType;
        /// <summary>
        /// The chart's media type.
        /// </summary>
        public string MediaType
        {
            get
            {
                return _mediaType;
            }
        }

        private string _musicStream;
        /// <summary>
        /// The track generic stream.
        /// </summary>
        public string MusicStream
        {
            get
            {
                return _musicStream;
            }
        }

        private string _guitarStream;
        /// <summary>
        /// The track guitar stream.
        /// </summary>
        public string GuitarStream
        {
            get
            {
                return _guitarStream;
            }
        }

        private string _bassStream;
        /// <summary>
        /// The track bass stream.
        /// </summary>
        public string BassStream
        {
            get
            {
                return _bassStream;
            }
        }

        private SynchTrack[] _synchTracks;

        /// <summary>
        /// The loaded synch tracks.
        /// </summary>
        public SynchTrack[] SynchTracks
        {
            get
            {
                return _synchTracks;
            }
            internal set
            {
                _synchTracks = value;
            }
        }

        private Section[] _sections;

        /// <summary>
        /// All of the related sections associated to this chart.
        /// </summary>
        public Section[] Sections
        {
            get
            {
                return _sections;
            }
            internal set
            {
                _sections = value;
            }
        }

        private Dictionary<string, object> _notes;

        /// <summary>
        /// All the imported notes related to this chart file.
        /// </summary>
        public Dictionary<string, object> Notes
        {
            get 
            { 
                return _notes; 
            }
            set 
            { 
                _notes = value; 
            }
        }
        

        //private Note[] _expertSinglePlayerNotes;

        ///// <summary>
        ///// All the notes associated to a expert single player notes.
        ///// </summary>
        //public Note[] ExpertSinglePlayerNotes
        //{
        //    get
        //    {
        //        return _expertSinglePlayerNotes;
        //    }
        //    internal set
        //    {
        //        _expertSinglePlayerNotes = value;
        //    }
        //}

        //private StarPower[] _expertSinglePlayerSP;

        ///// <summary>
        ///// All the star power associated to a expert single player.
        ///// </summary>
        //public StarPower[] ExpertSinglePlayerSP
        //{
        //    get
        //    {
        //        return _expertSinglePlayerSP;
        //    }
        //    internal set
        //    {
        //        _expertSinglePlayerSP = value;
        //    }
        //}

        /// <summary>
        /// The default constructor;
        /// </summary>
        public Chart()
        {
            _guid = Guid.NewGuid();
            _chartName = "Unknown";
            _chartArtist = "Unknown";
            _chartCharter = "Unknown";
            _chartCharter = "Unknown";
            _offset = 0.0;
            _resolution = 192;
            _player2 = "Unknown";
            _difficulty = -1;
            _previewStart = 0.0;
            _previewEnd = 0.0;
            _genre = "Unknown";
            _mediaType = "CD";
            _musicStream = "Unknown";
            _guitarStream = "Unknown";
            _bassStream = "Unknown";

            _synchTracks = new SynchTrack[0];
            _sections = new Section[0];
            _notes = new Dictionary<string, object>();
            //_expertSinglePlayerNotes = new Note[0];
            //_expertSinglePlayerSP = new StarPower[0];
        }

        /// <summary>
        /// Calculates and converts a tick to a time in microseconds relative to a desired synch track.
        /// </summary>
        /// <param name="synchTrack">The synch track to be relative to.</param>
        /// <param name="Tick">The current tick to convert.</param>
        /// <returns>long</returns>
        public long CalculateTickToTimeRelative(SynchTrack synchTrack, long Tick)
        {
            long microseconds;
            if (synchTrack.BeatsPerMinute == 0)
                microseconds = 0;
            else
                microseconds = 60000000000 / synchTrack.BeatsPerMinute;

            return Tick_to_Time(Tick - synchTrack.Tick,
                microseconds,
                Resolution) + synchTrack.TrueTime;
        }

        /// <summary>
        /// Claculates the current tick to a time value.
        /// </summary>
        /// <param name="tick">The current tick to convert.</param>
        /// <returns>long</returns>
        public long CalculateTickToTime(long tick)
        {
            SynchTrack synchTrack;
            synchTrack = FindCurrentSynch(tick);
            return CalculateTickToTimeRelative(synchTrack, tick);
        }

        /// <summary>
        /// Quantizes a value to the closest measure.
        /// </summary>
        /// <param name="number">The number to quantize</param>
        /// <param name="measure">The current measure.</param>
        /// <returns>long</returns>
        public static long QuantizeLong(long number, long measure)
        {
            double res;
            long output;
            long divider;
            divider = 12;

            res = (double)number / divider;
            output = ((long)Math.Round((float)res) * divider);
            return output;
        }

        /// <summary>
        /// Calculates tick to time.
        /// </summary>
        /// <param name="tick">The tick to calculate.</param>
        /// <param name="microsecondsPerBeat">The microbeats per minute.</param>
        /// <param name="resolution">The current resolution.</param>
        /// <returns>long</returns>
        internal static long Tick_to_Time(long tick, long microsecondsPerBeat, long resolution)
        {
            return tick * microsecondsPerBeat / resolution;
        }

        /// <summary>
        /// Finds the current synch.
        /// </summary>
        /// <param name="tick">The current tick.</param>
        /// <returns>SynchTrack</returns>
        internal SynchTrack FindCurrentSynch(long tick)
        {
            SynchTrack synchResult;

            synchResult = _synchTracks[0];

            foreach (SynchTrack synch in _synchTracks)
            {
                if (tick > synch.Tick)
                    synchResult = synch;
                else
                    break;
            }

            return synchResult;
        }



        /// <summary>
        /// Processes the file scanner enumerator.
        /// </summary>
        /// <param name="enumerator">The file's current enumerator</param>
        internal void ProcessEnumerator(IEnumerator enumerator)
        {
            string currentLine;

            while ((enumerator.MoveNext()) && (enumerator.Current != null))
            {
                currentLine = string.Empty;

                currentLine = enumerator.Current as string;

                if (!currentLine.Equals(string.Empty))
                    if (currentLine.Contains("}"))
                        break;
                    else
                        ProcessLine(currentLine);
            }
        }

        /// <summary>
        /// Processes the current line into meaningful data.
        /// </summary>
        /// <param name="line">The external file string line.</param>
        internal void ProcessLine(string line)
        {
            line = line.Replace("\t", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);

            string[] data = line.Split(new string[] { " = " }, StringSplitOptions.None);

            string tag = data[0].Replace(" ", string.Empty);
            string attribute = string.Empty;

            if (data.Length > 1)
                attribute = data[1].Replace("\"", string.Empty);

            switch (tag)
            {
                case "Name":
                    _chartName = attribute;
                    break;

                case "Artist":
                    _chartArtist = attribute;
                    break;

                case "Charter":
                    _chartCharter = attribute;
                    break;

                case "Offset":
                    _offset = double.Parse(attribute);
                    break;

                case "Resolution":
                    _resolution = long.Parse(attribute);
                    break;

                case "Player2":
                    _player2 = attribute;
                    break;

                case "Difficulty":
                    _difficulty = int.Parse(attribute);
                    break;

                case "PreviewStart":
                    _previewStart = double.Parse(attribute);
                    break;

                case "PreviewEnd":
                    _previewEnd = double.Parse(attribute);
                    break;

                case "Genre":
                    _genre = attribute;
                    break;

                case "MediaType":
                    _mediaType = attribute;
                    break;

                case "MusicStream":
                    _musicStream = attribute;
                    break;

                case "GuitarStream":
                    _guitarStream = attribute;
                    break;

                case "BassStream":
                    _bassStream = attribute;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Retrieves the notes given a key value.
        /// </summary>
        /// <param name="key">The key to use in data extraction.</param>
        /// <returns>Note[]</returns>
        public Note[] GetNotes(string key)
        {
            Dictionary<string, Array> container;
            if (Notes.ContainsKey(key))
            {
                container = (Dictionary<string, Array>)Notes[key];
                return (Note[])container["Notes"];
            }
            else
                return new Note[0];
        }

        /// <summary>
        /// Retrieves the star powers given a key value.
        /// </summary>
        /// <param name="key">The key to use in data extraction.</param>
        /// <returns>StarPower[]</returns>
        public StarPower[] GetStarPower(string key)
        {
            Dictionary<string, Array> container;
            if (Notes.ContainsKey(key))
            {
                container = (Dictionary<string, Array>)Notes[key];
                return (StarPower[])container["SP"];
            }
            else
                return new StarPower[0];
        }

        /// <summary>
        /// Displays the current reference's details.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result;
            result = string.Empty;

            result = ""
                + "Chart: " + _chartName
                //+ "\n Guid: " + _guid
                + "\n Name: " + _chartName
                + "\n Artist: " + _chartArtist
                + "\n Charter: " + _chartCharter
                + "\n Offset: " + _offset
                + "\n Resolution: " + _resolution
                + "\n Player2: " + _player2
                + "\n Difficulty: " + _difficulty
                + "\n PreviewStart: " + _previewStart
                + "\n PreviewEnd: " + _previewEnd
                + "\n Genre: " + _genre
                + "\n MediaType: " + _mediaType
                + "\n MusicStream: " + _musicStream
                + "\n GuitarStream: " + _guitarStream
                + "\n BassStream: " + _bassStream

                + "\n SyncTracks: " + _synchTracks.Length
                + "\n Sections: " + _sections.Length
                + "\n Expert Notes: " + GetNotes("ExpertSingle").Length
                + "\n Hard Notes: " + GetNotes("HardSingle").Length
                + "\n Medium Notes: " + GetNotes("MediumSingle").Length
                + "\n Easy Notes: " + GetNotes("EasySingle").Length
                ;

            return result;
        }

    }
}
