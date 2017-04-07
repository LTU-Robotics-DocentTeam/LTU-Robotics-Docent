//Serial comms can become very noisy if they're not slowed down. This function implements a cooldown counter to stop them from sending too often
// if too many messages are generated and excede buffer limit, send msgBuffer and reset cooldown and counter

void SerialOut(String msg)
{
  if (serialCommCounter <= 0 || msgCounter > MAX_BUFFER)// if serial counter is down to 0, send message
  {
    Serial.println(msg + msgBuffer);
    serialCommCounter = SERIAL_WAIT; // reset counter to specified constant
    msgCounter = 0;
  }
  else
  {
    msgBuffer += msg;
    msgCounter++;
  }
}

