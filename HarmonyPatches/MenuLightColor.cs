using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch(typeof(MenuLightsManager), nameof(MenuLightsManager.Start))]
	static class MenuLightColor {
		static MenuLightsManager instance;
		static MenuLightsPresetSO _defaultPreset;

		static Color defaultColor;

		static void Prefix(MenuLightsManager __instance, MenuLightsPresetSO ____defaultPreset) {
			instance = __instance;
			_defaultPreset = ____defaultPreset;

			defaultColor = _defaultPreset.playersPlaceNeonsColor;

			SetColor(Config.Instance.menuLightColor, false);
		}

		public static void SetColor(Color color, bool doApply = true) {
			if(instance == null || _defaultPreset == null || !(_defaultPreset.playersPlaceNeonsColor is SimpleColorSO mmmmm))
				return;

			if(Config.Instance.menuLightColor.a == 0)
				color = defaultColor;
				
			mmmmm.SetColor(color);

			if(doApply) {
				instance.RefreshLightsDictForPreset(_defaultPreset);
				instance.SetColorsFromPreset(_defaultPreset, 1f);
			}
		}

		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
