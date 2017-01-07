#include <Arduino.h>
#include "action.h"

uint8_t ActionLightProgressiveChange::Do()
{
  if (iOutputs[_output] == _targetValue)
    return 1;

  if (_linear)
    iOutputs[_output] += (int)_fStep;
  else
    iOutputs[_output] = (int)((float)iOutputs[_output] * _fStep);

  if ((int)_fStep > 0 && iOutputs[_output] > _targetValue)
    iOutputs[_output] = _targetValue;
  if ((int)_fStep < 0 && iOutputs[_output] < _targetValue)
    iOutputs[_output] = _targetValue;

  lastRun = millis();
  return 0;
}

uint8_t ActionLightProgressiveChange::CheckOutput(byte output)
{
  return (output == _output);
}

uint8_t ActionLightBlinker::Do()
{
  if (stop && (iOutputs[_output1] == 0x00))
  {
    iOutputs[_output2] = 0x00;
    return 1; 
  }

  iOutputs[_output1] = (iOutputs[_output1] == 0x00 ? 0xFF : 0x00);
  iOutputs[_output2] = (iOutputs[_output1] == 0x00 ? 0xFF : 0x00);
  interval = (iOutputs[_output1] == 0x00 ? _onTime2 : _onTime1);
  lastRun = millis();

  return 0;
}

uint8_t ActionLightBlinker::CheckOutput(byte output)
{
  return ((output == _output1 || output == _output2) ? 1 : 0);
}

uint8_t ActionLightBlinkerOLD::Do()
{
  if (stop && (iOutputs[_output1] == 0x00) && (iOutputs[_output2] == 0x00))
    return 1; 

  // Steps : true = light 1, false = light 2
  if (_step)
  {
    if (iOutputs[_output1] < 0xFF && !stop)
      iOutputs[_output1] += 12;
    if (iOutputs[_output2] > 0)
      iOutputs[_output2] = (int)((float)iOutputs[_output2] * 0.98);
  }
  else
  {
    if (iOutputs[_output2] < 0xFF && !stop)
      iOutputs[_output2] += 12;
    if (iOutputs[_output1] > 0)
      iOutputs[_output1] = (int)((float)iOutputs[_output1] * 0.98);
  }
  if (iOutputs[_output1] < 0)    iOutputs[_output1] = 0;
  if (iOutputs[_output1] > 0xFF) iOutputs[_output1] = 0xFF;
  if (iOutputs[_output2] < 0)    iOutputs[_output2] = 0;
  if (iOutputs[_output2] > 0xFF) iOutputs[_output2] = 0xFF;

  if (millis() - _lastStep > _stepInterval)
  {
    _step = !_step;
    _lastStep = millis();
    _stepInterval = (_step ? _onTime1 : _onTime2);
  }

  lastRun = millis();
  return 0;
}

uint8_t ActionLightBlinkerOLD::CheckOutput(byte output)
{
  return ((output == _output1 || output == _output2) ? 1 : 0);
}

uint8_t ActionLightTrafficLights::Do()
{
  if (stop && (_step == 0))
    return 1; 

  _step++;
  if ((_step == 1 || _step == 5) && _frenchMode)
    _step++;
  if (_step > 7 || (_step > 3 && _outputG2 == 0))
    _step = 0;
  
  // Steps : 0 = Red/Red, 1 = RedYellow/Red, 2 = Green/Red, 3 = Yellow/Red, 4 = Red/Red, 5 = Red/RedYellow, 6 = Red/Green, 7 = Red/Yellow
  
  iOutputs[_outputG1] = (_step == 2 ? 0xFF : 0x00);
  iOutputs[_outputY1] = (_step == 1 || _step == 3 ? 0xFF : 0x00);
  iOutputs[_outputR1] = (_step != 2 && _step != 3 ? 0xFF : 0x00);
  iOutputs[_outputG2] = (_step == 6 ? 0xFF : 0x00);
  iOutputs[_outputY2] = (_step == 5 || _step == 7 ? 0xFF : 0x00);
  iOutputs[_outputR2] = (_step != 6 && _step != 7 ? 0xFF : 0x00);

  interval = ((_step % 2) != 0 ? _onTimeY : (_step == 0 || _step == 4 ? _onTimeR : _onTimeG));

  lastRun = millis();
  return 0;
}

uint8_t ActionLightTrafficLights::CheckOutput(byte output)
{
  return ((output == _outputG1 || output == _outputY1 || output == _outputR1 || output == _outputG2 || output == _outputY2 || output == _outputR2) ? 1 : 0);
}

