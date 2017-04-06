
String Impact()
{
  String arraystate = "";
  for (int i = 0; i < L_NUM; i++)
  {
    if (mcp.digitalRead(impact[i]) == LOW)
    {
      EStop();
      arraystate += "1";
    }
    else
    {
      arraystate += "0";
    }
  }
  //Serial.println(arraystate);
  return arraystate;
}

