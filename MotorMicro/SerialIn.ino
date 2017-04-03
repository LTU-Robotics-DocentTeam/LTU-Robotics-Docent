void SerialIn()
{
  // Read full incoming message from the serial port
  msg = Serial.readString();

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
    // Extract message within '<' and '>'
    int msglngth = endin - startin;
    String msg = msg.substring(startin + 1, endin + 1);

    // Take out the first character as the Key and the rest as the value
    char key = msg[0];
    int value = msg.substring(1).toInt();

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
        if (value < 0) // if incoming speed is negative
        {
          rdir = true; // switch direction relay on
          value = -1 * value; // take incoming absolute value for the speed
        }
        else // if its positive
        {
          rdir = false; // switch direction relay off
          // keep value as it is
        }
        rmspeed = value; // store value as motor speed
        break;
      case 'L': // left motor input
        if (value < 0) // if incoming speed is negative
        {
          ldir = true; // switch direction relay on
          value = -1 * value; // take incoming absolute value for the speed
        }
        else // if its positive
        {
          ldir = false; // switch direction relay off
          // keep value as it is
        }
        lmspeed = value; // store value as motor speed
        break;
      case 'F': // attempt E-Stop reset signal
        estopsig = value; // set computer signal for the estop to whatever the computer sent (1 for true, 0 for false)
                          // this does not override internal estop signal, but must be reset to false to allow motion again
        break;
      default:
        break;
    }
  }
}

