void processCommandLight()
{
  if (cfModuleType == MODTYPE_LIGHT)
  {
    if (bfrIN[0] == BICCP_GRP_LIGHT)
    {
      switch(bfrIN[1])
      {
        case BICCP_CMD_LIGHT_IDENT:
          processLightIdent();
          break;
        case BICCP_CMD_LIGHT_PRESET:
          processLightPreset();
          break;
        case BICCP_CMD_LIGHT_SCENSTART:
          processLightScenario();
          break;
        case BICCP_CMD_LIGHT_SCENSTOP:
          processLightScenarioStop();
          break;

        default:
          if (bfrIN[1] >= BICCP_CMD_LIGHT_OUT_STR && bfrIN[1] <= BICCP_CMD_LIGHT_OUT_STR + 9)
            processLightSet();
          else if (bfrIN[1] >= BICCP_CMD_LIGHT_OUT_STR + 10 && bfrIN[1] <= BICCP_CMD_LIGHT_OUT_END)
            processLightSetDimmable();
      }
    }
  }
}

void processLightIdent()
{
#ifdef DEBUG
  Serial.println(F("modLIGHT-processLightIdent"));
#endif
  bfrOUT[0] = BICCP_GRP_LIGHT;
  bfrOUT[1] = BICCP_CMD_LIGHT_IDENT;
  for (int i = 0; i < MAXOUTPUTS; i++)
  {
    if (i < 0xA)
      bfrOUT[2 + i] = (iOutputs[i] ? 0xFF : 0x00);
    else
      bfrOUT[2 + i] = iOutputs[i];
  }
}

void processLightPreset()
{
#ifdef DEBUG
  Serial.println(F("modLIGHT-processLightPreset"));
#endif
  int iError = 0;
  for (int i = 0; i < 10; i++)
  {
    if (bfrIN[2 + i] != 0xFF && bfrIN[2 + i] != 0x00)
      iError = 1;
  }
  if (iError == 0)
  {
    for (int i = 0; i < 10; i++)
      iOutputs[i] = (bfrIN[2 + i] ? HIGH : LOW);
    for (int i = 10; i < MAXOUTPUTS; i++)
      iOutputs[i] = bfrIN[2 + i];
  }
  bfrOUT[0] = BICCP_GRP_LIGHT;
  bfrOUT[1] = BICCP_CMD_LIGHT_PRESET;
  bfrOUT[2] = (iError == 0 ? BICCP_SUCCESS : BICCP_ERROR);
}

