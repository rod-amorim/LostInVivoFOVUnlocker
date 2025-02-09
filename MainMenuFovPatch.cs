using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoGlitches;

namespace LIVFOVUnlocker;

[HarmonyPatch(typeof(NEW_MENU_SCR), "Slide")]
public class MainMenuFovPatch
{
    private static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("FOVPatchMenu");

    static bool Prefix(ref float value_to_add, NEW_MENU_SCR __instance)
    {
        if (__instance.State != 3)
        {
            return true;
        }

        if (__instance.Slot == 0)
        {
            if (value_to_add > 90f)
            {
                __instance.Sliders[0].GetComponent<Slider>().value = 1f;
            }
            else
            {
                value_to_add *= 0.01f;
                __instance.Sliders[0].GetComponent<Slider>().value += value_to_add;
            }

            FieldInfo tempGammaField = AccessTools.Field(typeof(NEW_MENU_SCR), "temp_GAMMA");

            tempGammaField.SetValue(__instance, __instance.Sliders[0].GetComponent<Slider>().value);

            // temp_GAMMA = __instance.Sliders[0].GetComponent<Slider>().value;

            PlayerPrefs.SetFloat("GAMMA", (float) tempGammaField.GetValue(__instance));
            __instance.cam.gameObject.GetComponent<ImageEffectBase>().gamma =
                (float) tempGammaField.GetValue(__instance);
        }
        else if (__instance.Slot == 1)
        {
            if (value_to_add > 90f)
            {
                __instance.Sliders[1].GetComponent<Slider>().value = 1f;
            }
            else
            {
                value_to_add *= 0.01f;
                __instance.Sliders[1].GetComponent<Slider>().value += value_to_add;
            }

            FieldInfo tempVolField = AccessTools.Field(typeof(NEW_MENU_SCR), "temp_VOL");

            tempVolField.SetValue(__instance, __instance.Sliders[1].GetComponent<Slider>().value);
            // temp_VOL = __instance.Sliders[1].GetComponent<Slider>().value;

            PlayerPrefs.SetFloat("Volume", (float) tempVolField.GetValue(__instance));
            AudioListener.volume = (float) tempVolField.GetValue(__instance);
        }
        else if (__instance.Slot == 2)
        {
            FieldInfo tempFovField = AccessTools.Field(typeof(NEW_MENU_SCR), "temp_FOV");

            if (value_to_add > 90f)
            {
                __instance.Sliders[2].GetComponent<Slider>().value = 45f;
            }
            else
            {
                value_to_add *= 0.18f;
                __instance.Sliders[2].GetComponent<Slider>().value += value_to_add;
            }

            var fovSlider = __instance.Sliders[2].GetComponent<Slider>();

            if (fovSlider != null)
            {
                fovSlider.maxValue = 120;
            }

            tempFovField.SetValue(__instance, Mathf.RoundToInt(__instance.Sliders[2].GetComponent<Slider>().value));

            if ((float) tempFovField.GetValue(__instance) > 45f)
            {
                __instance.Sliders[3].GetComponent<TextMeshPro>().text =
                    "The low FOV is to invoke feelings of claustrophobia. Please only change if you suffer from motion sickness.";
            }
            else
            {
                __instance.Sliders[3].GetComponent<TextMeshPro>().text = string.Empty;
            }

            PlayerPrefs.SetFloat("FOV", (float) tempFovField.GetValue(__instance));
            __instance.State3_OBJ[2].GetComponent<TextMeshPro>().text =
                "FOV [" + (float) tempFovField.GetValue(__instance) + "]";
        }

        return false; // Impede a execução do método original
    }
}