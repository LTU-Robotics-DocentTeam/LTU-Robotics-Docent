// Controls the servo used to lift the hall effect sensor array
void Servo_Lift(bool On)
{
 // Lowers the hall effect sensor array
  if (On == true)
  {
    Lift_Servo.write(180);
    Servo_Engaged = "1";
    return;
  }

  // Raises the hall effect sensor array
  if (On == false)
  {
    Lift_Servo.write(0);
    Servo_Engaged = "0";
    return;
  }
}

