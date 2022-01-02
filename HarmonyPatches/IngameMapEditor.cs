using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {

	[HarmonyPatch]
	static class DisableBeatmapEditor {
		const int BEATMAP_EDITOR = (int)MainMenuViewController.MenuButton.BeatmapEditor;

		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(int menuButton) =>
			menuButton != BEATMAP_EDITOR || !Config.Instance.disableIngameMapEditor;

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(MainMenuViewController), nameof(MainMenuViewController.HandleMenuButton));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
