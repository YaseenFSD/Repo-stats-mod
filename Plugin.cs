using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace StatsModPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }
}

public static class PlayerControllerReference
{
    public static PlayerController Instance { get; set; }
}

[HarmonyPatch(typeof(PlayerController))]
[HarmonyPatch("FixedUpdate")]
class PatchInfiniteSprint
{
    static void Prefix(PlayerController __instance,ref float ___EnergySprintDrain) 
    {
        PlayerControllerReference.Instance = __instance;
        ___EnergySprintDrain = 0;
    }
    
}

[HarmonyPatch(typeof(PlayerAvatar))]
[HarmonyPatch("Slide")]
class PatchInfiniteSprint2
{
    static void Prefix()
    {
        var playerController = PlayerControllerReference.Instance;
        if (playerController != null)
        {
            playerController.EnergyCurrent += 5f;
        }
    }
}