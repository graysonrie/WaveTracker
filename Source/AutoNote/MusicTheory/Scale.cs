using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTracker.Source.AutoNote.MusicTheory;
public class Scale {
    public enum MajorMinor {
        Major,
        Minor
    }
    /// <summary>
    /// The <c>key</c> parameter must be a byte between 0 and 11, representing musical notes as follows:
    /// 0 - C, 1 - C#, 2 - D, 3 - D#, 4 - E, 5 - F, 6 - F#, 
    /// 7 - G, 8 - G#, 9 - A, 10 - A#, 11 - B.
    /// </summary>
    /// <param name="key"></param>
    /// <returns>A byte array of the 7 notes in a scale</returns>
    public static int[] AsNumbers(int key, MajorMinor mm) {
        int offset = mm switch {
            MajorMinor.Major => 9,
            _ => 0
        };
        key += offset;
        key %= 12;
        return [key, key + 2, key + 3, key + 5, key + 7, key + 8, key + 10];
    }
}
