using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartLoader.NET.Framework
{
    /// <summary>
    /// This INoteable allows developers to create multiple objects in the notes list.
    /// </summary>
    public interface INoteable
    {
        /// <summary>
        /// The unique Identification number associated to this reference.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// The event's associated chart.
        /// </summary>
        Chart Chart { get; }

        /// <summary>
        /// The event's associated synch track.
        /// </summary>
        SynchTrack SynchTrack { get; }

        /// <summary>
        /// The event's current event line.
        /// </summary>
        EventLine EventLine { get; }

        /// <summary>
        /// The current tick.
        /// </summary>
        long Tick { get; }

        /// <summary>
        /// The current index.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The current duration.
        /// </summary>
        long Duration { get; }

        /// <summary>
        /// The current text line.
        /// </summary>
        string Line { get; }

        /// <summary>
        /// The current type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The event's current time in microseconds.
        /// </summary>
        long Microseconds { get; }

        /// <summary>
        /// The event's current time in seconds.
        /// </summary>
        float Seconds { get; }

        /// <summary>
        /// Retrieves the notes related to this chart.
        /// </summary>
        Note[] Notes { get; }

        /// <summary>
        /// Retrieves the star powers related to this chart.
        /// </summary>
        StarPower[] StarPowers { get; }

        /// <summary>
        /// All the finger indexes related to this event.
        /// </summary>
        bool[] ButtonIndexes { get; }

        /// <summary>
        /// The highest recorded fret.
        /// </summary>
        int HighestFret { get; }

        /// <summary>
        /// Is the note forced solid or not?
        /// </summary>
        bool ForcedSolid { get; }

        /// <summary>
        /// Is the note a forced HOPO or tap note?
        /// </summary>
        bool IsHOPO { get; }

        /// <summary>
        /// The key array related to this chart.
        /// </summary>
        string KeyParent { get; }
        

        /// <summary>
        /// Clones and copies the current reference.
        /// </summary>
        /// <returns>object</returns>
        object Clone();

        /// <summary>
        /// Displays the current reference's details.
        /// </summary>
        /// <returns>string</returns>
        string ToString();

        /// <summary>
        /// Appends the current event line index, to buttonindexes.
        /// </summary>
        /// <param name="eventLine">The eventline to append with.</param>
        void AppendFret(EventLine eventLine);
    }
}
