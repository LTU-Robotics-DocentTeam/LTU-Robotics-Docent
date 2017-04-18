void SerialIn()
{
  // Read full incoming message from the serial port
  //String msg = Serial.readStringUntil('>');



  String inputBuffer;

  while (Serial.available() > 0)
  {
    inputBuffer += (char)Serial.read();
  }

  for (int i = 0; i < inputBuffer.length(); i++)
  {
    bBuff += inputBuffer[i];

    if (inputBuffer[i] == '>')
    {
      ProcessSerial(bBuff);
      bBuff = "";
    }
  }

}
void ProcessSerial(String msg)
{


  //Serial.println("ReadString");

  // check for valid messages with a start character '<' and end character '>'
  int startin = msg.indexOf('<');

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
        Serial.print("<C2>"); // send back device id (1 for the motor microntroller)
        pcConnect = true;
      }
      // else do nothing
      break;
    case 'T':
      if (value == 1)
      {
        Servo_Lift(true);
      }
      else if (value == 0)
      {
        Servo_Lift(false);
      }
      break;
  }
}
