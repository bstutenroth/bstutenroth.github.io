using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartLoader.NET.Framework
{

    /// <summary>
    /// The current event details.
    /// </summary>
    public class Section
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
        /// The chart associated to this reference.
        /// </summary>
        public Chart Chart
        {
            get
            {
                return _chart;
            }
        }

        private SynchTrack _synchTrack;

        /// <summary>
        /// The synch track associated to this reference.
        /// </summary>
        public SynchTrack SynchTrack
        {
            get
            {
                return _synchTrack;
            }
        }

        /// <summary>
        /// The current section name.
        /// </summary>
        public string SectionName
        {
            get {
                return EventLine.Text;
            }
        }

        /// <summary>
        /// The current text line from the external file.
        /// </summary>
        public string Line
        {
            get
            {
                return EventLine.Line;
            }
        }

        /// <summary>
        /// The current type of this reference.
        /// </summary>
        public string Type
        {
            get
            {
                return EventLine.Type;
            }
        }

        private EventLine _eventLine;

        /// <summary>
        /// The current event line.
        /// </summary>
        public EventLine EventLine
        {
            get
            {
                return _eventLine;
            }
        }

        /// <summary>
        /// The current tick of this event.
        /// </summary>
        public long Tick
        {
            get
            {
                return EventLine.Tick;
            }
        }

        /// <summary>
        /// The event's time in microseconds.
        /// </summary>
        public long Microseconds
        {
            get
            {
                return Chart.CalculateTickToTimeRelative(SynchTrack, Tick) + Chart.Offset;
            }
        }

        /// <summary>
        /// The event's time in seconds.
        /// </summary>
        public float Seconds
        {
            get
            {
                return Microseconds / 1000000.0f;
            }
        }

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="chart">The associated chart.</param>
        /// <param name="eventLine">The current event line.</param>
        public Section(Chart chart, EventLine eventLine)
        {
            _chart = chart;
            _synchTrack = chart.FindCurrentSynch(eventLine.Tick);
            _guid = Guid.NewGuid();
            _eventLine = eventLine;
        }

        /// <summary>
        /// Processes all events.
        /// </summary>
        internal static Section[] ProcessEvents(IEnumerator enumerator, Chart chart)
        {
            Section[] sections;
            Section r_section;
            string currentLine;
            EventLine eventLine;

            List<Section> sectionsList = new List<Section>();


            while ((enumerator.MoveNext()) && (enumerator.Current != null))
            {
                currentLine = string.Empty;

                currentLine = enumerator.Current as string;

                if (!currentLine.Equals(string.Empty))
                    if (currentLine.Contains("}"))
                        break;
                    else if (!currentLine.Contains("{"))
                    {

                        eventLine = new EventLine(currentLine);

                        r_section = new Section(chart, eventLine);

                        sectionsList.Add(r_section);
                    }
            }

            sections = new Section[sectionsList.Count];
            sectionsList.CopyTo(sections);

            return sections;
        }

        /// <summary>
        /// Clones and copies the current reference.
        /// </summary>
        /// <returns>Event</returns>
        public Section Clone()
        {
            Section r_event = new Section(Chart, EventLine);

            r_event._eventLine = _eventLine;

            return r_event;
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
                + "Guid: " + _guid
                + ", Tick: " + _eventLine.Tick
                + ", Time: " + Seconds
                + ", Section Type: " + _eventLine.Type
                + ", Section Name: " + _eventLine.Text + "\n"
                + SynchTrack
                ;

            return result;
        }

    }
}
