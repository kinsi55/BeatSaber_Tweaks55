using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class DisableSettingsResetONCancel {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Config.Instance.keepGameSettingsOnCancel;

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(SettingsFlowCoordinator), nameof(SettingsFlowCoordinator.CancelSettings));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
