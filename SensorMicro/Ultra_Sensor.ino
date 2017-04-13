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
  int mm; // Declaration for the millimeters distance because Arduino IDE doesn't like the next declarations
  if ( duration > 0) // if it detects an obstacle closer than 6 feet 
  { 
    mm = (duration/2) / 2.91; // converts microseconds into length in millimeters
  }

  else 
  {
    mm = 2000; //sees nothing
  }

  return mm; // returns distance in millimeters
}
