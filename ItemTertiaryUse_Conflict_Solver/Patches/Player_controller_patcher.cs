using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItemTertiaryUse_Conflict_Solver.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class Player_controller_patcher
    {
        [HarmonyPatch("ItemTertiaryUse_performed")]
        [HarmonyPrefix]
        static bool patch_bad_interactions(ref InteractTrigger ___hoveringOverTrigger)
        {
            if (___hoveringOverTrigger != null)
            {
                string interact_key = "interact_default", use_key = "use_default";
                foreach (var binding in IngamePlayerSettings.Instance.playerInput.actions.FindAction("Interact").bindings)
                {
                    if (binding.path.Contains("Keyboard"))
                    {
                        interact_key = binding.overridePath;
                    }
                }
                foreach (var binding in IngamePlayerSettings.Instance.playerInput.actions.FindAction("ItemTertiaryUse").bindings)
                {
                    if (binding.path.Contains("Keyboard"))
                    {
                        use_key = binding.overridePath;
                    }
                }
                ItemTertiaryUse_conflict_solver_base.LogInfo("keybind comparison: " + interact_key + ", " + use_key);
                if (interact_key == use_key)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
