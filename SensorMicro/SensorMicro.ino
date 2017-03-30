String signal;

void setup() {
  Serial.begin(115200);
  pinMode(2, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(3, INPUT);
  pinMode(5, INPUT);
  pinMode(9, INPUT);
  pinMode(13, INPUT);
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
  String Ultra_1 = "V";
  Ultra_1 += String(Ultrasonic_1);
  long Ultrasonic_2 = Ultra_Sensor( 4, 5); //Call for Second Ultrasonic
  String Ultra_2 = "W";
  Ultra_2 += String(Ultrasonic_2);
  long Ultrasonic_3 = Ultra_Sensor( 6, 7); //Call for Third Ultrasonic
  String Ultra_3 = "X";
  Ultra_3 += String(Ultrasonic_3);
  long Ultrasonic_4 = Ultra_Sensor( 8, 9); //Call for Fourth Ultrasonic
  String Ultra_4 = "Y";
  Ultra_4 += String(Ultrasonic_4);
  long Ultrasonic_5 = Ultra_Sensor( 12, 13); //Call for Fifth Ultrasonic
  String Ultra_5 = "Z";
  Ultra_5 += String(Ultrasonic_5);
  
}
