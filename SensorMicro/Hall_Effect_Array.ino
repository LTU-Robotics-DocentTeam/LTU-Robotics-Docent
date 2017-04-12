String Hall_Effect_Array()
{
  String hall_array = ""; // create empty string
  for (int i = 0; i < H_NUM; i++) // fill in string with individual sensor states in binary with time out cycle
  {
    if (mcp.digitalRead(HEpins[i]) == LOW){
      hall_array += "1"; // if sensor is triggered, add "1" to string
      hallValuesLife[i] = HALL_LIFE;
    }
    else{  //When hall is off, check if the life is still on, if not set 0
      if (hallValuesLife[i] > 0){
        hallValuesLife[i]--;
        hall_array += "1";
      }
      else{
        hall_array += "0";
      }
    }  
  }
  return "<H"+ hall_array + ">"; // return code to send through serial
}

