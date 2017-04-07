
void SetMotorLeft(int LeftVelocity)
{
  if (EStopped)
    return;


  //Serial.println("SetMotorLeft");

  LeftSpeed = abs(LeftVelocity);

  LeftReverse = LeftVelocity < 0;

  CommandHealth = HEALTH_CONSTANT;

}


void SetMotorRight(int RightVelocity)
{
  if (EStopped)
    return;

  //Serial.println("SetMotorRight");

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

  if (CommandHealth > 0)
  {
    CommandHealth--;
  }

  if (CommandHealth == 0)
  {
    //EStop();
  }




  if (LeftReverse == LeftRelayClosed)
  {
    if (LeftSpeed > LeftMotorValue) //on the way up
    {
      if (LeftMotorValue == 0)
        LeftMotorValue = DEAD_ZONE;
      else
        LeftMotorValue += RAMP_CONSTANT;

    }

    if (LeftSpeed < LeftMotorValue) //on the way down
    {
      if (LeftMotorValue < DEAD_ZONE)
        LeftMotorValue = 0;
      else
        LeftMotorValue -= RAMP_CONSTANT;
    }
  }
  else //to zero
  {
    if (LeftMotorValue > 0) // ramp down for relay
    {
      if (LeftMotorValue < DEAD_ZONE && LeftMotorValue > PRE_JUMP)
        LeftMotorValue = PRE_JUMP;
      else
        LeftMotorValue -= RAMP_CONSTANT;
    }
    else
    {
      LeftRelayClosed = LeftReverse;
      //Serial.println("LeftRelay");
    }
  }





  if (RightReverse == RightRelayClosed) //on the way up
  {
    if (RightSpeed > RightMotorValue)
    {
      if (RightMotorValue == 0)
        RightMotorValue = DEAD_ZONE;
      else
        RightMotorValue += RAMP_CONSTANT;
    }

    if (RightSpeed < RightMotorValue) //on the way down
    {
      if (RightMotorValue < DEAD_ZONE)
        RightMotorValue = 0;
      else
        RightMotorValue -= RAMP_CONSTANT;
    }
  }
  else //to zero
  {
    if (RightMotorValue > 0)
    {
      if (RightMotorValue < DEAD_ZONE && RightMotorValue > PRE_JUMP)
        RightMotorValue = PRE_JUMP;
      else
        RightMotorValue -= RAMP_CONSTANT;
    }
    else
    {
      RightRelayClosed = RightReverse;
      //Serial.println("RightRelay");
    }
  }





  if (LeftMotorValue == 0) LeftMotor.write(LeftMotorValue);
  else LeftMotor.write(LeftMotorValue + LEFT_CORRECTION);
  //Serial.println("Left:" + String(LeftMotorValue));

  RightMotor.write(RightMotorValue);
  //Serial.println("Right:"+ String(RightMotorValue));


  if (LeftRelayClosed)
    digitalWrite(P_U1_LD, HIGH);
  else
    digitalWrite(P_U1_LD, LOW);


  if (RightRelayClosed)
    digitalWrite(P_U1_RD, HIGH);
  else
    digitalWrite(P_U1_RD, LOW);

}
