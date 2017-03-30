String signal;

void setup() {
  Serial.begin(115200);
  // put your setup code here, to run once:

}

void loop() {
  if (Serial.available() > 0)
  {
    signal = Serial.readString();
    //msg = signal;
    while (true)
    {
      int startin = signal.indexOf('<');
      if (startin < 0)
      {
        break;
      }
      int endin = signal.indexOf('>');
      if (endin < 0)
      {
        break;
      }
      int msglngth = endin - startin;
      String msg = signal.substring(startin + 1, endin + 1);
      char key = msg[0];
      int value = msg.substring(1).toInt();
      //Serial.println(value);
      signal = signal.substring(endin + 1);
      //Serial.println(signal);

      switch (key)
      {
        case 'C':
          if (value == 0)
          {
            Serial.println("<C2>");
          }
      }
    }
    //msg = prevmsg;
  }
  //String Path = Hall_Effect_Array() // insert stuff
  long Ultrasonic_1 = Ultra_Sensor(2, 3); //Call for First Ultrasonic
  String Ultra_1 = "A";
  Ultra_1 += String(Ultrasonic_1);
  long Ultrasonic_2 = Ultra_Sensor( 4, 5); //Call for Second Ultrasonic
  String Ultra_2 = "B";
  Ultra_2 += String(Ultrasonic_2);
  long Ultrasonic_3 = Ultra_Sensor( 6, 7); //Call for First Ultrasonic
  String Ultra_3 = "C";
  Ultra_3 += String(Ultrasonic_3);
  long Ultrasonic_4 = Ultra_Sensor( 8, 9); //Call for First Ultrasonic
  String Ultra_4 = "D";
  Ultra_4 += String(Ultrasonic_4);
  long Ultrasonic_5 = Ultra_Sensor( 12, 13); //Call for First Ultrasonic
  String Ultra_5 = "E";
  Ultra_5 += String(Ultrasonic_5);
  
}