void processLightScenario()
{
#ifdef DEBUG
  Serial.println(F("modLIGHT-processLightScenario"));
#endif
  int iError = 0;
  int i = GetFreeActionSlot();
  if (i < 0)
      iError = 1;
  else if (bfrIN[2] == BICCP_SCN_LIGHT_PROGCHANGE)
  {
    unsigned int iStepMillis = bfrIN[5] * 256 + bfrIN[4];
    unsigned int iStep       = bfrIN[9] * 256 + bfrIN[8];
    actions[i] = new ActionLightProgressiveChange(bfrIN[3], iStepMillis, bfrIN[6], bfrIN[7], iStep, bfrIN[10]);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_TUNGON)
  {
    actions[i] = new ActionLightProgressiveChange(bfrIN[3], 6, 0x00, 0xFF, 12, 1);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_TUNGOFF)
  {
    unsigned int iStep = bfrIN[5] * 256 + bfrIN[4];
    actions[i] = new ActionLightProgressiveChange(bfrIN[3], 6, 0xFF, 0x00, 98, 0);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_BLKLED)
  {
    unsigned int iOn1 = bfrIN[6] * 256 + bfrIN[5];
    unsigned int iOn2 = bfrIN[8] * 256 + bfrIN[7];
    actions[i] = new ActionLightBlinker(bfrIN[3], bfrIN[4], iOn1, iOn2);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_BLKOLD)
  {
    unsigned int iOn1 = bfrIN[6] * 256 + bfrIN[5];
    unsigned int iOn2 = bfrIN[8] * 256 + bfrIN[7];
    actions[i] = new ActionLightBlinkerOLD(bfrIN[3], bfrIN[4], iOn1, iOn2);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_TRFCFRA)
  {
    unsigned int iOnG = bfrIN[10] * 256 + bfrIN[9];
    unsigned int iOnY = bfrIN[12] * 256 + bfrIN[11];
    unsigned int iOnR = bfrIN[14] * 256 + bfrIN[13];
    actions[i] = new ActionLightTrafficLights(bfrIN[3], bfrIN[4], bfrIN[5], bfrIN[6], bfrIN[7], bfrIN[8], iOnG, iOnY, iOnR, 1);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_TRFCDEU)
  {
    unsigned int iOnG = bfrIN[10] * 256 + bfrIN[9];
    unsigned int iOnY = bfrIN[12] * 256 + bfrIN[11];
    unsigned int iOnR = bfrIN[14] * 256 + bfrIN[13];
    actions[i] = new ActionLightTrafficLights(bfrIN[3], bfrIN[4], bfrIN[5], bfrIN[6], bfrIN[7], bfrIN[8], iOnG, iOnY, iOnR, 0);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_CHASER)
  {
    unsigned int iStep  = bfrIN[12] * 256 + bfrIN[11];
    unsigned int iOn    = bfrIN[14] * 256 + bfrIN[13];
    unsigned int iPause = bfrIN[16] * 256 + bfrIN[15];
    actions[i] = new ActionLightChaser(bfrIN[3], bfrIN[4], bfrIN[5], bfrIN[6], bfrIN[7], bfrIN[8], bfrIN[9], bfrIN[10], iStep, iOn, iPause, bfrIN[17]);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_ARCWELDING)
  {
    unsigned int iOn       = bfrIN[5]  * 256 + bfrIN[4];
    unsigned int iOff      = bfrIN[7]  * 256 + bfrIN[6];
    unsigned int iPauseMin = bfrIN[9]  * 256 + bfrIN[8];
    unsigned int iPauseMax = bfrIN[11] * 256 + bfrIN[10];
    actions[i] = new ActionLightFlasher(bfrIN[3], iOn, iOff, iPauseMin, iPauseMax, bfrIN[12], bfrIN[13]);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_CAMERAFLASH)
  {
    unsigned int iOn       = bfrIN[5]  * 256 + bfrIN[4];
    unsigned int iPauseMin = bfrIN[7]  * 256 + bfrIN[6];
    unsigned int iPauseMax = bfrIN[9]  * 256 + bfrIN[8];
    actions[i] = new ActionLightFlasher(bfrIN[3], iOn, 5, iPauseMin, iPauseMax, 1, 1);
  }
  else if (bfrIN[2] == BICCP_SCN_LIGHT_FIRE)
  {
    unsigned int iSpeed = bfrIN[6]  * 256 + bfrIN[5];
    actions[i] = new ActionLightFire(bfrIN[3], bfrIN[4], iSpeed);
  }
  
  bfrOUT[0] = BICCP_GRP_LIGHT;
  bfrOUT[1] = BICCP_CMD_LIGHT_SCENSTART;
  bfrOUT[2] = (iError == 0 ? BICCP_SUCCESS : BICCP_ERROR);
  bfrOUT[3] = (iError == 0 ? i : 0);
}

void processLightScenarioStop()
{
#ifdef DEBUG
  Serial.println(F("modLIGHT-processLightScenarioStop"));
#endif
  int iError = 0;
  if (bfrIN[2] < MAXOUTPUTS)
  {
    for (int i = 0; i < MAXACTIONS; i++)
    {
      if (actions[i] != 0)
        if (actions[i]->CheckOutput(bfrIN[2]))
          actions[i]->stop = 1;
    }
  }
  else
      iError = 1;

  bfrOUT[0] = BICCP_GRP_LIGHT;
  bfrOUT[1] = BICCP_CMD_LIGHT_SCENSTOP;
  bfrOUT[2] = (iError == 0 ? BICCP_SUCCESS : BICCP_ERROR);
}

