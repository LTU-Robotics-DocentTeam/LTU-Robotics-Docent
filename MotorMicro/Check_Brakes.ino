String Check_Brakes()
{
  int Left_Brake = digitalRead(P_U1_LB);
  String Left_Brake_Engaged;
  String Right_Brake_Engaged;
  if (Left_Brake == LOW)
  {
    EStop();
    Serial.println("BRAKES");
    LeftMotorBrake = true;
    Left_Brake_Engaged = "1";
  }
  else if (Left_Brake == HIGH)
  {
    Left_Brake_Engaged = "0";
  }
  
  int Right_Brake = digitalRead( P_U1_RB);
  if (Right_Brake == LOW)
  {
    EStop();
    Serial.println("BRAKES");
    RightMotorBrake = true;
    Right_Brake_Engaged = "1";
  }
  else if (Right_Brake == HIGH)
  {
    Right_Brake_Engaged = "0";
  }

  return ("<P" + Left_Brake_Engaged + "><Q" + Right_Brake_Engaged + ">");
}

