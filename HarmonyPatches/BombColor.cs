using HarmonyLib;
using IPA.Utilities;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BombColor {
		internal static readonly Color defaultColor = Color.black.ColorWithAlpha(0);

		static readonly int _SimpleColor = Shader.PropertyToID("_SimpleColor");

		static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor BombNoteController_material0
			= FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material0");

		static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor BombNoteController_material1
			= FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material1");

		static Color? basegameBombaColor0 = null;
		static Color? basegameBombaColor1 = null;

		[HarmonyPriority(int.MaxValue)]
		static void Postfix(BombNoteController ____bombNotePrefab) {
			var cms = ____bombNotePrefab
				.GetComponentInChildren<ConditionalMaterialSwitcher>();

			if(cms == null)
				return;

			basegameBombaColor0 ??= BombNoteController_material0(ref cms)?.GetColor(_SimpleColor);
			basegameBombaColor1 ??= BombNoteController_material1(ref cms)?.GetColor(_SimpleColor);

			var isDefaultColor = Config.Instance.bombColor == defaultColor;

			BombNoteController_material0(ref cms)?.SetColor(_SimpleColor, isDefaultColor ? (basegameBombaColor0 ?? default) : Config.Instance.bombColor);
			BombNoteController_material1(ref cms)?.SetColor(_SimpleColor, isDefaultColor ? (basegameBombaColor1 ?? default) : Config.Instance.bombColor);
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapObjectsInstaller), "InstallBindings");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
