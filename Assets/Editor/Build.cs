using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class Build
{

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }

    [MenuItem("File/AutoBuilder/Windows/32-bit")]
    static void PerformWinBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win/" + GetProjectName() + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    [MenuItem("File/AutoBuilder/Windows/64-bit")]
    static void PerformWin64Build()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win64/" + GetProjectName() + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("File/AutoBuilder/Mac OSX/Universal")]
    static void PerformOSXUniversalBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneOSXUniversal);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/OSX-Universal/" + GetProjectName() + ".app", BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
    }

    [MenuItem("File/AutoBuilder/Mac OSX/Intel")]
    static void PerformOSXIntelBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneOSXIntel);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/OSX-Intel/" + GetProjectName() + ".app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
    }

    [MenuItem("File/AutoBuilder/Web/Standard")]
    static void PerformWebBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebGL);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Web", BuildTarget.WebGL, BuildOptions.None);
    }
}