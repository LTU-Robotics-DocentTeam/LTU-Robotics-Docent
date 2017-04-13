void SerialOut()
{
  String message = ""; // create empty string to fill in with a message to the computer

  // Read from sensors through their respective functions and store data as message strings
  String voltage = "<S" + String(float(analogRead(P_U2_BattVolt)) * VOLTAGE_FACTOR) + ">";
  //String hallArray = Hall_Effect_Array();
  long ultraValue = Ultra_Sensor(USping[i], USecho[i]);
  String ultrasonic = "<" + (String)USkey[i] + (String)ultraValue + ">";

  // Add corresponding sensor data to the message only if it has changed from last iteration

  // send hall effect data every HE_SENSOR_LOOP iterations
  // quickest update loop. only sends data when there's a change
  if (serialCount % HE_SENSOR_LOOP == 0)
  {
    String hallArray = Hall_Effect_Array();
    if (prevHe != hallArray)
    {
      message += hallArray;
      prevHe = hallArray;
    }
  }

  // send ultrasonic data every US_SENSOR_LOOP iterations
  // arranged such that every sensor is checked twice per second
  if (serialCount % US_SENSOR_LOOP == 0)
  {
    message += ultrasonic;
    // Counter for ultrasonic sensor
    i++; // update every iteration
    if (i == U_NUM) // as soon as it reaches 5 (i > 4), reset counter i
    {
      i = 0;
    }
        
    //    if (USprev[i] != ultraValue)
    //    {
    //      message += ultrasonic;
    //      USprev[i] = ultraValue;
    //    }
  }

  // send voltage data every UPDATE_LOOP iterations
  // slowest loop as voltage readings jump around a lot
  if (serialCount % UPDATE_LOOP == 0)
  {
    if (prevVolt != voltage)
    {
      message += voltage;
      prevVolt = voltage;
    }
  }

  // if message is not empty, send through serialport
  if (message != "" && pcConnect)
  {
    Serial.print(message);
  }

  // update serial counter
  if (serialCount > 0)
  {
    serialCount--;
  }
  else
  {
    serialCount = SERIAL_COMM_INIT;
  }

}

