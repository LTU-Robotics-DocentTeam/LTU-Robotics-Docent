
void SetMotor(char motor, int newVelocity)
{
  if (EStopped)
    return;



  if(motor == 'R')
    RightSeat = newVelocity;
    

  if(motor == 'L')
    LeftSeat = newVelocity;



  

  if(RightSeat != -1 && LeftSeat != -1)
  {
    
      
    if(RightSeat == LeftSeat && RightCache != LeftCache)
    {
      if(RightCache > LeftCache)
      {
        RightPauseTimer = 50;
      }
      else if(LeftCache > RightCache)
      {
        LeftPauseTimer = 50;
      }
    }


    RightCache = RightSeat;
  
    LeftCache = LeftSeat;
  

  
    RightSpeed = abs(RightSeat);

    RightReverse = RightSeat < 0;
  

 
    LeftSpeed = abs(LeftSeat);

    LeftReverse = LeftSeat < 0;
  
    
    CommandHealth = HEALTH_CONSTANT;


    RightSeat = -1;
    LeftSeat = -1;
    
  } 

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





  if (LeftMotorValue == 0 || LeftPauseTimer > 0)
  {
    LeftMotor.write(0);
    LeftPauseTimer--;
  }
  else
    LeftMotor.write(LeftMotorValue + LEFT_CORRECTION);
  //Serial.println("Left:" + String(LeftMotorValue));

  if (RightMotorValue == 0 || RightPauseTimer > 0)
  {
    RightMotor.write(0);
    RightPauseTimer--;
  }
  else
    RightMotor.write(RightMotorValue + RIGHT_CORRECTION);
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
