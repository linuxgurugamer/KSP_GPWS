﻿// GPWS mod for KSP
// License: CC-BY-NC-SA
// Author: bss, 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP_GPWS.Interfaces;

namespace KSP_GPWS.Impl
{
    public class GPWSLander : ILanderConfig, IBasicGPWSFunction
    {
        private IGPWSCommonData CommonData = null;

        #region ILanderConfig

        #endregion

        #region IConfigNode Members

        public void Load(ConfigNode node)
        {
            throw new NotImplementedException();
        }

        public void Save(ConfigNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void InitializeConfig()
        {
        }

        public void Initialize(IGPWSCommonData data)
        {
            CommonData = data;
        }

        public bool PreUpdate()
        {
            // on surface
            if (CommonData.ActiveVessel.Landed || CommonData.ActiveVessel.Splashed)
            {
                Util.audio.MarkNotPlaying();
                return false;
            }
            return true;
        }

        public void UpdateGPWS()
        {
        }

        public void SetVesselInfo(Vessel v)
        {
        }
    }
}