using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartLoader.NET.Framework
{
    /// <summary>
    /// This is a temporary object class used to read lines. Later these are discarded to save memory.
    /// </summary>
    public class EventLine
    {

        private long _tick;

        /// <summary>
        /// The current tick.
        /// </summary>
        public long Tick
        {
            get 
            {
                return _tick; 
            }
            set 
            {
                _tick = value; 
            }
        }

        private string _type;

        /// <summary>
        /// The current type.
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        private int _index;

        /// <summary>
        /// The current index.
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        private long _duration;

        /// <summary>
        /// The current duration.
        /// </summary>
        public long Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }

        private string _line;

        /// <summary>
        /// The current text line.
        /// </summary>
        public string Line
        {
            get
            {
                return _line;
            }
            set
            {
                _line = value;
            }
        }

        private string _text;

        /// <summary>
        /// The current text content.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        /// <summary>
        /// The constructor with parameters.
        /// </summary>
        /// <param name="line">Requires the current text line.</param>
        public EventLine(string line)
        {
            _tick = 0;
            _type = "E";
            _duration = 0;
            _index = -1;

            ProcessLine(line);
            _line = line;
        }

        /// <summary>
        /// By using a string line from the external file, this method appplies all data to the correct properties.
        /// </summary>
        /// <param name="line">Using the external file current line.</param>
        public void ProcessLine(string line)
        {

            string[] data;

            string[] attributes;

            line = line.Replace("\t", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);

            data = line.Split(new string[] { " = " }, StringSplitOptions.None);
            if (data.Length > 0)
            {
                _tick = long.Parse(data[0].Replace(" ", string.Empty));


                if (data.Length > 1)
                {
                    attributes = data[1].Split(' ');

                    _type = attributes[0];
                    if (_type.Contains("E"))
                    {
                        _text = data[1].Replace("E ", string.Empty).Replace("section ", string.Empty);
                        if (_text.Length > 1)
                            _text = _text.Substring(1, _text.Length - 2);
                    }
                    else
                    {
                        _index = int.Parse(attributes[1]);

                        if (attributes.Length > 2)
                            _duration = int.Parse(attributes[2]);
                    }
                }
            }
        }

        /// <summary>
        /// Outputs the current details of this reference.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string result;

            result = "EventLine: \n"
                + " Tick: " + _tick
                + ", Type: " + _type
                + ", Index: " + _index
                + ", Duration: " + _duration;

            return result;
        }
    }
}
