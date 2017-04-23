
void SetMotor(char motor, int newVelocity)
{
  if (EStopped)
    return;



  if(motor == 'R')
  {
    
    RightSpeed = abs(newVelocity);

    RightDirection = constrain(newVelocity, -1, 1);
    
  }
  
    
    

  if(motor == 'L')
  {
 
    LeftSpeed = abs(newVelocity);

    LeftDirection = constrain(newVelocity, -1, 1);
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



  if (LeftDirection == -1)
    LeftRelayClosed = true;

  if (LeftDirection == 1)
    LeftRelayClosed = false;


  LeftMotorValue = LeftSpeed; 
  


  if (RightDirection == -1)
    RightRelayClosed = true;

  if (RightDirection == 1)
    RightRelayClosed = false;


  RightMotorValue = RightSpeed; 
  


  
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