uint8_t ActionLightChaser::Do()
{
  // When onTime exceeded, turn all lights off
  for (int i = 0; i < _nbOutputs; i++)
    iOutputs[_outputs[i]] = 0x00;

  if (stop) return 1;

  // When stepTime exceeded, go to next step
  if (millis() - _lastStep >= _stepTime)
  {
    _step++;
    if (_step > _nbSteps) // Loop to step zero when reached last step
    {
      if ((_coexistence & 0xF0) == 0x10) // Circular mode
        _step = 1;
      else
        _step = 0;
    }
    _lastStep = millis();
  }

  // Process illumination
  if (_step > 0 && millis() - _lastStep < _onTime)
  {
    if ((_coexistence & 0xF0) == 0x00) // Linear mode
    {
      for (int i = _step - (_coexistence & 0x0F); i < _step && i < 8; i++)
        if (i >= 0) iOutputs[_outputs[i]] = 0xFF;
    }
    else if ((_coexistence & 0xF0) == 0x10) // Circular mode
    {
      for (int i = _step - 1; i < (_step + (_coexistence & 0x0F) - 1); i++)
        iOutputs[_outputs[(i < _nbOutputs ? i : i - _nbOutputs)]] = 0xFF;
    }
    else if ((_coexistence & 0xF0) == 0x20) // Progressive mode
    {
      for (int i = 0; i < _step; i++)
        iOutputs[_outputs[i]] = 0xFF;
    }
  }
  
  // Set pause time when active step is back to zero, or set 5ms interval when running chaser
  if (_step == 0 && (_coexistence & 0xF0) != 0x10)
    interval = _pauseTime;
  else
    interval = 5;
  
  lastRun = millis();
  return 0;
}

/*uint8_t ActionLightChaser::Do()
{
  if (stop && (_step == 0))
    return 1; 

  // When onTime exceeded, turn all lights off
  if (millis() - _lastStep >= _onTime)
  {
    for (int i = 0; i < _nbOutputs; i++)
      iOutputs[_outputs[i]] = 0x00;
  }

  // When stepTime exceeded, go to next step
  if (millis() - _lastStep >= _stepTime)
  {
    _step++;
    if (_step >= _nbOutputs + _coexistence) // Loop to step zero when reached last step
      _step = 0;

    for (int i = 0; i < _nbOutputs; i++)
      iOutputs[_outputs[i]] = (_step > i && _step <= i + _coexistence ? 0xFF : 0x00);

    _lastStep = millis();
  }

  // Set pause time when active step is back to zero, or set 5ms interval when running chaser
  interval = (_step == 0 ? _pauseTime : 5);
  
  lastRun = millis();
  return 0;
}*/

uint8_t ActionLightChaser::CheckOutput(byte output)
{
  for (int i = 0; i < 8; i++)
  {
    if (_outputs[i] == output)
      return true;
  }
  return false;
}

uint8_t ActionLightFlasher::Do()
{
  if (stop && (_step == 0))
    return 1; 

  // If we are on step 0, défine number of steps randomly
  if (_step == 0)
    _nbSteps = (byte)random(_nbFlashMin, _nbFlashMax);

  // Define on/off status of flash depending on preceding state
  if (iOutputs[_output])
  {
    iOutputs[_output] = 0x00;
    interval = _offTime;
  }
  else
  {
    iOutputs[_output] = 0xFF;
    interval = _onTime;
    _step++;
  }

  // If overdue steps, rollback to step 0, turn light off and define a random pause time
  if (_step > _nbSteps)
  {
    iOutputs[_output] = 0x00;
    interval = random(_pauseTimeMin, _pauseTimeMax);
    _step = 0;
  }

  lastRun = millis();
  return 0;
}

uint8_t ActionLightFlasher::CheckOutput(byte output)
{
  return (_output == output);
}

uint8_t ActionLightFire::Do()
{
  if (stop && iOutputs[_output1] == 0x00 && iOutputs[_output2] == 0x00)
    return 1; 

  byte direction1 = 0;
  byte direction2 = 0;
  // Define direction (lighter or dimmer) randomly for each light. If stop condition, leave on dimmer until complete off.
  if (!stop)
  {
    direction1 = random(0, 2);
    direction2 = random(0, 2);
  }

  if (direction1)
    iOutputs[_output1] = iOutputs[_output1] + random(10, 40);
  else
    iOutputs[_output1] = iOutputs[_output1] - random(10, 40);
  
  if (direction2)
    iOutputs[_output2] = iOutputs[_output2] + random(10, 40);
  else
    iOutputs[_output2] = iOutputs[_output2] - random(10, 40);

  if (iOutputs[_output1] < 0x00) iOutputs[_output1] = 0x00;
  if (iOutputs[_output1] > 0xFF) iOutputs[_output1] = 0xFF;
  if (iOutputs[_output2] < 0x00) iOutputs[_output2] = 0x00;
  if (iOutputs[_output2] > 0xFF) iOutputs[_output2] = 0xFF;

  lastRun = millis();
  return 0;
}

uint8_t ActionLightFire::CheckOutput(byte output)
{
  return ((output == _output1 || output == _output2) ? 1 : 0);
}

