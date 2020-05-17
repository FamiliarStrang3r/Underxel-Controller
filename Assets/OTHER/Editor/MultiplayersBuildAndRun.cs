using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MultiplayersBuildAndRun
{
    //[MenuItem("File/Run Multiplayer/2 Players (Win)")]
    private static void PerformWin64Build2() => PerformWin64Build(2);

    //[MenuItem("File/Run Multiplayer/Windows/3 Players")]
    //private static void PerformWin64Build3() => PerformWin64Build(3);

    //[MenuItem("File/Run Multiplayer/Windows/4 Players")]
    //private static void PerformWin64Build4() => PerformWin64Build(4);

    private static void PerformWin64Build(int playerCount)
    {
        //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        string[] scenePaths = GetScenePaths();
        //foreach (var p in scenePaths) Debug.Log(p);

        for (int i = 1; i <= playerCount; i++)
        {
            //BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win64/" + GetProjectName() + i.ToString() + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);

            string p2 = "Builds/Win64/" + i.ToString() + "/" + GetProjectName() + i.ToString() + ".exe";

            //Debug.Log(p2);
            BuildPipeline.BuildPlayer(scenePaths, p2, BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
        }
    }

    //[MenuItem("File/Run Multiplayer/Mac OSX/2 Players")]
    //private static void PerformOSXBuild2() => PerformOSXBuild(2);

    //[MenuItem("File/Run Multiplayer/Mac OSX/3 Players")]
    //private static void PerformOSXBuild3() => PerformOSXBuild(3);

    //[MenuItem("File/Run Multiplayer/Mac OSX/4 Players")]
    //private static void PerformOSXBuild4() => PerformOSXBuild(4);

    static void PerformOSXBuild(int playerCount)
    {
        //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneOSXUniversal);
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
        for (int i = 1; i <= playerCount; i++)
        {
            //BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/OSX/" + GetProjectName() + i.ToString() + ".app", BuildTarget.StandaloneOSXUniversal, BuildOptions.AutoRunPlayer);
            ////BuildPipeline.BuildPlayer(
            ////    GetScenePaths(),
            ////    "Builds/OSX/" + GetProjectName() + i.ToString() + ".app",
            ////    BuildTarget.StandaloneOSX, BuildOptions.AutoRunPlayer);

            string[] p1 = GetScenePaths();
            string p2 = "Builds/OSX/" + GetProjectName() + i.ToString() + ".app";
            BuildPipeline.BuildPlayer(p1, p2, BuildTarget.StandaloneOSX, BuildOptions.AutoRunPlayer);
        }
    }

    private static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    private static string[] GetScenePaths()
    {
        //string[] scenes = new string[EditorBuildSettings.scenes.Length];
        List<string> scenes = new List<string>();
        int length = EditorBuildSettings.scenes.Length;

        for (int i = 0; i < length; i++)
        {
            string p = EditorBuildSettings.scenes[i].path;

            if (p.Length > 0) scenes.Add(p);
        }

        return scenes.ToArray();
    }
}