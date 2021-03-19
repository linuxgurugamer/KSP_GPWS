// GPWS mod for KSP
// License: CC-BY-NC-SA
// Author: bss, 2015

using UnityEngine;
using ToolbarControl_NS;

namespace KSP_GPWS
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class GuiToolbarBtn : MonoBehaviour
    {
        ToolbarControl toolbarControl;


        internal const string MODNAME = "GPWS";
        internal const string MODID = "GPWS";
        public void Awake()
        {
            if (toolbarControl == null)
            {
                GameObject gameObject = new GameObject();
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(ToggleOn, ToggleOff,
                    KSP.UI.Screens.ApplicationLauncher.AppScenes.FLIGHT,
                    MODID,
                    "GPWSBtn",
                    "GPWS/PluginData/gpws",
                    "GPWS/PluginData/gpws",
                    MODNAME
                );
            }

            void ToggleOn()
            {
                SettingGui.toggleSettingGui(true);
                //appBtn.SetFalse(false);
            }
            void ToggleOff()
            {
                SettingGui.toggleSettingGui(false);
                //appBtn.SetFalse(false);
            }

        }

        public void OnDestroy()
        {
            if (toolbarControl != null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
            }
        }
    }
}
