
void SetMotor(char motor, int newVelocity)
{
  if (EStopped)
    return;



  if(motor == 'R')
  {
    RightSpeed = abs(newVelocity);

    RightReverse = newVelocity < 0;
  }
    

  if(motor == 'L')
  {
    LeftSpeed = abs(newVelocity);
  
    LeftReverse = newVelocity < 0;
  }


  
    
    CommandHealth = HEALTH_CONSTANT;

}
  

void RunMotors()
{
  

  if (CommandHealth > 0)
  {
    CommandHealth--;
  }

  if (CommandHealth == 0)
  {
    SetMotor('R',0);
    SetMotor('L',0);
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






  if (LeftMotorValue == 0)
  {
    LeftMotor.write(0);
  }
  else
    LeftMotor.write(LeftMotorValue + LEFT_CORRECTION);
  


  if (RightMotorValue == 0)
  {
    RightMotor.write(0);
  }
  else{
    if(correction)
    {
      
      RightMotor.write(RightMotorValue + RIGHT_CORRECTION);
      correction = false;
    }
    else
    {
      RightMotor.write(RightMotorValue);
      correction = true;
    }
    
  }



  if (LeftRelayClosed)
    digitalWrite(P_U1_LD, HIGH);
  else
    digitalWrite(P_U1_LD, LOW);


  if (RightRelayClosed)
    digitalWrite(P_U1_RD, HIGH);
  else
    digitalWrite(P_U1_RD, LOW);

}
