using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WaveTracker.Tracker;
namespace WaveTracker.Source.AutoNote;

/// <summary>
/// Allow the analyzer to run in the background
/// </summary>
internal class Dispatcher {
    public AutoNoteSongInfo SongAnalyzing { get; set; }
    public CursorPos cursorPosRef;

    List<PhraseIdea> PhraseIdeas { get; set; }
    /// <summary>
    /// Background thread that gathers up details about the song in order to offer predictions. Should be wrapped in a try/catch because it uses a cancellation token.
    /// <para>
    /// The cancellation token should ONLY be called when switching to a new song. Otherwise, the dispatcher will constantly get interuppted.
    /// </para>
    /// </summary>
    /// <param name="song"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task AnalyzeSongAsync(AutoNoteSongInfo song, CancellationToken token) {
        Console.WriteLine("Dispatcher has been invoked");
        SongAnalyzing = song;
        token.ThrowIfCancellationRequested();

        // Attempt to figure out what key the song is in
        if (SongAnalyzing.PerceivedScale == null) {
            await PredictSongScaleAsync();
        }
        
    }
    public async Task PredictSongScaleAsync() {
        WTModule parentModule = SongAnalyzing.WTSong.ParentModule;

        List<int[]> patternNotes = [];
        foreach (var pattern in SongAnalyzing.WTSong.Patterns) {
            for (int r = 0; r < pattern.Height; r++) { // iterate over every row
                for (int c = 0; c < parentModule.ChannelCount; c++) { // iterate over every channel
                    int note = pattern[r, c, CellType.Note];
                }
            }
        }
    }
}
