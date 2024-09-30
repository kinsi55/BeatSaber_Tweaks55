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

		static Color defaultColor;
		static ColorSO colorTarget;

		static void Prefix(MenuLightsManager __instance) {
			instance = __instance;

			var colorPair = __instance._defaultPreset.lightIdColorPairs.FirstOrDefault(x => x.lightId == 1);
			colorTarget = colorPair?.baseColor;
			defaultColor = colorTarget?.color ?? Color.black;

			SetColor(Config.Instance.menuLightColor, false);
		}

		public static void SetColor(Color color, bool doApply = true) {
			if(instance == null || color == null || !(colorTarget is SimpleColorSO mmmmm))
				return;

			mmmmm.SetColor(color.a == 0 ? defaultColor : color);

			if(doApply)
				instance.RefreshColors();
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(MenuLightsManager), nameof(MenuLightsManager.Start));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
