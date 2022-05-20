using HarmonyLib;
using IPA.Utilities;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BombColor {
		static readonly int _SimpleColor = Shader.PropertyToID("_SimpleColor");

		static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor BombNoteController_material0
			= FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material0");

		static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor BombNoteController_material1
			= FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material1");


		[HarmonyPriority(int.MaxValue)]
		static void Postfix(BombNoteController ____bombNotePrefab) {
			var cms = ____bombNotePrefab
				.GetComponentInChildren<ConditionalMaterialSwitcher>();

			if(cms == null)
				return;

			BombNoteController_material0(ref cms)?.SetColor(_SimpleColor, Config.Instance.bombColor);
			BombNoteController_material1(ref cms)?.SetColor(_SimpleColor, Config.Instance.bombColor);
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapObjectsInstaller), "InstallBindings");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
