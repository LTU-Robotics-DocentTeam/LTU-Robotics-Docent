//Serial comms can become very noisy if they're not slowed down. This function implements a cooldown counter to stop them from sending too often

void SerialOut(String msg)
{
  if (serialCommCounter <= 0)// if serial counter is down to 0, send message
  {
    Serial.println(msg);
    serialCommCounter = SERIAL_COUNT; // reset counter to specified constant
  }
}

