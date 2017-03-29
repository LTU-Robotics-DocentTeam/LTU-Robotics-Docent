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

}
