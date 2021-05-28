using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(SettingsFlowCoordinator), nameof(SettingsFlowCoordinator.CancelSettings))]
	static class DisableSettingsResetONCancel {
		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() => !Configuration.PluginConfig.Instance.keepGameSettingsOnCancel;
	}
}
