﻿// GPWS mod for KSP
// License: CC-BY-NC-SA
// Author: bss, 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP_GPWS.SimpleTypes;
using KSP_GPWS.Interfaces;
using KSP_GPWS.Impl;

namespace KSP_GPWS
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public partial class GPWS : MonoBehaviour, IGPWSCommonData
    {
        public Vessel ActiveVessel { get; private set; }

        public float RadarAltitude { get; set; }

        public float Altitude { get; set; }

        public float LastRadarAltitude { get; private set; }

        public float LastAltitude { get; private set; }

        // time scene loaded
        private float t0 = 0.0f;

        /// time in seconds since scene loaded
        public float time { get; private set; }

        public float lastTime { get; private set; }

        private static GPWSPlane Plane = null;
        private static GPWSLander Lander = null;

        private IBasicGPWSFunction GPWSFunc;

        public static void InitializeGPWSFunctions()
        {
            if (Plane == null && Lander == null)    // call once
            {
                Plane = new GPWSPlane();
                Settings.PlaneConfig = Plane as IPlaneConfig;
                Lander = new GPWSLander();
                Settings.LanderConfig = Lander as ILanderConfig;
            }
        }

        public void Awake()
        {
            Plane.Initialize(this as IGPWSCommonData);
            Lander.Initialize(this as IGPWSCommonData);
            initializeVariables();
        }

        private void initializeVariables()
        {
            ActiveVessel = null;

            RadarAltitude = 0.0f;
            Altitude = 0.0f;
            LastRadarAltitude = float.PositiveInfinity;
            LastAltitude = float.PositiveInfinity;

            t0 = Time.time;
            time = t0;
            lastTime = t0;
        }

        public void Start()
        {
            Util.Log("Start");
            Util.audio.AudioInitialize();

            GameEvents.onVesselChange.Add(OnVesselChange);
            ActiveVessel = FlightGlobals.ActiveVessel;
            Plane.ChangeVessel(ActiveVessel);
            Lander.ChangeVessel(ActiveVessel);
        }

        private void OnVesselChange(Vessel v)
        {
            Plane.ChangeVessel(ActiveVessel);
            Lander.ChangeVessel(ActiveVessel);
        }

        private bool preUpdate()
        {
            time = Time.time - t0;
            // check time, prevent problem
            if (time < 2.0f)
            {
                Util.audio.SetUnavailable();
                return false;
            }

            // just switched
            if (FlightGlobals.ActiveVessel != ActiveVessel)
            {
                Util.audio.MarkNotPlaying();
                return false;
            }

            // check volume
            if (Util.audio.Volume != GameSettings.VOICE_VOLUME * Settings.Volume)
            {
                Util.audio.UpdateVolume();
            }

            if (Plane.GearCount > 0)
            {
                GPWSFunc = Plane as IBasicGPWSFunction;
            }
            else
            {
                GPWSFunc = Lander as IBasicGPWSFunction;
            }


            if (!GPWSFunc.PreUpdate())
            {
                return false;
            }

            return true;
        }

        void UpdateGPWS()
        {
            // height in meters/feet
            if (UnitOfAltitude.FOOT == Settings.PlaneConfig.UnitOfAltitude)
            {
                RadarAltitude = Util.RadarAltitude(ActiveVessel) * Util.M_TO_FT;
                Altitude = (float)(FlightGlobals.ship_altitude * Util.M_TO_FT);
            }
            else
            {
                RadarAltitude = Util.RadarAltitude(ActiveVessel);
                Altitude = (float)FlightGlobals.ship_altitude;
            }

            GPWSFunc.UpdateGPWS();
        }

        public void Update()
        {
            if (preUpdate())
            {
                UpdateGPWS();
            }

            saveData();
        }

        private void saveData() // after Update
        {
            LastRadarAltitude = RadarAltitude;    // save last gear height
            LastAltitude = Altitude;
            lastTime = time;        // save time of last frame
            ActiveVessel = FlightGlobals.ActiveVessel;
        }

        public void OnDestroy()
        {
            GameEvents.onVesselChange.Remove(OnVesselChange);
        }
    }
}
