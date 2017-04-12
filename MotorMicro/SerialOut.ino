//Serial comms can become very noisy if they're not slowed down. This function implements a cooldown counter to stop them from sending too often
// if too many messages are generated and excede buffer limit, send msgBuffer and reset cooldown and counter

void SerialOut()
{
  String message = ""; // create empty string to fill in with a message to the computer

  if (serialCount % L_LOOP == 0)
  {
    //message += impactArray;
    if (prevIm != impactArray)
    {
      message += impactArray;
      prevIm = impactArray;
    }
  }

  if (serialCount % UPDATE_LOOP == 0)
  {
    if (digitalRead(P_U1_SW) == LOW)
    {
      message += "<U0>";
    }
    else
    {
      message += "<U1>";
    }
  }

  if (message != "" && pcConnect)
  {
    Serial.println(message);
  }

  // update serial counter
  if (serialCount > 0)
  {
    serialCount--;
  }
  else
  {
    serialCount = SERIAL_COMM_INIT;
  }

}

void SerialBuffer(String msg)
{
  if (serialCommCounter <= 0 || msgCounter > MAX_BUFFER)// if serial counter is down to 0, send message
  {
    Serial.println(msg + msgBuffer);
    serialCommCounter = SERIAL_WAIT; // reset counter to specified constant
    msgBuffer = "";
    msgCounter = 0;
  }
  else
  {
    msgBuffer += msg;
    msgCounter++;
  }
}

