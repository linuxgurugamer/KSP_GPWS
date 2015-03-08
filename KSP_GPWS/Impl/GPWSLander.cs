﻿// GPWS mod for KSP
// License: CC-BY-NC-SA
// Author: bss, 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP_GPWS.Interfaces;
using KSP_GPWS.SimpleTypes;

namespace KSP_GPWS.Impl
{
    public class GPWSLander : ILanderConfig, IBasicGPWSFunction
    {
        private IGPWSCommonData CommonData = null;

        #region ILanderConfig
        public bool EnableSystem { get; set; }
        public bool EnableDescentRate { get; set; }
        public bool EnableHorizontalSpeed { get; set; }
        public bool EnableAltitudeCallouts { get; set; }
        public int[] AltitudeArray { get; set; }

        /// <summary>
        /// use meters or feet, default:meter
        /// </summary>
        public UnitOfAltitude UnitOfAltitude { get; set; }
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

        public GPWSLander()
        {
            InitializeConfig();
        }

        public void InitializeConfig()
        {
            EnableSystem = true;
            EnableDescentRate = true;
            EnableHorizontalSpeed = true;
            EnableAltitudeCallouts = true;

            AltitudeArray = new int[] { 2500, 1000, 500, 100, 50, 40, 30, 20, 10 };
            UnitOfAltitude = UnitOfAltitude.METER;
        }

        public void Initialize(IGPWSCommonData data)
        {
            CommonData = data;

            initializeCurves();
        }

        private void initializeCurves()
        {
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
            if (CommonData.RadarAltitude > 0 && CommonData.RadarAltitude < float.PositiveInfinity)
            {
                if (checkMode_sinkRate())   // Decent Rate
                { }
                else if (checkMode_hSpeed())    // Horizontal Speed
                { }
                else if (checkMode_altitudeCallout())   // Altitude Callouts
                { }
                else if (!Util.audio.IsPlaying())
                {
                    Util.audio.MarkNotPlaying();
                }
            }
        }

        /// <summary>
        /// Descent Rate
        /// SINK RATE / MASTER WARN
        /// </summary>
        /// <returns></returns>
        private bool checkMode_sinkRate()
        {
            if (EnableDescentRate)
            {
            }
            return false;
        }

        /// <summary>
        /// Horizontal Speed
        /// HORIZONTAL SPEED
        /// </summary>
        /// <returns></returns>
        private bool checkMode_hSpeed()
        {
            if (EnableHorizontalSpeed)
            {
            }
            return false;
        }

        /// <summary>
        /// Altitude Callouts
        /// </summary>
        /// <returns></returns>
        private bool checkMode_altitudeCallout()
        {
            if (EnableAltitudeCallouts)
            {
                // is descending
                if (CommonData.RadarAltitude - CommonData.LastRadarAltitude < 0)
                {
                    // lower than an altitude
                    foreach (float threshold in AltitudeArray)
                    {
                        if (CommonData.LastRadarAltitude > threshold && CommonData.RadarAltitude < threshold)
                        {
                            // play sound
                            Util.audio.PlaySound(KindOfSound.ALTITUDE_CALLOUTS, threshold.ToString());
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ChangeVessel(Vessel v)
        {
        }

        public void Clear()
        {
        }
    }
}
