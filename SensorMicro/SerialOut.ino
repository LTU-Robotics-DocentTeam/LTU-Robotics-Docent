void SerialOut(int i)
{
  String message = ""; // create empty string to fill in with a message to the computer

  // Read from sensors through their respective functions and store data as message strings
  String voltage = "<S" + String(float(analogRead(P_U2_BattVolt)) * VOLTAGE_FACTOR) + ">";
  String hallArray = Hall_Effect_Array();
  long ultraValue = Ultra_Sensor(USping[i], USecho[i]);
  String ultrasonic = "<" + (String)USkey[i] + (String)ultraValue + ">";

  // Add corresponding sensor data to the message only if it has changed from last iteration

  if (serialCount % HE_SENSOR_LOOP == 0)
  {
    if (prevHe != hallArray)
    {
      message += hallArray;
      prevHe = hallArray;
    }
  }

  if (serialCount % US_SENSOR_LOOP == 0)
  {
    if (USprev[i] != ultraValue)
    {
      message += ultrasonic;
      USprev[i] = ultraValue;
    }
  }

  if (serialCount % UPDATE_LOOP == 0)
  {
    if (prevVolt != voltage)
    {
      message += voltage;
      prevVolt = voltage;
    }
  }

  // if message is not empty, send through serialport
  if (message != "")
  {
    Serial.println(message);
  }

  if (serialCount > 0)
  {
    serialCount--;
  }
  else
  {
    serialCount = SERIAL_COMM_INIT;
  }

}

