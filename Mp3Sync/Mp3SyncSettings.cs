using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mp3Sync
{
    public class Mp3SyncSettings
    {
        private string _source;
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        private List<string> _dests;
        public List<string> Dests
        {
            get { return _dests; }
            set { _dests = value; }
        }

        private string _destBin;
        public string DestBin
        {
            get { return _destBin; }
            set { _destBin = value; }
        }

        private string _stExt;

        public string StExt
        {
            get { return _stExt; }
            set { _stExt = value; }
        }

        public string AutoSync { get; set; }

        public Mp3SyncSettings()
        {
            _stExt = ".mp3";
            AutoSync = "folder.jpg";

            Dests = new List<string>();
        }

        public Mp3SyncSettings(bool createDummyInputs)
        {
            AutoSync = "folder.jpg";
            StExt = ".mp3|.jpg";
            Dests = new List<string>();
            DestBin = @"c:\MusicRemovedFromPlayer";
            Source = @"c:\Music";
            Dests.Add(@"d:\audio");
            Dests.Add(@"e:\audio");
        }
    }
}
