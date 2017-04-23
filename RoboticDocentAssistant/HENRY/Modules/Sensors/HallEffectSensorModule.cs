using HENRY.ModuleSystem;
using System.Timers;
using System;
using System.Windows.Forms;


namespace HENRY.Modules.Sensors
{
    /// <summry>
    /// Hall effect secondary processing module. Takes raw boolean inputs from the sensor array and determines
    /// line angle with respect to the robot
    /// </summary>
    /// TO DO:
    /// - Update line following to work with the clusters of two arrangement
    class HallEffectSensorModule : LengarioModuleAuxiliary
    {
        System.Timers.Timer t;

        
        
        public HallEffectSensorModule()
        {
            for (int i = 1; i <= Constants.ARRAY_NUM; i++)
                SetPropertyValue("ArraySensor" + i.ToString(), false);

            SetPropertyValue("LineAngle", 0.0);

            //t = new Timer();
            //t.Interval = 10;
            //t.Elapsed += t_Elapsed;
            //t.Start();
        }

        /// <summary>
        /// Process hall effect sensor data. Determine line direction based on state of the Hall effect array.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public int Calculate()
        {
            bool[] arr = new bool[Constants.ARRAY_NUM]; // Boolean array that represents full hall effect array
            // Each unit represents a sensor
            bool[] clarr = new bool[Constants.CLUSTER_NUM]; // Boolean array that represents full cluster array
            // Each unit represents a cluster, not a single sensor
            //double anglestep = 180.0 / (NumOfCluster - 1); // Set angle step to match number of clusters in array
            double lineloc = 0; // start as negative number, so if no line is found, a negative number is sent to main module
            // Negative number means error state in nav module, so no line

            // Fill in array of clusters from raw data. Uses clustersize variable, but in reality can only really work
            // with clusters of two the way its set up. Loads backwards so visual representation matches actual arrangement
            //-------------------------------------------------------------------------------------------------------------
            // Patrick, feel free to use what I got so far or use your own method. 

            SetPropertyValue("Extra", String.Empty);

            //Take serial Hall effect sensor data and load into a boolean array
            for (int i = 0; i < Constants.ARRAY_NUM; i++)
            {
                arr[i] = GetPropertyValue("ArraySensor" + (i + 1).ToString()).ToBoolean();
                
            }

            //Use Hall effect boolean array data to load cluster array. ****This loop only works for a cluster size of 2****
            for (int i = 0; i < Constants.CLUSTER_NUM; i++)
            {
                if (arr[i * Constants.CLUSTER_SIZE] || arr[(i * Constants.CLUSTER_SIZE) + 1]) //if either hall in a pair is on, set cluster to true
                {
                    clarr[i] = true;
                }
                else //if neither hall in the pair is on, cluster is set to false
                {
                    clarr[i] = false;
                }
            }

            // Use cluster data to determine where the line is
            //Find the spaceing between the clusters that are on


            /*

            int gap = 0; //number of clusters off from the right side
            bool gapSwitch = false; //detects the beginning of a gap
            bool gapError = false; //set to true when a gap in clusters is detected: error state var
            bool numOfClusterError = false; //set to true when too many clusters detected: error state var
            bool noClusterFoundError = false; //set to true when no clusters are detected: error state var
            int clusterFound = 0; //incrementing number of clusters detected

            for (int i = 0; i < (Constants.CLUSTER_NUM); i++)
            {
                if (clusterFound < 1 && !clarr[i]) //increment the gap until a cluster is found on
                {
                    gap++;
                }
                else if (clusterFound > 0 && !clarr[i]) //after finding any clusters on then one off, turn on gap detection
                {
                    gapSwitch = true;
                }
                else if (clusterFound > 0 && clarr[i] && gapSwitch) //when gap detection is on and another cluster is found, incite gap error
                {
                    gapError = true;
                    clusterFound++;
                }
                else if (clarr[i]) //whenever we find a cluster on, increment the clusterFound number
                {
                    clusterFound++;
                }
            }

            //use gap spaceing to detirmine where line is
            //different angle setting technique for different number of clusters found
            if (clusterFound == 0)
            {
                noClusterFoundError = true; //Set no cluster found error because no sensors detected
            }
            else if (clusterFound == 1)
            {
                lineloc = (1.0 * gap); //set angle size in direction of the one cluster
            }
            else if (clusterFound == 2)
            {
                lineloc = ((1.0 * gap) + (1.0 / 2.0)); //set angle size inbetween two sensed clusters
            }
            else if (clusterFound == 3)
            {
                lineloc = (1.0 * (gap + 1.0)); //set line angle towords the middle of the three clusters
            }
            else
            {
                numOfClusterError = true; //set error for too many clusters on
            }

            //If any errors, set lineloc to an error state
            if (noClusterFoundError || gapError || numOfClusterError)
            {
                SetPropertyValue("LineAngle", -100.0);
                return -1;
            }
            else
            {
                lineloc -= Constants.MAX_DIR;
                SetPropertyValue("LineAngle", -lineloc);
            }
             * 
             * */

            int activeClusters = 0;
            double distanceStorage = 0;

            for (int i = 0; i < (Constants.CLUSTER_NUM); i++)
            {
                if(clarr[i])
                {
                    distanceStorage += Constants.CLUSTER_GAP * (i - 3.5);
                    activeClusters++;
                }
            }

            //MessageBox.Show(activeClusters.ToString());

            double averageDistance;

            if (activeClusters > 0)
                averageDistance = distanceStorage / activeClusters;
            else
            {
                SetPropertyValue("LineAngle", -100.0);
                return -1;
            }
                


            double averageAngle = -RadiansToDegrees(Math.Atan(averageDistance / Constants.ARRAY_TO_CENTER));



            SetPropertyValue("LineAngle", averageAngle);
            

            return 0;
        }

        public override string GetModuleName()
        {
            return "HallEffectSensorModule";
        }

        private double RadiansToDegrees(double angle)
        {
            return angle * ( 180.0 / Math.PI );
        }
    }
}
