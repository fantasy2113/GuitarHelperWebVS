using System;
using System.Collections.Generic;
using GuitarService;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GuitarHelperWebVS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                List<BaseDataClass> baseDataList = new List<BaseDataClass> {
                    new ValueData(),
                    new NotesData("const"),
                    new ChordStepsData("const MAJOR_STEPS_DICT = {", GuitarData.CHORD_MAJOR_STEPS),
                    new ChordStepsData("const MINOR_STEPS_DICT = {", GuitarData.CHORD_MINOR_STEPS),
                    new ChordStepsData("const HARMONIC_STEPS_DICT = {", GuitarData.CHORD_HARMONIC_STEPS),
                    new ChordStepsData("const MELODIC_STEPS_DICT = {", GuitarData.CHORD_MELODIC_STEPS),
                    new ChordStepsData("const PY_DOM_STEPS_DICT = {", GuitarData.CHORD_PY_DOM_STEPS),
                    new ChordStepsData("const PY_STEPS_DICT = {", GuitarData.CHORD_PY_STEPS),
                    new ChordStepsData("const MIXLYDIAN_STEPS_DICT = {", GuitarData.CHORD_MIXLYDIAN_STEPS),
                    new ChordStepsData("const LYDIAN_STEPS_DICT = {", GuitarData.CHORD_LYDIAN_STEPS),
                    new ChordStepsData("const DORIC_STEPS_DICT = {", GuitarData.CHORD_DORIC_STEPS),
                    new ChordStepsData("const HUNGARIAN_STEPS_DICT = {", GuitarData.CHORD_HUNGARIAN_STEPS),
                    new ChordStepsData("const ARABIC_STEPS_DICT = {", GuitarData.CHORD_HUNGARIAN_STEPS),
                    new ChordStepsData("const BLUES_STEPS_DICT = {", GuitarData.CHORD_BLUES_STEPS),
                    new NotesListData("const TONES_ARR = [", string.Empty),
                    new NotesListData("const KEYTONES_ARR = [", "\"No keytone\""),
                    new ScaleData("const SCALES_MAP = {", "\"No scale\": [],"),
                    new TuningData("const TUNING_MAP = {")
                };

                CreateJSAppFile generateAppFile = new CreateJSAppFile(baseDataList);
                generateAppFile.CreateAppFile();
                generateAppFile = null;
                baseDataList = null;
            }
            catch (Exception ex)
            {
                IDataLogger dataLogger = DataLogger.GetInstance;
                dataLogger.WriteMsg(ex.ToString());
            }
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
