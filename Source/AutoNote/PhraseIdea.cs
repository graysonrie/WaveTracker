using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveTracker.Tracker;
namespace WaveTracker.Source.AutoNote; 

/// <summary>
/// A <c>PhraseIdea</c> just corresponds to a Pattern
/// </summary>
internal class PhraseIdea {
    public WTPattern ParentPattern { get; private set; }
    public byte[][] Data { get; set; }
    public PhraseIdea(WTPattern parent) {
        ParentPattern = parent;
    }
}
