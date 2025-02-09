using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LIVFOVUnlocker;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static Harmony _harmony;
    private static ManualLogSource _log;

    public void Awake()
    {
        _log = Logger;
        _log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll();
    }
}

public static class MyPluginInfo
{
    public const string PLUGIN_GUID = "lostinvivo.fovunlocker";
    public const string PLUGIN_NAME = "Liv Fov Unlocker";
    public const string PLUGIN_VERSION = "0.0.1";
}