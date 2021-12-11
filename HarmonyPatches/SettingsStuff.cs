using HarmonyLib;
using System;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(SettingsFlowCoordinator), nameof(SettingsFlowCoordinator.CancelSettings))]
	static class DisableSettingsResetONCancel {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.keepGameSettingsOnCancel;

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed("DisableSettingsResetONCancel", ex);
	}
}
