using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENRY.Modules
{
    class Constants
    {
        public const int DEFAULT_SPEED = 10; // default speed for both motors when no obstacles are in front or nearby

        public const int MAX_MOTOR_SPEED = 180;                   // Maximum speed the motors can handle - DO NOT SURPASS 180!!
        public const int DEAD_ZONE = 55;                          // Deadzone to lowest signal the motor can take to move - DO NOT GO BELOW 50!
        public const int MAX_SPEED = MAX_MOTOR_SPEED - DEAD_ZONE; // Determine maximum speed the code can set past the dead zone
        

        public const int US_NUM = 5;

        public const int IR_NUM = 6;

        public const int IMPACT_NUM = 10;

        public const int ARRAY_NUM = 16; // Number of sensors in hall effect array (At 7 for testing purposes, total of 16)

        public const int CLUSTER_SIZE = 2; // Number of sensors per cluster
    }
}
