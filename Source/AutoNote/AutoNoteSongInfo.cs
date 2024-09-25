using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveTracker.Source.AutoNote.MusicTheory;
using WaveTracker.Tracker;

namespace WaveTracker.Source.AutoNote; 

/// <summary>
/// The feature should be able to persist data about songs and collect information in the background
/// so that way uneccessary recomputations can be avoided
/// </summary>
public class AutoNoteSongInfo {
    public WTSong WTSong { get; private set; }
    public Scale PerceivedScale { get; private set; }
    public AutoNoteSongInfo(WTSong parent) {
        WTSong = parent;
    }
}
