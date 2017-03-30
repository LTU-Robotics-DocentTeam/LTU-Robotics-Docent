void Estop(int onoff)
{
  if (onoff)
  {
    estop = true;
    rmotor.write(0);
    lmotor.write(0);
    estopsig = true;
  }
  else
  {
    estop = false;
    estopsig = true;
  }
}
