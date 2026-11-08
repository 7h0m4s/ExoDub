using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ExoDub.Utility
{
    public static class SettingsUtility
    {
        public static Selectable CreateButton(ref SettingsMenu __instance, ConfigEntry<bool> settingValue, Selectable aboveButton, string trueText, string falseText, SettingsGroup currentGroup)
        {
            var addButtonMethod = AccessTools.Method(typeof(SettingsMenu), "AddButton");
            var connectNavigationMethod = AccessTools.Method(typeof(SettingsMenu), "ConnectNavigation");
            var text = settingValue.Value ? trueText : falseText;
            // Original: NWButton button = AddButton(text, null);
            NWButton button = (NWButton)addButtonMethod.Invoke(__instance, new object[] { text, null, null });
            Action listener2 = delegate
            {
                button.text = settingValue.Value ? trueText : falseText;
                settingValue.BoxedValue = !settingValue.Value;
                SettingsMenu.instance.UpdateButtons(currentGroup);
            };
            button.onClick.ReplaceListener(delegate
            {
                listener2();
            });

            connectNavigationMethod.Invoke(__instance, new object[] { aboveButton, button });
            return button;
        }

        public static Selectable CreateVoicePackSelectDropdown(ref SettingsMenu __instance, string settingName, ConfigEntry<string> settingValue, Selectable aboveButton)
        {
            DubAudioEngine.LoadVoicePackDirectoryList();
            var connectNavigationMethod = AccessTools.Method(typeof(SettingsMenu), "ConnectNavigation");
            SettingsDropdown dropdown = SettingsMenu.instance.dropdownPrefab.Spawn(SettingsMenu.instance.buttonsContainer);
            dropdown.label = settingName;
            List<object> objectList = new List<object>();
            objectList.AddRange(DubState.VoicePackDirectoryList);
            dropdown.SetOptions(objectList, DubState.VoicePackDirectoryList);
            dropdown.SetSelectedValue(settingValue.Value, fireListeners: false);
            dropdown.listener = delegate
            {
                settingValue.BoxedValue = dropdown.selectedValue;
                
                DubAudioEngine.LoadFileList();
                DubAudioEngine.LoadVoicePackDirectoryList();
                if (DubState.VoicePackDirectoryList.All(directoryName => directoryName != settingValue.Value))
                {
                    // Reset to Default Voice Pack if selected pack doesn't exist
                    settingValue.BoxedValue = DubState.Constants.DefaultVoicePack;
                }
            };
            var selectableDropdown = dropdown.GetComponentInChildren<NWDropdown>();
            connectNavigationMethod.Invoke(__instance, new object[] { aboveButton, selectableDropdown });
            return selectableDropdown;
        }

        public static Selectable CreateSlider(ref SettingsMenu __instance, string settingName, ConfigEntry<float> settingValue, Selectable aboveButton)
        {
            var addFillbarMethod = AccessTools.Method(typeof(SettingsMenu), "AddFillbar");
            var connectNavigationMethod = AccessTools.Method(typeof(SettingsMenu), "ConnectNavigation");
            
            float value = NWUtils.ReflectionGetValue<float>(Settings.instance, settingName);
            UnityAction<float> listener = delegate(float num)
            {
                settingValue.BoxedValue = num;
            };
            // Original: FillbarDraggableSlider fillbarDraggableSlider = AddFillbar(text, value, listener);
            FillbarDraggableSlider fillbarDraggableSlider = (FillbarDraggableSlider)addFillbarMethod.Invoke(__instance, new object[] { settingName, settingValue.Value, listener });
            // Original: ConnectNavigation(aboveButton, fillbarDraggableSlider);
            connectNavigationMethod.Invoke(__instance, new object[] { aboveButton, fillbarDraggableSlider });
            return fillbarDraggableSlider;
        }
    }
}