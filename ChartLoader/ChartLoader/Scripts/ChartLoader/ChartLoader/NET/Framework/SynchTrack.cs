using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartLoader.NET.Framework
{

    /// <summary>
    /// This class controls the current position of each note.
    /// </summary>
    public class SynchTrack
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

        private Chart _chart;

        /// <summary>
        /// The current chart.
        /// </summary>
        public Chart Chart
        {
            get
            {
                return _chart;
            }
        }

        private long _tick;

        /// <summary>
        /// The current tick.
        /// </summary>
        public long Tick
        {
            get {
                return _tick;
            }
        }

        private long _time;

        /// <summary>
        /// The current time in microseconds.
        /// </summary>
        public long Microseconds
        {
            get
            {
                return _time + Chart.Offset;
            }
        }

        /// <summary>
        /// The current time seconds.
        /// </summary>
        public float Seconds
        {
            get
            {
                return (_time + Chart.Offset) / 1000000.0f;
            }
        }

        /// <summary>
        /// The true current time.
        /// </summary>
        internal long TrueTime
        {
            get
            {
                return _time;
            }
        }

        private long _beatsPerMinute;

        /// <summary>
        /// The current beats per minute using this format {100,000}, 
        /// which equates to 100 beats per minute.
        /// </summary>
        public long BeatsPerMinute
        {
            get
            {
                return _beatsPerMinute;
            }
        }

        private int _measures;

        /// <summary>
        /// The measures of this track synch.
        /// </summary>
        public int Measures
        {
            get {
                if (_measures == 0)
                    _measures = 4;
                return _measures;
            }
        }

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="chart">The associated chart.</param>
        /// <param name="previousSynchTrack">The previous synch track.</param>
        public SynchTrack(Chart chart)
        {
            _chart = chart;
            _guid = Guid.NewGuid();
            _tick = 0;
            _beatsPerMinute = 0;
            _measures = 0;
        }

        /// <summary>
        /// By using a string line from the external file, this method appplies all data to the correct properties.
        /// </summary>
        /// <param name="line">Using the external file current line.</param>
        internal void ProcessLine(string line)
        {
            string[] data;
            string r_tick;
            string[] attributes;

            line = line.Replace("\t", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);

            data = line.Split(new string[] { " = " }, StringSplitOptions.None);

            r_tick = data[0].Replace(" ", string.Empty);

            _tick = int.Parse(r_tick);

            if (data.Length > 1)
            {
                attributes = data[1].Split(' ');
                if (attributes[0].Contains("TS"))
                    _measures = int.Parse(attributes[1]);
                else
                    _beatsPerMinute = long.Parse(attributes[1]);
            }
        }

        /// <summary>
        /// Processes all synch tracks.
        /// </summary>
        internal static SynchTrack[] ProcessSynchTracks(IEnumerator enumerator, Chart chart)
        {
            string currentLine;

            SynchTrack[] synchTracks;
            SynchTrack synchTrack;
            SynchTrack previousSynchTrack;

            long previousBPM = 0;
            long MicroBeatsPerMinute;

            List<SynchTrack> synchTracksList = new List<SynchTrack>();

            // Create default synch tracks for later use.
            synchTrack = new SynchTrack(chart);
            previousSynchTrack = synchTrack.Clone();

            // Move to next line and check if there is there a next line.
            while ((enumerator.MoveNext()) && (enumerator.Current != null))
            {
                // Assign next line as currentLine.
                currentLine = string.Empty;
                currentLine = enumerator.Current as string;

                // If line is not empty, continue.
                if (!currentLine.Equals(string.Empty))
                    if (currentLine.Contains("}"))
                        break;
                    else if (!currentLine.Contains("{"))
                    {
                        // Clone and process the previous synch track.
                        synchTrack = previousSynchTrack.Clone();
                        synchTrack.ProcessLine(currentLine);

                        // If the beats per minute have changed, re-assign the latest version.
                        if (previousSynchTrack.BeatsPerMinute != previousBPM)
                            previousBPM = previousSynchTrack.BeatsPerMinute;

                        if (previousBPM == 0)
                            MicroBeatsPerMinute = 0;
                        else
                            MicroBeatsPerMinute = 60000000000 / previousBPM;


                        synchTrack._time = Chart.Tick_to_Time(
                            synchTrack.Tick - previousSynchTrack.Tick,
                            MicroBeatsPerMinute,
                            chart.Resolution)
                                + previousSynchTrack.TrueTime;

                        if (synchTracksList.Count == 0)
                            synchTracksList.Add(synchTrack);

                        if (previousSynchTrack.Tick == synchTrack.Tick)
                            previousSynchTrack.ProcessLine(currentLine);
                        else
                        {
                            synchTracksList.Add(synchTrack);
                            previousSynchTrack = synchTrack;
                        }
                    }
            }

            synchTracks = new SynchTrack[synchTracksList.Count];
            synchTracksList.CopyTo(synchTracks);

            return synchTracks;
        }

        /// <summary>
        /// Clones and copies the current reference.
        /// </summary>
        /// <returns>SynchTrack</returns>
        public SynchTrack Clone()
        {
            SynchTrack synchTrack = new SynchTrack(Chart);

            synchTrack._tick = _tick;
            synchTrack._measures = _measures;
            synchTrack._beatsPerMinute = _beatsPerMinute;
            return synchTrack;
        }

        /// <summary>
        /// Displays the current reference's details.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result;
            result = string.Empty;

            result = "SynchTrack: \n"
                + " Tick: " + _tick
                + ", Measures: " + _measures
                + ", BPM: " + _beatsPerMinute/1000
                ;

            return result;
        }
    }
}
