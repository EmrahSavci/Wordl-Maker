using UnityEngine;
using Lofelt.NiceVibrations;

public static class Haptics
{
    public static bool hapticOff = false;
    public static void PlayPreset(this HapticType hapticPatterns)
    {
        if(hapticOff) return;

        switch (hapticPatterns)
        {
            case HapticType.Light:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                break;
            case HapticType.Medium:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                break;
            case HapticType.Heavy:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
                break;
            case HapticType.Success:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
                break;
            case HapticType.Fail:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
                break;
        }

        Debug.Log("Haptic Playing" + hapticPatterns.ToString());
    }
}

public enum HapticType { Light, Medium, Heavy, Fail, Success }