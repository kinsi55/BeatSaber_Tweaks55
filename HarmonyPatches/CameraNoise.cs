using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Tweaks55.Util;
using UnityEngine;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class CameraNoise {
		static readonly int _globalNoiseTextureID = Shader.PropertyToID("_GlobalBlueNoiseTex");

		static bool lastDisableState = false;

		[HarmonyPriority(int.MaxValue)]
		static bool Prefix() {
			if(!Config.Instance.disableCameraNoise) {
				lastDisableState = false;
				return true;
			}

			if(!lastDisableState) {
				Shader.SetGlobalTexture(_globalNoiseTextureID, null);
				lastDisableState = true;
			}
			return false;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BlueNoiseDitheringUpdater), nameof(BlueNoiseDitheringUpdater.HandleCameraPreRender), assemblyName: "HMRendering");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
