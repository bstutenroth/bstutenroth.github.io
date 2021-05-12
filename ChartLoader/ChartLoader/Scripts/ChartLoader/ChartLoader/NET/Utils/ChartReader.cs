using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChartLoader.NET.Utils;

using ChartLoader.NET.Framework;

namespace ChartLoader.NET.Utils
{
    /// <summary>
    /// Reads and manipulates external chart files.
    /// </summary>
    public class ChartReader
    {
        private string _path;

        /// <summary>
        /// The chart file path in an external source.
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
        }


        private Chart _chart;
        /// <summary>
        /// The associated chart.
        /// </summary>
        public Chart Chart
        {
            get 
            {
                return _chart;
            }
        }

        private string[] _fileData;

        /// <summary>
        /// The file data as a string array.
        /// </summary>
        public string[] FileData
        {
            get
            {
                return _fileData;
            }
        }

        // The file stream enumerator.
        private IEnumerator _fileScanner;


        /// <summary>
        /// The Constructor with parameters.
        /// </summary>
        public ChartReader()
        {
            _path = "";
            _chart = new Chart();
            _fileData = new string[0];
        }

        /// <summary>
        /// Reads a chart file, given a specified external path, and returns the chart object.
        /// </summary>
        /// <param name="path">The chart path.</param>
        /// <return>Chart</return>
        public Chart ReadChartFile(string path)
        {
            _path = path;
            _fileData = IO.ReadFile(_path);
            ParseChartText(_fileData);
            return Chart;
        }

        /// <summary>
        /// Parses the current string.
        /// </summary>
        /// <param name="chartText">The string to parse.</param>
        /// <return>Chart</return>
        public Chart ParseChartText(string chartText)
        {
            string[] stringLines = chartText.Split(
                new string[] { Environment.NewLine }, 
                StringSplitOptions.None
                );

            ParseChartText(stringLines);
            return Chart;
        }

        /// <summary>
        /// Parses the current string array.
        /// </summary>
        /// <param name="chartTextArray">The string array to parse.</param>
        /// <return>Chart</return>
        public Chart ParseChartText(string[] chartTextArray)
        {
            _fileData = chartTextArray;
            _fileScanner = _fileData.GetEnumerator();
            ProcessFile();
            return Chart;
        }

        /// <summary>
        /// Processes the file string array and converting it into meaningful data.
        /// </summary>
        private void ProcessFile()
        {
            string currentLine;

            while ((_fileScanner.MoveNext()) && (_fileScanner.Current != null))
            {
                currentLine = string.Empty;

                currentLine = _fileScanner.Current as string;

                if (!currentLine.Equals(string.Empty))
                    ProcessLine(currentLine);
            }
        }

        /// <summary>
        /// Processes the current line and parses it.
        /// </summary>
        /// <param name="line">The line to process.</param>
        private void ProcessLine(string line)
        {
            switch(line)
            {
                case "[Song]":
                    _chart.ProcessEnumerator(_fileScanner);
                    break;

                case "[SyncTrack]":
                    _chart.SynchTracks = SynchTrack.ProcessSynchTracks(_fileScanner, Chart);
                    break;

                case "[Events]":
                    _chart.Sections = Section.ProcessEvents(_fileScanner, Chart);
                    break;

                default:
                    ProcessNoteEvents(line);
                    break;
            }
        }


        /// <summary>
        /// Processes all note events.
        /// </summary>
        /// <param name="NoteType">The current note type.</param>
        internal void ProcessNoteEvents(string NoteType)
        {
            Dictionary<string, Array> container;
            Note[] playerNotes;
            StarPower[] playerSP;

            string currentLine;
            currentLine = string.Empty;

            NoteEvent noteEvent = null;

            NoteEvent previousNoteEvent = null;

            List<Note> notesList = new List<Note>();
            List<StarPower> starPowersList = new List<StarPower>();

            NoteType = NoteType.Replace("[", string.Empty)
                    .Replace("]", string.Empty);

            while ((_fileScanner.MoveNext()) && (_fileScanner.Current != null))
            {
                currentLine = string.Empty;

                currentLine = _fileScanner.Current as string;

                if (!currentLine.Equals(string.Empty))
                    // If we reached the end of this section, break out of the loop.
                    if (currentLine.Contains("}"))
                        break;
                    // If the line does not contain a '{' character, begin to parse the string.
                    else if (!currentLine.Contains("{"))
                    {
                        CheckLineContent(currentLine,
                            ref noteEvent,
                            ref previousNoteEvent,
                            ref notesList,
                            ref starPowersList,
                            NoteType
                            );
                    }
            }

            container = new Dictionary<string, Array>();

            playerNotes = new Note[notesList.Count];
            notesList.CopyTo(playerNotes);

            playerSP = new StarPower[starPowersList.Count];
            starPowersList.CopyTo(playerSP);

            container.Add("Notes", playerNotes);
            container.Add("SP", playerSP);

            Chart.Notes.Add(NoteType, 
                container);

        }

        /// <summary>
        /// Checks the current file line, and compares the contents with the previous event.
        /// </summary>
        /// <param name="line">The current line we are working with.</param>
        /// <param name="currentEvent">The current event we are comparing.</param>
        /// <param name="previousEvent">The previous event we previously managed.</param>
        /// <param name="notesList">The current event list.</param>
        /// <param name="starPowersList">The current event list.</param>
        /// <param name="keyParent">The current key parent.</param>
        internal void CheckLineContent(string line,
            ref NoteEvent currentEvent,
            ref NoteEvent previousEvent,
            ref List<Note> notesList,
            ref List<StarPower> starPowersList,
            string keyParent
            )
        {
            EventLine eventLine = new EventLine(line);

            // Create a new INoteable and process the provided line
            currentEvent = new NoteEvent(Chart, eventLine, keyParent);

            if (eventLine.Index == 5)
                currentEvent.ForcedSolid = true;

            if (eventLine.Index == 6)
                currentEvent.IsHOPO = true;

            // If there are no notes, add it to the list.
            if (notesList.Count == 0)
                notesList.Add(Note.GetCopy(currentEvent));


            // If previous event is null, just make a new copy.
            if (previousEvent == null)
                previousEvent = (NoteEvent)currentEvent.Clone();


            // If the previous starPower is on the same tick and has the same type.
            if (previousEvent.Tick == currentEvent.Tick
                && previousEvent.Type == currentEvent.Type)
            {

                previousEvent.AppendFret(eventLine);

                if (previousEvent.Type.Contains("N"))
                {
                    if (notesList.Count - 1 >= 0)
                        notesList.RemoveAt(notesList.Count - 1);
                    notesList.Add(Note.GetCopy(previousEvent));
                }
                else
                {
                    if (starPowersList.Count - 1 >= 0)
                        starPowersList.RemoveAt(starPowersList.Count - 1);
                    starPowersList.Add(StarPower.GetCopy(previousEvent));
                }

            }

            else
            {


                /* Else if it is diferent and is a StarPower type, 
                add it to the list and re-assign the previous starPower.*/
                if (currentEvent.Type.Contains("N"))
                {
                    currentEvent.Index = notesList.Count;
                    notesList.Add(Note.GetCopy(currentEvent));
                }
                else
                {
                    currentEvent.Index = starPowersList.Count;
                    starPowersList.Add(StarPower.GetCopy(currentEvent));
                }

                previousEvent = currentEvent;
            }
        }
    }
}
