void SerialIn()
{
  // Read full incoming message from the serial port
  String msg = Serial.readString();

  Serial.println("ReadString");

  while (true) // loop until the code breaks
  {
    // check for valid messages with a start character '<' and end character '>'
    int startin = msg.indexOf('<');
    if (startin < 0) // if not found (indexOf returned -1), break loop
    {
      break;
    }
    int endin = msg.indexOf('>'); // if not found (indexOf returned -1), break loop
    if (endin < 0)
    {
      break;
    }

    Serial.println("FoundMessage");

    Serial.println("startin:");
    Serial.println(startin);
    Serial.println("endin:");
    Serial.println(endin);
    
    // Extract message within '<' and '>'
    int msglngth = endin - startin;

    Serial.println("msglngth:");
    Serial.println(msglngth);
    
    String msg = msg.substring(startin + 1, endin - 1);


    Serial.println("msg:");
    Serial.println(msg);

    // Take out the first character as the Key and the rest as the value
    char key = msg[0];
    int value = msg.substring(1).toInt();

    Serial.println("MessageKey:");
    Serial.println(key);
    Serial.println("MessageValue:");
    Serial.println(value);
    delay(30000);

    // Remove the digits already read from the incoming string
    msg = msg.substring(endin + 1);

    // Switch statement for determining where to assign the value based on the key
    switch (key)
    {
      case 'C': // identification key
        if (value == 0) // if incoming id is 0 (the computer)...
        {
          Serial.println("<C1>"); // send back device id (1 for the motor microntroller)
        }
        // else do nothing
        break;
      case 'R': // right motor input
        SetMotorRight(value); // store value as motor speed
        Serial.println("SendingRightMotorValue");
        break;
      case 'L': // left motor input
        SetMotorLeft(value); // store value as motor speed
        break;
      case 'F': // attempt E-Stop reset signal
        ResetEStop();
        break;
      default:
        break;
    }
  }
}

