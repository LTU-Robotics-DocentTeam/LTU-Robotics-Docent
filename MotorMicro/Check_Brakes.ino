// Funciton checks to see if the mechanical brakes are engaged

String Check_Brakes()
{
  int Left_Brake = digitalRead(P_U1_LB); // reads left brake
  int Right_Brake = digitalRead( P_U1_RB); // read right brake
    
  String Left_Brake_Engaged;
  String Right_Brake_Engaged;
  
  if (Left_Brake == LOW) // if left brake is engaged
  {
    EStop();
    Serial.println("BRAKES");
    LeftMotorBrake = true;
    Left_Brake_Engaged = "1";
  }
  else if (Left_Brake == HIGH) // if left brake is not engaged
  {
    Left_Brake_Engaged = "0";
  }
  
  if (Right_Brake == LOW) // if right brake is engaged
  {
    EStop();
    Serial.println("BRAKES");
    RightMotorBrake = true;
    Right_Brake_Engaged = "1";
  }
  else if (Right_Brake == HIGH) // if right brake is not engaged
  {
    Right_Brake_Engaged = "0";
  }

  return ("<P" + Left_Brake_Engaged + "><Q" + Right_Brake_Engaged + ">");
}

