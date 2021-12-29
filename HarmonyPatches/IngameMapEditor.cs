using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {

	[HarmonyPatch(typeof(MainMenuViewController), nameof(MainMenuViewController.HandleMenuButton))]
	static class DisableBeatmapEditor {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix(MainMenuViewController.MenuButton menuButton) =>
			menuButton != MainMenuViewController.MenuButton.BeatmapEditor || !Config.Instance.disableIngameMapEditor;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
