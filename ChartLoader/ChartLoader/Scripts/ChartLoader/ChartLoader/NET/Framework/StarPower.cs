using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartLoader.NET.Framework
{
    /// <summary>
    /// The starPower class stores all information related to star power.
    /// </summary>
    public class StarPower : NoteEvent
    {
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="chart">Requires the associated chart.</param>
        /// <param name="eventLine">Requires the associated event line.</param>
        /// <param name="keyParent">Requires the associated key parent.</param>
        public StarPower(Chart chart, EventLine eventLine, string keyParent)
            : base(chart, eventLine, keyParent)
        {
            /* TO DO
             */ 
        }

        /// <summary>
        /// Copies the current interface noteable to this reference object.
        /// </summary>
        /// <param name="parent">The INoteable parent.</param>
        /// <returns>StarPower</returns>
        internal static StarPower GetCopy(INoteable parent)
        {
            StarPower starPower = new StarPower(parent.Chart, parent.EventLine, parent.KeyParent);
            for (int i = 0; i < parent.ButtonIndexes.Length; i++)
                starPower.ButtonIndexes[i] = parent.ButtonIndexes[i];

            
            starPower.EventLine = parent.EventLine;
            starPower.Guid = parent.Guid;
            starPower.IsHOPO = parent.IsHOPO;
            starPower.ForcedSolid = parent.ForcedSolid;
            starPower.HighestFret = parent.HighestFret;
            starPower.Index = parent.Index;
            return starPower;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Star Power "
                + EventLine
                + ", Time: " + Seconds
                ;
        }
    }
}
