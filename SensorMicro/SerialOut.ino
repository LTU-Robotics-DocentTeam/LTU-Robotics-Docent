void SerialOut(int i)
{
  String message = "";
  String voltage = "<S" + String(analogRead(P_U2_BattVolt)) + ">";
  String hallArray = Hall_Effect_Array();
  long ultraValue = Ultra_Sensor(USping[i], USecho[i]);
  String ultrasonic = "<" + (String)USkey[i] + (String)ultraValue + ">";
  //  Old ultrasonic code
  //  long Ultrasonic_1 = Ultra_Sensor(2, 3); //Call for First Ultrasonic
  //  String Ultra_1 = "A";
  //  Ultra_1 += String(Ultrasonic_1);
  //  long Ultrasonic_2 = Ultra_Sensor( 4, 5); //Call for Second Ultrasonic
  //  String Ultra_2 = "B";
  //  Ultra_2 += String(Ultrasonic_2);
  //  long Ultrasonic_3 = Ultra_Sensor( 6, 7); //Call for First Ultrasonic
  //  String Ultra_3 = "C";
  //  Ultra_3 += String(Ultrasonic_3);
  //  long Ultrasonic_4 = Ultra_Sensor( 8, 9); //Call for First Ultrasonic
  //  String Ultra_4 = "D";
  //  Ultra_4 += String(Ultrasonic_4);
  //  long Ultrasonic_5 = Ultra_Sensor( 12, 13); //Call for First Ultrasonic
  //  String Ultra_5 = "E";
  //  Ultra_5 += String(Ultrasonic_5);

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

  if (message != "")
  {
    Serial.println(message);
  }

}

