
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
    LeftMotorValue = LeftSpeed; 
  }
  else //to zero
  {
    if (LeftMotorValue > 0) // ramp down for relay
    {
      LeftMotorValue = 0;
    }
    else
    {
      LeftRelayClosed = LeftReverse;
      //Serial.println("LeftRelay");
    }
  }





  if (RightReverse == RightRelayClosed) //on the way up
  {
    RightMotorValue = RightSpeed;
    
  }
  else //to zero
  {
    if (RightMotorValue > 0)
    {
        RightMotorValue = 0;
    }
    else
    {
      RightRelayClosed = RightReverse;
      //Serial.println("RightRelay");
    }
  }

  
    LeftMotor.write(LeftMotorValue + LEFT_CORRECTION);
  //Serial.println("Left:" + String(LeftMotorValue));

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
