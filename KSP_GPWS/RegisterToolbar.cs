
using ToolbarControl_NS;
using UnityEngine;
using KSP_Log;
using KSP_GPWS.Impl;

namespace KSP_GPWS
{

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(GuiToolbarBtn.MODID, GuiToolbarBtn.MODNAME);
        }
    }

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class Statics : MonoBehaviour
    {
        static public Log Log;
        void Awake()
        {
#if DEBUG
            Log = new Log("GPWS", Log.LEVEL.INFO);
#else
            Log = new Log("GPWS", Log.LEVEL.ERROR);
#endif
            Util._audio = new AudioManager();
        }

        bool initted = false;
        void OnGUI()
        {
            if (!initted)
            {
                GUI.skin = HighLogic.Skin;
                SettingGui.ConfigureStyles();
                initted = true;
            }
        }

    }

}
