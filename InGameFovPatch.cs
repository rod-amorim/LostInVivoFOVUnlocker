using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoGlitches;

namespace LIVFOVUnlocker;

[HarmonyPatch(typeof(Ironsights), "Start")]
public class InGameFovPatch
{
    private static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("FOVPatchInGame");

    static bool Prefix(Ironsights __instance)
    {
        Log.LogInfo("Running in game fov unlocker");
        __instance.SmoothMouseLook = __instance.CameraObj.GetComponent<SmoothMouseLook>();

        //PlayerWeaponsComponent
        // __instance.PlayerWeaponsComponent = __instance.weaponObj.GetComponent<PlayerWeapons>();
        FieldInfo playerWeaponComponentFieldInfo = AccessTools.Field(typeof(Ironsights), "PlayerWeaponsComponent");
        playerWeaponComponentFieldInfo.SetValue(__instance, __instance.weaponObj.GetComponent<PlayerWeapons>());

        //FPSWalker
        // __instance.FPSWalker = __instance.playerObj.GetComponent<FPSRigidBodyWalker>();
        FieldInfo fpsWalkerFieldInfo = AccessTools.Field(typeof(Ironsights), "FPSWalker");
        fpsWalkerFieldInfo.SetValue(__instance, __instance.playerObj.GetComponent<FPSRigidBodyWalker>());

        __instance.VerticalBob = __instance.playerObj.GetComponent<VerticalBob>();
        __instance.HorizontalBob = __instance.playerObj.GetComponent<HorizontalBob>();

        //FPSPlayer
        // __instance.FPSPlayerComponent = __instance.playerObj.GetComponent<FPSPlayer>();
        FieldInfo fpsPlayerComponent = AccessTools.Field(typeof(Ironsights), "FPSPlayerComponent");
        fpsPlayerComponent.SetValue(__instance, __instance.playerObj.GetComponent<FPSPlayer>());

        //InputControl
        // __instance.InputComponent = __instance.playerObj.GetComponent<InputControl>();
        FieldInfo inputComponent = AccessTools.Field(typeof(Ironsights), "InputComponent");
        inputComponent.SetValue(__instance, __instance.playerObj.GetComponent<InputControl>());

        //set the WeaponPivotComponent from the fpsPlayerComponent
        // __instance.WeaponPivotComponent = __instance.FPSPlayerComponent.WeaponPivotComponent;
        var fpsPlayer = (FPSPlayer) fpsPlayerComponent.GetValue(__instance);
        __instance.WeaponPivotComponent = fpsPlayer.WeaponPivotComponent;
        
        //aSource
        FieldInfo aSourceFieldInfo = AccessTools.Field(typeof(Ironsights), "aSource");
        var aSource = (AudioSource) aSourceFieldInfo.GetValue(__instance);

        aSource = __instance.playerObj.AddComponent<AudioSource>();
        aSource.spatialBlend = 0f;
        aSource.playOnAwake = false;

        __instance.defaultFov = PlayerPrefs.GetFloat("FOV", 45f);
        __instance.sprintFov = __instance.defaultFov;
        __instance.nextFov = __instance.defaultFov;
        __instance.newFov = __instance.defaultFov;
        return false;
    }
}