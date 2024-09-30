using HarmonyLib;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;
using System.Linq;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class MenuLightColor {
		static MenuLightsManager instance;

		static LightIdColorPair colorPair;
		static ColorSO defaultColor;

		static void Prefix(MenuLightsManager __instance) {
			instance = __instance;

			colorPair = __instance._defaultPreset.lightIdColorPairs.FirstOrDefault(x => x.lightId == 1);
			defaultColor = colorPair?.baseColor;

			SetColor(Config.Instance.menuLightColor, false);
		}

		public static void SetColor(Color color, bool doApply = true) {
			if(instance == null || colorPair == null || !(defaultColor is SimpleColorSO mmmmm))
				return;

			if(Config.Instance.menuLightColor.a == 0)
				colorPair.baseColor = defaultColor;

			mmmmm.SetColor(color);

			if(doApply)
				instance.RefreshColors();
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(MenuLightsManager), nameof(MenuLightsManager.Start));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
