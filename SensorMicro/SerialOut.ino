void SerialOut(int i)
{
  String message = ""; // create empty string to fill in with a message to the computer

  // Read from sensors through their respective functions and store data as message strings
  String voltage = "<S" + String(analogRead(P_U2_BattVolt)) + ">";
  String hallArray = Hall_Effect_Array();
  long ultraValue = Ultra_Sensor(USping[i], USecho[i]);
  String ultrasonic = "<" + (String)USkey[i] + (String)ultraValue + ">";

  // Add corresponding sensor data to the message only if it has changed from last iteration
  if (prevVolt != voltage)
  {
    message += voltage;
    prevVolt = voltage;
  }
  if (prevHe != hallArray)
  {
    message += hallArray;
    prevHe = hallArray;
  }
  if (USprev[i] != ultraValue)
  {
    message += ultrasonic;
    USprev[i] = ultraValue;
  }

  // if message is not empty, send through serialport
  if (message != "")
  {
    Serial.println(message);
  }

}

