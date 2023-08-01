using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Tweaks55.Util;

namespace Tweaks55.HarmonyPatches {
	[HarmonyPatch]
	static class BombSize {
		[HarmonyPriority(int.MaxValue)]
		static void Prefix(BombNoteController ____bombNotePrefab) {
			____bombNotePrefab.transform.localScale = new UnityEngine.Vector3(Config.Instance.bombScale, Config.Instance.bombScale, Config.Instance.bombScale);

			// Need to scale up the collider to account for the smaller base obj
			//foreach(var x in ____bombNotePrefab.GetComponentsInChildren<UnityEngine.SphereCollider>())
			//	x.radius /= Config.Instance.bombScale;
		}

		static MethodBase TargetMethod() => Resolver.GetMethod(nameof(BeatmapObjectsInstaller), "InstallBindings");
		static Exception Cleanup(Exception ex) => Plugin.PatchFailed(ex);
	}
}
