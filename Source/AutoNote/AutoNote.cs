using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WaveTracker.Tracker;

namespace WaveTracker.Source.AutoNote; 

// TODO: remove all of the Console.WriteLines when done

/// <summary>
/// Can be broken up into two parts:
/// 
/// <para>
/// <c>Dispatcher:</c> purpose is to gather information about the song and come up with ideas
/// </para>
/// <c>AutoNote:</c> purpose is to relay the Dispatcher's information (if it has anything good) back to the pattern editor whenever the pattern editor invokes an update on <c>AutoNote</c>
/// </summary>
public partial class AutoNote {
    /// <summary>
    /// How long the program will wait to display an autocomplete if the user has been idling for a certain amount of time. In other words, the <c>copilot pause</c>
    /// </summary>
    const int MAX_USER_INACTIVITY_TICKS = 10000;
    int userInactivityTimer = MAX_USER_INACTIVITY_TICKS;
    /// <summary>
    /// True if <c>AutoNote</c> is toggled on
    /// </summary>
    public bool Enabled { get; private set; } = true;
    CancellationTokenSource cts;
    Dispatcher dispatcher = new();
    public List<AutoNoteSongInfo> Songs { get; private set; } = [];
    public AutoNote() {
        Console.WriteLine("Autonote is active");
    }

    void NewCancellationToken() {
        cts?.Cancel();
        cts = new CancellationTokenSource(); 
    }
    public void AnalyzeSong(WTSong song) {
        // If it is analyzing the current song, don't attempt to dispatch it again
        
        if ( (dispatcher.SongAnalyzing != null && Songs.Contains(dispatcher.SongAnalyzing)) || !Enabled) 
            return;

        NewCancellationToken();

        AutoNoteSongInfo NewInfo() {
            AutoNoteSongInfo info = new(song);
            Songs.Add(info);
            return info;
        }

        Task.Run(async () => {
            try {
                var info = Songs.FirstOrDefault(x => {
                    return x.WTSong == song;
                }) ?? NewInfo();
                await dispatcher.AnalyzeSongAsync(info, cts.Token);
            } catch (OperationCanceledException){
                Console.WriteLine("dispatcher cancelled");
            }
        });
    }

    public void IdleTimerTick() {
        if(!Enabled) 
            return;
        if (userInactivityTimer > 0) {
            userInactivityTimer--;
            if(userInactivityTimer == 0) {
                AskForAutocomplete();
                // Reset the timer after the user becomes active again
                // For now, just refresh it here
                userInactivityTimer = MAX_USER_INACTIVITY_TICKS;
            }
        }
    }

    public void AnalyzeCurrentSong(CursorPos cursorPosRef) {
        dispatcher.cursorPosRef = cursorPosRef;
        AnalyzeSong(App.CurrentSong);
    }

    public void AskForAutocomplete() {
        Console.WriteLine("Attempting to get autocomplete");
    }

    public void GetIdeas(int frame, int row, int channel) {
        //int autoNoteValue = App.CurrentSong.Patterns[frame].GetAutoNoteCell(row, channel, CellType.Note);
        var pattern = App.CurrentSong.Patterns[frame];
        //byte[][] cells = pattern.AutoNoteCells;
        // 12 = C-0
        pattern.SetAutoNoteCell(0, 1, CellType.Note, 12);
    }
}