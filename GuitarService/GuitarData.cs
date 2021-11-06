using System;
using System.Collections.Generic;
using System.IO;

namespace GuitarService
{
    public sealed class GuitarData
    {
        private static readonly string STEP_STR = "\"{0}\" : \"{1}\",";
        public static readonly string APP_1_PATH = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\templates\app_1.js");
        public static readonly string APP_2_PATH = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\templates\app_2.js");
        public static readonly string APP_PATH = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\js\app.js");
        public static readonly string LOG_PATH = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\log.txt");
        public static readonly string TAB = "\t";
        public static readonly List<string> TUNINGS_LIST = new List<string> {
            "4. E A D g",
            "4. G C A e",
            "5. B E A D g",
            "6. E A D G B e",
            "6. D A D G B e",
            "6. C G C F A d",
            "6. D G D G B d",
            "6. C# G# C# F# Bb d#",
            "6. D# G# C# F# Bb d#",
            "7. B E A D G B e",
            "7. A E A D G B e",
            "8. F# B E A D G B e",
            "8. G D G C F Bb D g",
            "9. C# F# B E A D G B e"
        };
        public static readonly List<string> STEP_LIST = new List<string> { "I", "II", "III", "IV", "V", "VI", "VII" };
        public static readonly List<string> TONES_LIST = new List<string> { "A", "B_FLAT", "B", "C", "C_SHARP", "D", "D_SHARP", "E", "F", "F_SHARP", "G", "G_SHARP", "A", "B_FLAT", "B", "C", "C_SHARP", "D", "D_SHARP", "E", "F", "F_SHARP", "G", "G_SHARP" };
        public static readonly Dictionary<string, string> TONE_DICT = new Dictionary<string, string> {
            { "A", "A" },
            { "B_FLAT", "A#/Bb" }, // const MAX_NOTE_LEN = 5 !!!
            { "B", "B" },
            { "C", "C" },
            { "C_SHARP", "C#/Db" },
            { "D", "D" },
            { "D_SHARP", "D#/Eb" },
            { "E", "E" },
            { "F", "F" },
            { "F_SHARP", "F#/Gb" },
            { "G", "G" },
            { "G_SHARP", "G#/Ab" }
        };
        public static readonly Dictionary<string, Interval> SCALES_DICT = new Dictionary<string, Interval> {
            // major
            { TAB + "\"{0} 5 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, false, false, false, true, false, false, false, false) },
            { TAB + "\"{0} +5 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, false, false, true, false, false, false) },
            { TAB + "\"{0} 7+5 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, false, false, true, false, true, false) },
            { TAB + "\"{0} 7-5 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, true, false, false, false, true, false) },
            { TAB + "\"{0} 6 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, false, true, false, true, false, false) },
            { TAB + "\"{0} 7 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, false, true, false, false, true, false) },
            { TAB + "\"{0} 9/6 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, false, false, true, false, true, false, false) },
            { TAB + "\"{0} 9 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, false, false, true, false, false, true, false) },
            { TAB + "\"{0} Major Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, false, true, false, false, false, false )},
            { TAB + "\"{0} Slash Major Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, false, true, false, false, false, true, false, false )},
            { TAB + "\"{0} Major 7 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, false, true, false, false, false, true) },
            { TAB + "\"{0} Major 9 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, false, false, true, false, false, false, true) },
            { TAB + "\"{0} Add 2 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, false, false, true, false, false, false, false) },
            { TAB + "\"{0} Aug Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, true, false, false, false, true, false, false, false) },
            { TAB + "\"{0} Sus 2 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, false, false, false, true, false, false, false, false) },
            { TAB + "\"{0} Sus 4 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, false, true, false, true, false, false, false, false) },
            { TAB + "\"{0} Sus 4/7 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, false, false, true, false, true, false, false, true, false) },
            { TAB + "\"{0} Major Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, true, false, true, false, true, false, true )},
            { TAB + "\"{0} Major Pentatonic\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, false, false, true, false, true, false, false)},
            { TAB + "\"{0} Phrygian Dominant Scale\": [{1}]," + Environment.NewLine, new Interval (true, true, false, false, true, true, false, true, true, false, true, false)},
            { TAB + "\"{0} Lydian Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, false, true, true, false, true, false, true)},
            { TAB + "\"{0} Mixolydian Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, false, true, true, false, true, false, true, true, false)},
            { TAB + "\"{0} Arabic Scale\": [{1}]," + Environment.NewLine, new Interval (true, true, false, false, true, true, false, true, true, false, false, true)},
            // minor
            { TAB + "\"{0} Minor Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, false, false, true, false, false, false, false) },
            { TAB + "\"{0} Minor 6 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, false, false, true, false, true, false, false) },
            { TAB + "\"{0} Minor 7 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, false, false, true, false, false, true, false) },
            { TAB + "\"{0} Minor 7-5 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, false, true, false, false, false, true, false) },
            { TAB + "\"{0} Minor 9 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, true, true, false, false, false, true, false, false, true, false) },
            { TAB + "\"{0} Dim Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, false, true, false, false, false, false, false) },
            { TAB + "\"{0} Dim 7 Chord\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, false, true, false, false, false, false, true) },
            { TAB + "\"{0} Blues\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, true, true, true, false, false, true, false )},
            { TAB + "\"{0} Minor Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, true, false, true, false, true, true, false, true, false) },
            { TAB + "\"{0} Minor Pentatonic\": [{1}]," + Environment.NewLine, new Interval (true, false, false, true, false, true, false, true, false, false, true, false )},
            { TAB + "\"{0} Minor Harmonic Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, true, false, true, false, true, true, false, false, true)},
            { TAB + "\"{0} Minor Melodic Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, true, false, true, false, true, false, true, false, true)},
            { TAB + "\"{0} Phrygian Scale\": [{1}]," + Environment.NewLine, new Interval (true, true, false, true, false, true, false, true, true, false, true, false)},
            { TAB + "\"{0} Doric Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, true, false, true, false, true, false, true, true, false)},
            { TAB + "\"{0} Hungarian Scale\": [{1}]," + Environment.NewLine, new Interval (true, false, true, true, false, false, true, true, true, false, false, true)},
        };
        public static readonly List<string> CHORD_MAJOR_STEPS = new List<string> {
            {string.Format(TAB + STEP_STR, STEP_LIST[0], "Major")},
            {string.Format(TAB + STEP_STR, STEP_LIST[1], "Minor")},
            {string.Format(TAB + STEP_STR, STEP_LIST[2], "Minor")},
            {string.Format(TAB + STEP_STR, STEP_LIST[3], "Major")},
             {string.Format(TAB + STEP_STR, STEP_LIST[4], "Major")},
             {string.Format(TAB + STEP_STR, STEP_LIST[5], "Minor")},
             {string.Format(TAB + STEP_STR.Replace(",", ""), STEP_LIST[6], "Dim")},
        };
        public static readonly List<string> CHORD_MINOR_STEPS = new List<string> {
            {string.Format(TAB + STEP_STR, STEP_LIST[0], "Minor")},
            {string.Format(TAB + STEP_STR, STEP_LIST[1], "Dim")},
            {string.Format(TAB + STEP_STR, STEP_LIST[2], "Major")},
            {string.Format(TAB + STEP_STR, STEP_LIST[3], "Minor")},
             {string.Format(TAB + STEP_STR, STEP_LIST[4], "Minor")},
             {string.Format(TAB + STEP_STR, STEP_LIST[5], "Major")},
             {string.Format(TAB + STEP_STR.Replace(",", ""), STEP_LIST[6], "Major")},
        };
        public static readonly List<string> CHORD_HARMONIC_STEPS = new List<string> {
            {string.Format(TAB + STEP_STR, STEP_LIST[0], "Minor")},
            {string.Format(TAB + STEP_STR, STEP_LIST[1], "Dim")},
            {string.Format(TAB + STEP_STR, STEP_LIST[2], "Aug")},
            {string.Format(TAB + STEP_STR, STEP_LIST[3], "Minor")},
             {string.Format(TAB + STEP_STR, STEP_LIST[4], "Major")},
             {string.Format(TAB + STEP_STR, STEP_LIST[5], "Major")},
             {string.Format(TAB + STEP_STR.Replace(",", ""), STEP_LIST[6], "Dim")},
        };
        public static readonly List<string> CHORD_MELODIC_STEPS = new List<string> {
            {string.Format(TAB + STEP_STR, STEP_LIST[0], "Minor")},
            {string.Format(TAB + STEP_STR, STEP_LIST[1], "Minor")},
            {string.Format(TAB + STEP_STR, STEP_LIST[2], "Aug")},
            {string.Format(TAB + STEP_STR, STEP_LIST[3], "Major")},
             {string.Format(TAB + STEP_STR, STEP_LIST[4], "Major")},
             {string.Format(TAB + STEP_STR, STEP_LIST[5], "Dim")},
             {string.Format(TAB + STEP_STR.Replace(",", ""), STEP_LIST[6], "Dim")},
        };
        public static readonly List<string> CHORD_PY_DOM_STEPS = new List<string>
        {
            // ...
        };
        public static readonly List<string> CHORD_LYDIAN_STEPS = new List<string>
        {
            // ...
        };
        public static readonly List<string> CHORD_MIXLYDIAN_STEPS = new List<string>
        {
            // ...
        };
        public static readonly List<string> CHORD_ARABIC_STEPS = new List<string>
        {
            // ...
        };
        public static readonly List<string> CHORD_BLUES_STEPS = new List<string>
        {
            // ...
        };
        public static readonly List<string> CHORD_PY_STEPS = new List<string>
        {
            // ...
        };
        public static readonly List<string> CHORD_DORIC_STEPS = new List<string>
        {
            // ...
        };
        public static readonly List<string> CHORD_HUNGARIAN_STEPS = new List<string>
        {
            // ...
        };

        public static readonly List<string> VALUES_LIST = new List<string> {
            "const MAX_NOTE_LEN = 5;",
            "const FRET_WIRES_ARR = [\"12\", \"18\", \"21\", \"22\", \"24\", \"27\"];",
            string.Format("const STEPS = [\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\"];", STEP_LIST[0], STEP_LIST[1], STEP_LIST[2], STEP_LIST[3], STEP_LIST[4], STEP_LIST[5], STEP_LIST[6]),
        };
    }
}
