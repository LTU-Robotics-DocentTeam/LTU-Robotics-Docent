using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules
{
    class Constants
    {
        public const int DEFAULT_SPEED = 50; // default speed for both motors when no obstacles are in front or nearby

        public const int MAXSPEED = 120; // DO NOT CHANGE -- Maximum speed the motors can take (based on regular servo code)
                                         // 180 is SANIC fast, so don't set spd to the max
        public const int DEAD_ZONE = 55;

        public const int US_NUM = 5;

        public const int IR_NUM = 6;

        public const int IMPACT_NUM = 10;

        public const int ARRAY_NUM = 16; // Number of sensors in hall effect array (At 7 for testing purposes, total of 16)

        public const int CLUSTER_SIZE = 2; // Number of sensors per cluster
    }
}