void processLightSet()
{
#ifdef DEBUG
  Serial.println(F("modGENPURP-processLightSet"));
#endif
  if (bfrIN[2] == 0xFF || bfrIN[2] == 0x00)
  {
    iOutputs[bfrIN[1] - BICCP_CMD_LIGHT_OUT_STR] = (bfrIN[2] == 0xFF ? HIGH : LOW);
    bfrOUT[2] = BICCP_SUCCESS;
  }
  else
    bfrOUT[2] = BICCP_ERROR;

  bfrOUT[0] = BICCP_GRP_LIGHT;
  bfrOUT[1] = bfrIN[1];
}

void processLightSetDimmable()
{
#ifdef DEBUG
  Serial.println(F("modLIGHT-processLightSetDimmable"));
#endif
  iOutputs[bfrIN[1] - BICCP_CMD_LIGHT_OUT_STR] = bfrIN[2];

  bfrOUT[0] = BICCP_GRP_LIGHT;
  bfrOUT[1] = bfrIN[1];
  bfrOUT[2] = BICCP_SUCCESS;
}

void setupLight()
{
  // Non dimmable
//  pinMode(A6, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(13, OUTPUT);
  pinMode(A0, OUTPUT);
  pinMode(A1, OUTPUT);
  pinMode(A2, OUTPUT);
  pinMode(A3, OUTPUT);
  // Dimmable
  pinMode(3, OUTPUT);
  pinMode(5, OUTPUT);
  pinMode(6, OUTPUT);
  pinMode(9, OUTPUT);
  pinMode(10, OUTPUT);
  pinMode(11, OUTPUT);
  // Non dimmable
//  digitalWrite(A6, LOW);
  digitalWrite(4, LOW);
  digitalWrite(7, LOW);
  digitalWrite(8, LOW);
  digitalWrite(12, LOW);
  digitalWrite(13, LOW);
  digitalWrite(A0, LOW);
  digitalWrite(A1, LOW);
  digitalWrite(A2, LOW);
  digitalWrite(A3, LOW);
  // Dimmable
  analogWrite(3, 0);
  analogWrite(5, 0);
  analogWrite(6, 0);
  analogWrite(9, 0);
  analogWrite(10, 0);
  analogWrite(11, 0);
}

void loopLight()
{
//  digitalWrite(A6, iOutputs[0] ? HIGH : LOW);
  digitalWrite(4,  iOutputs[1] ? HIGH : LOW); // 0x1
  digitalWrite(7,  iOutputs[2] ? HIGH : LOW); // 0x2
  digitalWrite(8,  iOutputs[3] ? HIGH : LOW); // 0x3
  digitalWrite(12, iOutputs[4] ? HIGH : LOW); // 0x4
  digitalWrite(13, iOutputs[5] ? HIGH : LOW); // 0x5
  digitalWrite(A0, iOutputs[6] ? HIGH : LOW); // 0x6
  digitalWrite(A1, iOutputs[7] ? HIGH : LOW); // 0x7
  digitalWrite(A2, iOutputs[8] ? HIGH : LOW); // 0x8
  digitalWrite(A3, iOutputs[9] ? HIGH : LOW); // 0x9

  analogWrite(3,  iOutputs[0xA]); // 0xA
  analogWrite(5,  iOutputs[0xB]); // 0xB
  analogWrite(6,  iOutputs[0xC]); // 0xC
  analogWrite(9,  iOutputs[0xD]); // 0xD
  analogWrite(10, iOutputs[0xE]); // 0xE
  analogWrite(11, iOutputs[0xF]); // 0xF
}

void emerLight()
{
  setupLight();
}

