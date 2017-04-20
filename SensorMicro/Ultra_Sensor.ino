// Controls the ultrasonic sensors, sends out an ultrasonic ping and waits for a calculated time in microseconds for the echo
int Ultra_Sensor(int P, int E)// Call for the Ultrasonic Sensors
{
 // pinMode(P, OUTPUT); //sets ping pin
  digitalWrite(P, LOW);// These are the ping sent out
  delayMicroseconds(5);// These are the ping sent out
  digitalWrite(P, HIGH);// These are the ping sent out
  delayMicroseconds(10);// These are the ping sent out
  digitalWrite(P, LOW);// These are the ping sent out

 // pinMode(E, INPUT); //set echo pin
  long  duration = pulseIn(E, HIGH, 11662); // waits for the time duration in microseconds to reach 6 feet out and back or until ping is heard
  int mm; // Declaration for the millimeters distance

  // Conversion calculation of duration in microseconds to distance in millimeters
  if ( duration > 0) // if it detects an obstacle closer than 6 feet 
  { 
    mm = (duration/2) / 2.91; // converts microseconds into length in millimeters
  }

  else  // if there is a time out duration is set to 0 there for distance is greater than 2000mm
  {
    mm = 2000; //sees nothing
  }
  
  if (mm == 0) // if for some reason the previous calculation fails
  {
    mm = 2000;
  }
  
  if (j == 0) // iteration for each sensor, since each sensor is called 10 times in a row this is for the shortest distance to be logged
  {
   SmallestDist[i] = 2000;
  }
  
  else if (mm < SmallestDist[i]) // if current distance is smaller than shortest distance logged sets as shortest distance
  {
    SmallestDist[i] = mm;
  }
  j++;
  if (j == 2*US_SENSOR_LOOP) // resest sensor iteration count after a sensor is finished reading
        {
            j = 0;
        } 

  return SmallestDist[i]; // returns distance in millimeters
}
