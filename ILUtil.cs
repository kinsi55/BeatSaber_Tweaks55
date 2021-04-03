using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Tweaks55 {
	static class ILUtil {
		public static void ContinueOnCondition(int index, ref IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodInfo condition, int continueOffset = 0) {
			if(condition.ReturnType != typeof(bool) || !condition.IsStatic)
				throw new Exception("condition must be static bool");

			var l = instructions.ToList();

			var label = il.DefineLabel();

			l[index + continueOffset].labels.Add(label);

			l.InsertRange(index, new CodeInstruction[] {
				new CodeInstruction(OpCodes.Call, condition),
				new CodeInstruction(OpCodes.Brtrue, label),
				new CodeInstruction(OpCodes.Ret)
			});

			instructions = l;
		}
		public static void InsertFn(int index, ref IEnumerable<CodeInstruction> instructions, MethodInfo fn) {
			if(fn.ReturnType != typeof(void) || !fn.IsStatic)
				throw new Exception("FN must be static void");

			var l = instructions.ToList();

			l.Insert(index, new CodeInstruction(OpCodes.Call, fn));

			instructions = l;
		}
	}
}
