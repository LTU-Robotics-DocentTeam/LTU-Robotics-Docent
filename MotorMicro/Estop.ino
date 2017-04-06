
void EStop()
{
  EStopped = true;

  //Serial.println("EStop");

  LeftSpeed = 0;
  RightSpeed = 0;

  LeftMotorValue = 0;
  RightMotorValue = 0;

  LeftReverse = false;
  RightReverse = false;

  Serial.println("<E1>");

  LeftMotor.write(0);
  RightMotor.write(0);
}

void ResetEStop()
{
  EStopped = false;
}
