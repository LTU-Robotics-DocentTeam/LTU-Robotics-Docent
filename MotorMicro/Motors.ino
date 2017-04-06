
void SetMotorLeft(int LeftVelocity)
{
  if (EStopped)
    return;


  Serial.println("SetMotorLeft");

  LeftSpeed = abs(LeftVelocity);

  LeftReverse = LeftVelocity < 0;

  CommandHealth = HEALTH_CONSTANT;

}


void SetMotorRight(int RightVelocity)
{
  if (EStopped)
    return;

  Serial.println("SetMotorRight");

  RightSpeed = abs(RightVelocity);

  RightReverse = RightVelocity < 0;

  CommandHealth = HEALTH_CONSTANT;

}


void RunMotors()
{
//  if (MotorCooldown > 0)
//  {
//    MotorCooldown--;
//
//    LeftMotor.write(0);
//    RightMotor.write(0);
//
//    return;
//  }

  if(CommandHealth > 0)
  {
    CommandHealth--;
  }
  
  if(CommandHealth == 0)
  {
    //EStop();
  }




  if (LeftReverse == LeftRelayClosed)
  {
    if (LeftSpeed > LeftMotorValue)
    {
      LeftMotorValue += RAMP_CONSTANT;
      if (LeftMotorValue == 1)
      {
        LeftMotorValue = 55;
      }
      if (LeftMotorValue == -40)
      {
        LeftMotorValue = 0;
      }
    }

    if (LeftSpeed < LeftMotorValue)
    {
      LeftMotorValue -= RAMP_CONSTANT;
      if (LeftMotorValue == 40)
      {
        LeftMotorValue = 0;
      }
      if (LeftMotorValue == -1)
      {
        LeftMotorValue = -55;
      }
    }
  }
  else
  {
    if (LeftMotorValue > 0)
      LeftMotorValue -= RAMP_CONSTANT;
    else
      LeftRelayClosed = LeftReverse;
  }





  if (RightReverse == RightRelayClosed)
  {
    if (RightSpeed > RightMotorValue)
      RightMotorValue += RAMP_CONSTANT;

    if (RightSpeed < RightMotorValue)
      RightMotorValue -= RAMP_CONSTANT;
  }
  else
  {
    if (RightMotorValue > 0)
      RightMotorValue -= RAMP_CONSTANT;
    else
      RightRelayClosed = RightReverse;
  }





  LeftMotor.write(LeftMotorValue);

  RightMotor.write(RightMotorValue);


  if (LeftRelayClosed)
    digitalWrite(P_U1_LD, HIGH);
  else
    digitalWrite(P_U1_LD, LOW);


  if (RightRelayClosed)
    digitalWrite(P_U1_RD, HIGH);
  else
    digitalWrite(P_U1_RD, LOW);

}
