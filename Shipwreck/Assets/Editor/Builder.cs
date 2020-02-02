using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class Builder
{
    static readonly string name = "Shipwreck";
    static readonly string[] levels = new[] {
        "Assets/Scenes/Menu.unity",
        "Assets/Scenes/Game.unity",
        "Assets/Scenes/ResultsScreen.unity"
    };


    [MenuItem("Build/Build All")]
    public static void BuildAll() {
        BuildWin64();
        BuildLin64();
        BuildAndroid();
    }

	[MenuItem("Build/Build Windows 64-bit")]
    public static void BuildWin64()
    {
        var report = BuildPipeline.BuildPlayer(levels, $"Build/Win/{name}.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
		PrintReport(report);
    }

	[MenuItem("Build/Build Linux 64-bit")]
    public static void BuildLin64()
    {
		var report = BuildPipeline.BuildPlayer(levels, $"Build/Linux/{name}.x86_64", BuildTarget.StandaloneLinux64, BuildOptions.None);
		PrintReport(report);
    }

	[MenuItem("Build/Build Android")]
    public static void BuildAndroid()
    {
		var report = BuildPipeline.BuildPlayer(levels, $"Build/Android/{name}.apk", BuildTarget.Android, BuildOptions.None);
		PrintReport(report);
    }

	private static void PrintReport(BuildReport report) {
		if (report.summary.result == BuildResult.Succeeded) {
			Debug.Log($"Build Succeeded {report.summary.totalSize} bytes");
		} else {
			Debug.LogError("Build Failed");
		}
	}
}
