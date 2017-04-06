void SerialIn()
{
  // Read incoming serialport message up to next valid end byte '>'
  String msg = Serial.readStringUntil('>');

  // Uncomment when debugging through serial monitor
  //Serial.println("ReadString");

  while (true) // loop until the code breaks
  {
    // check for valid message with a start character '<'
    int startin = msg.indexOf('<');
    if (startin < 0) // if not found (indexOf returned -1), break loop
    {
      break;
    }

    // Uncomment when debugging through serial monitor
    //Serial.println("FoundMessage");
    //
    //Serial.println("startin:");
    //Serial.println(startin);


    //Serial.println("msglngth:");
    //Serial.println(msglngth);

    msg = msg.substring(startin + 1);


    //Serial.println("msg:");
    //Serial.println(msg);

    // Take out the first character as the Key and the rest as the value
    char key = msg[0];
    int value = msg.substring(1).toInt();

    // Uncomment when debugging through serial monitor
    //Serial.println("MessageKey:");
    //Serial.println(key);
    //Serial.println("MessageValue:");
    //Serial.println(value);
    //delay(30000);

    // Switch statement for determining where to assign the value based on the key
    switch (key)
    {
      case 'C': // identification key
        if (value == 0) // if incoming id is 0 (the computer)...
        {
          Serial.println("<C2>"); // send back device id (1 for the motor microntroller)
          pcConnect = true;
        }
        // else do nothing
        break;
    }
  }
}
