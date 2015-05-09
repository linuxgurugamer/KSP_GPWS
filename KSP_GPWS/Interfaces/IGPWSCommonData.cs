﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSP_GPWS.Interfaces
{
    public interface IGPWSCommonData
    {
        float RadarAltitude { get; set; }

        float LastRadarAltitude { get; }

        float Altitude { get; set; }

        float LastAltitude { get; }

        /// <summary>
        /// in m/s
        /// </summary>
        float HorSpeed { get; }

        float LastHorSpeed { get; }

        /// <summary>
        /// in m/s
        /// </summary>
        float VerSpeed { get; }

        float LastVerSpeed { get; }

        float Speed { get; }

        float time { get; }

        float lastTime { get; }

        /// <summary>
        /// time of takeoff.
        /// when on ground, takeOffTime = time
        /// </summary>
        float takeOffTime { get; }

        /// <summary>
        /// time of landing/splashing.
        /// when flying, landingTime = time
        /// </summary>
        float landingTime { get; }

        Vessel ActiveVessel { get; }
    }
}
