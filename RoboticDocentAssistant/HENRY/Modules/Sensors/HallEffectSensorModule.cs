using HENRY.ModuleSystem;
using System.Timers;


namespace HENRY.Modules.Sensors
{
    /// <summary>
    /// Hall effect secondary processing module. Takes raw boolean inputs from the sensor array and determines
    /// line angle with respect to the robot
    /// </summary>
    /// TO DO:
    /// - Update line following to work with the clusters of two arrangement
    class HallEffectSensorModule : LengarioModuleAuxiliary
    {
        Timer t;
        
        const int ArrayNum = 16; // Number of sensors in hall effect array (At 7 for testing purposes, total of 16)
        const int ClusterSize = 2; // Number of sensors per cluster
        
        public HallEffectSensorModule()
        {
            for (int i = 1; i <= ArrayNum; i++)
                SetPropertyValue("ArraySensor" + i.ToString(), false);

            SetPropertyValue("ArrayNum", ArrayNum);

            //0 is Hard left, 90 is straight on (Can be avoided, See comment below)
            // ^ Load array backwards to make visualization more intuitive (i.e. 0 is hard right,
            //   90 is straight on, 180 is hard left)
            SetPropertyValue("LineAngle", 0.0);

            t = new Timer();
            t.Interval = 330;
            t.Elapsed += t_Elapsed;
            t.Start();
        }

        /// <summary>
        /// Process hall effect sensor data. Determine line direction based on state of the Hall effect array.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool[] arr = new bool[ArrayNum]; // Boolean array that represents full hall effect array
                                             // Each unit represents a sensor
            bool[] clarr = new bool[ArrayNum / ClusterSize]; // Boolean array that represents full cluster array
                                                             // Each unit represents a cluster, not a single sensor
            double anglestep = 180.0 / (ArrayNum / ClusterSize); // Set angle step to match number of clusters in array
            double lineloc = -1; // start as negative number, so if no line is found, a negative number is sent to main module
                                 // Negative number means error state in nav module, so no line
            
            // Fill in array of clusters from raw data. Uses clustersize variable, but in reality can only really work
            // with clusters of two the way its set up. Loads backwards so visual representation matches actual arrangement
            //-------------------------------------------------------------------------------------------------------------
            // Patrick, feel free to use what I got so far or use your own method. 
            for (int i = (ArrayNum / ClusterSize) - 1; i >= 0; i -= ClusterSize)
            {
                // Load data adjacent sensors in clusters of two 
                arr[i * ClusterSize] = GetPropertyValue("ArraySensor" + (i * ClusterSize + 1).ToString()).ToBoolean();
                arr[i * ClusterSize - 1] = GetPropertyValue("ArraySensor" + (i * ClusterSize).ToString()).ToBoolean();

                // Then it would determine whether the cluster is active using OR logic (if any sensor in cluster is on,
                // cluster is on)
                clarr[i] = arr[i * ClusterSize] || arr[i * ClusterSize - 1];
            }

            // Use cluster data to determine where the line is
            //Find the spaceing between the clusters that are on
            int gap1 = 0; //number of clusters off from the right side
            int gap2 = 0; //size of gap between 2 clusters on or from one on to left side
            int gap3 = 0; //size of second gap between second two engauged clusters if three are on
            int gapNum = 1; //Number of gap to increase
            for (int i = 0; i < (ArrayNum / ClusterSize); i++)
            {
                if (!clarr[i])
                {
                    switch (gapNum) //Logic to find which gap to increase if the cluster is off
                    {
                        case 1:
                            gap1++; //When we have not encountered an engauged cluster we increase the first gap
                            break;
                        case 2:
                            gap2++; //After the first cluster is found we increase the second gap
                            break;
                        case 3:
                            gap3++; //After second engauged cluster is found we increase the third gap
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    gapNum++; //Each time a new activated cluster is found move to the next gap
                }

            }

            //use gap spaceing to detirmine where line is

            lineloc = anglestep * gap1; //set tenitive angle size
            //If two clusters are on, Adjust the angle to half way between them
            if (gap3 > 0) 
            {
                lineloc += (anglestep * (gap2 / 2));
            }
            //error catching and extreme cases, ****will be changed through testing
            if (gap1 == 0 || gap1 == 1)
            {
                if (gap2 >= ((ArrayNum / ClusterSize) - 3))
                    lineloc = -1;
            }

            SetPropertyValue("LineAngle", lineloc);

            // Add some sort of error catching here maybe? (i.e. two clusters on opposite sides fire, what do?)
            
            // Below is old code, feel free to ignore or draw inspiration from it

            //// Load boolean array variable that represents hall effect array. 
            //// Loads backwards so visual representation matches actual arrangement
            //for (int i = ArrayNum - 1; i >= 0; i--)
            //{
            //    arr[i] = GetPropertyValue("ArraySensor" + (i + 1).ToString()).ToBoolean();
            //}

            //for (int i = 0; i < ArrayNum; i++)
            //{
                
            //    if (arr[i])
            //        lineloc = anglestep * (i + 1);

            //    SetPropertyValue("LineAngle", lineloc);
            //}


        }

        public override string GetModuleName()
        {
            return "HallEffectSensorModule";
        }
    }
}
