
String Impact()
{
  String arraystate = "";
  for (int i = 0; i < 8; i++)
  {
    if (mcp.digitalRead(impact[i]))
    {
      //EStop();
      arraystate += "1";
    }
    else
    {
      arraystate += "0";
    }
  }
  return arraystate;
}

