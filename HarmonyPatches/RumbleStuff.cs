using HarmonyLib;
using Libraries.HM.HMLib.VR;
using System;
using System.Reflection;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class CutRumble {
		public const float STRENGTH_NORMAL = 1f;
		public const float DURATION_NORMAL = 0.14f;

		public const float STRENGTH_WEAK = 0.75f;
		public const float DURATION_WEAK = 0.07f;

		public static readonly HapticPresetSO normalPreset = ScriptableObject.CreateInstance<HapticPresetSO>();
		public static readonly HapticPresetSO weakPreset = ScriptableObject.CreateInstance<HapticPresetSO>();

		[HarmonyPriority(int.MinValue)]
		static bool Prefix(HapticFeedbackManager ____hapticFeedbackManager, SaberType saberType, NoteCutHapticEffect.Type type) {
			if(!Config.Instance.enableCustomRumble)
				return true;

			if(type == NoteCutHapticEffect.Type.ShortWeak) {
				if(weakPreset._duration == 0f || weakPreset._strength == 0f)
					return false;

				____hapticFeedbackManager.PlayHapticFeedback(saberType.Node(), weakPreset);
			} else {
				if(normalPreset._duration == 0f || normalPreset._strength == 0f)
					return false;

				____hapticFeedbackManager.PlayHapticFeedback(saberType.Node(), normalPreset);
			}

			return false;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(NoteCutHapticEffect), nameof(NoteCutHapticEffect.HitNote));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}

	[HarmonyPatch]
	static class ArcRumble {
		public const float STRENGTH_NORMAL = 0.75f;
		public const float DURATION_NORMAL = 0.01f;

		public static readonly HapticPresetSO preset = ScriptableObject.CreateInstance<HapticPresetSO>();

		static void Prepare() {
			preset._continuous = true;
		}

		[HarmonyPriority(int.MinValue)]
		static void Prefix(ref HapticPresetSO ____hapticPreset) {
			if(Config.Instance.enableCustomRumble)
				____hapticPreset = preset;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(SliderHapticFeedbackInteractionEffect), nameof(SliderHapticFeedbackInteractionEffect.Vibrate));
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
