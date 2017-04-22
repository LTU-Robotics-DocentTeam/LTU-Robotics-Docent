using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules
{
    class Constants
    {
        public const int DEFAULT_SPEED = 5; // default speed for both motors when no obstacles are in front or nearby

        public const int MAX_MOTOR_SPEED = 90;                      // Maximum speed the motors can handle - DO NOT SURPASS 180!!
        public const int DEAD_ZONE = 48;                            // Deadzone to lowest signal the motor can take to move - DO NOT GO BELOW 48!
        public const int MAX_SPEED = MAX_MOTOR_SPEED - DEAD_ZONE;   // Determine maximum speed the code can set past the dead zone

        public const int MAX_DIST = 2000;
        public const int LINE_HOLD_BUFFER = 40;

        public const double TURN_FACTOR = 1;
        public const int TURN_ZONE = 39;
        public const int ZERO_POINT_ZONE = 75;

        //Number of sensors we have
        public const int US_NUM = 5;
        public const int IR_NUM = 6;
        public const int IMPACT_NUM = 8;
        public const int ARRAY_NUM = 16; // Number of sensors in hall effect array (At 7 for testing purposes, total of 16)

        public const int CLUSTER_SIZE = 2; // Number of sensors per cluster
        public const int CLUSTER_NUM = ARRAY_NUM / CLUSTER_SIZE; //Number of clusters in array
        public const double MAX_DIR = (Constants.CLUSTER_NUM - 1.0) / 2.0;

        public string FilePath = "log" + DateTime.Now.ToString() + ".txt";

        public const int MAST_TO_FRONT = 430;

        public static int ULTRA_WAIT_TIME = 80;

        public const int STOP_DIST = 450;
        public const double LOW_BATT_THRESHOLD = 24.0; // check
        public const double CRITICAL_BATT_THRESHOLD = 22.0;


        public const double ARRAY_TO_CENTER = 130;
        public const double CLUSTER_GAP = 21;
    }
}
