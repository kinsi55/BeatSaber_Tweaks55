using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Tweaks55 {
	static class ILUtil {
		public static void DynamicPrefix(int index, ref IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodInfo condition) {
			InsertConditionalIL(index, ref instructions, il, condition, new CodeInstruction[] {
				new CodeInstruction(OpCodes.Ret)
			}, false);
		}

		public static void InsertConditionalIL(int index, ref IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodInfo condition, IEnumerable<CodeInstruction> extraIL) {
			InsertConditionalIL(index, ref instructions, il, condition, extraIL, true);
		}

		private static void InsertConditionalIL(int index, ref IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodInfo condition, IEnumerable<CodeInstruction> extraIL, bool continueOnTrue = true) {
			if(condition.ReturnType != typeof(bool) || !condition.IsStatic)
				throw new Exception("condition must be static bool");

			var label = GetLabelForIndex(index, instructions, il);
			var l = instructions.ToList();

			l.InsertRange(index, new CodeInstruction[] {
				new CodeInstruction(OpCodes.Call, condition),
				new CodeInstruction(continueOnTrue ? OpCodes.Brfalse : OpCodes.Brtrue, label)
			}.Concat(extraIL));

			instructions = l;
		}

		public static Label GetLabelForIndex(int index, IEnumerable<CodeInstruction> instructions, ILGenerator il) {
			var label = il.DefineLabel();

			instructions.ElementAt(index).labels.Add(label);

			return label;
		}

		public static MethodInfo GetIterator<T>(string method) {
			return AccessTools.FirstInner(typeof(T), t => t.Name.StartsWith($"<{method}"))?.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static bool CheckIL(IEnumerable<CodeInstruction> instructions, Dictionary<int, OpCode> confirmations) {
			foreach(var c in confirmations) {
				if(instructions.ElementAt(c.Key).opcode != c.Value)
					return false;
			}
			return true;
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
