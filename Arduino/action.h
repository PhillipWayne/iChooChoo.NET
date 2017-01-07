#ifndef _ACTION_H
#define _ACTION_H
#include "iChooChoo.h"

extern int iOutputs[MAXOUTPUTS];

class Action
{
  public:
    unsigned long lastRun;
    unsigned int interval;
    uint8_t stop = 0;

    virtual uint8_t CheckOutput(byte output) = 0;
    virtual uint8_t Do() = 0;

  protected:
    byte _step = 0;  // Active step

  private:
};

class ActionLightProgressiveChange : public Action
{
  public:
    ActionLightProgressiveChange (byte output, unsigned int millisStep, byte startValue, byte targetValue, int Step, byte linear)
    {
      _output = output;
      iOutputs[_output] = startValue;
      interval = (unsigned long)millisStep;
      _targetValue = targetValue;
      _linear = linear;
      if (_linear)
        _fStep = Step;
      else
        _fStep = (float)Step / 100.0f;

      lastRun = millis();
    }
    virtual uint8_t CheckOutput(byte output);
    virtual uint8_t Do();
    
  private:
    byte _output;       // Output for light
    byte _targetValue;  // Target value of the progressive turn on
    float _fStep;       // Step of increment
    byte _linear;       // 0 = Exponential (*step), 1 = Linear (+step)
};

class ActionLightBlinker : public Action
{
  public:
    ActionLightBlinker (byte output1, byte output2, unsigned int millisOn1, unsigned int millisOn2)
    {
      _output1 = output1;
      _output2 = output2;
      _onTime1 = (unsigned long)millisOn1;
      _onTime2 = (unsigned long)millisOn2;
      lastRun = millis();
      interval = _onTime2;
    }
    virtual uint8_t CheckOutput(byte output);
    virtual uint8_t Do();
    
  private:
    byte _output1;           // Output for light 1
    byte _output2;           // Output for light 2
    unsigned long _onTime1;  // Duration of light 1
    unsigned long _onTime2;  // Duration of light 2 (= Off time of light 1, if using only one light)
};

class ActionLightBlinkerOLD : public Action
{
  public:
    ActionLightBlinkerOLD (byte output1, byte output2, unsigned int millisOn1, unsigned int millisOn2)
    {
      _output1 = output1;
      _output2 = output2;
      _onTime1 = (unsigned long)millisOn1;
      _onTime2 = (unsigned long)millisOn2;
      lastRun = millis();
      interval = 6;
      _lastStep = millis();
      _stepInterval = _onTime2;
    }
    virtual uint8_t CheckOutput(byte output);
    virtual uint8_t Do();
    
  private:
    byte _output1;                // Output for light 1
    byte _output2;                // Output for light 2
    unsigned long _onTime1;       // Duration of light 1
    unsigned long _onTime2;       // Duration of light 2 (= Off time of light 1, if using only one light)
    unsigned long _lastStep;      // Time stamp of last step change
    unsigned long _stepInterval;  // Active step duration
};

class ActionLightTrafficLights : public Action
{
  public:
    ActionLightTrafficLights (byte outputG1, byte outputY1, byte outputR1, byte outputG2, byte outputY2, byte outputR2, unsigned int millisOnG, unsigned int millisOnY, unsigned int millisOnR, byte frenchMode)
    {
      _outputG1 = outputG1; _outputY1 = outputY1; _outputR1 = outputR1;
      _outputG2 = outputG2; _outputY2 = outputY2; _outputR2 = outputR2;
      iOutputs[_outputG1] = 0x00; iOutputs[_outputY1] = 0x00; iOutputs[_outputR1] = 0xFF;
      iOutputs[_outputG2] = 0x00; iOutputs[_outputY2] = 0x00; iOutputs[_outputR2] = 0xFF;
      _onTimeG = (unsigned long)millisOnG;
      _onTimeY = (unsigned long)millisOnY;
      _onTimeR = (unsigned long)millisOnR;
      _frenchMode = frenchMode;
      lastRun = millis();
      interval = _onTimeR;
    }
    virtual uint8_t CheckOutput(byte output);
    virtual uint8_t Do();
    
  private:
    byte _outputG1;          // Output for Green led of Traffic Light 1
    byte _outputY1;          // Output for Yellow led of Traffic Light 1
    byte _outputR1;          // Output for Red led of Traffic Light 1
    byte _outputG2;          // Output for Green led of Traffic Light 2
    byte _outputY2;          // Output for Yellow led of Traffic Light 2
    byte _outputR2;          // Output for Red led of Traffic Light 2
    unsigned long _onTimeG;  // Duration of Green light
    unsigned long _onTimeY;  // Duration of Yellow light (and Red/Yellow for German mode)
    unsigned long _onTimeR;  // Duration of Red light (before next green light, whether using 1 or 2 traffic lights)
    byte _frenchMode;        // French Mode = 1, German Mode = 0
};

class ActionLightChaser : public Action
{
  public:
    ActionLightChaser (byte output1, byte output2, byte output3, byte output4, byte output5, byte output6, byte output7, byte output8, unsigned int millisStep, unsigned int millisOn, unsigned int millisPause, byte coexistence)
    {
      _nbOutputs = 8;
      _outputs[0] = output1; if (output1 == 0) _nbOutputs = 0;
      _outputs[1] = output2; if (output2 == 0) _nbOutputs = 1;
      _outputs[2] = output3; if (output3 == 0) _nbOutputs = 2;
      _outputs[3] = output4; if (output4 == 0) _nbOutputs = 3;
      _outputs[4] = output5; if (output5 == 0) _nbOutputs = 4;
      _outputs[5] = output6; if (output6 == 0) _nbOutputs = 5;
      _outputs[6] = output7; if (output7 == 0) _nbOutputs = 6;
      _outputs[7] = output8; if (output8 == 0) _nbOutputs = 7;
      _stepTime = (unsigned long)millisStep;
      _onTime = (unsigned long)millisOn;
      _pauseTime = (unsigned long)millisPause;
      _coexistence = coexistence;
      if ((_coexistence & 0xF0) == 0x00)
        _nbSteps = _nbOutputs + (_coexistence & 0x0F) - 1;
      else
        _nbSteps = _nbOutputs;
      
      _lastStep = millis();
      lastRun = millis();
      interval = 5;
    }
    virtual uint8_t CheckOutput(byte output);
    virtual uint8_t Do();
    
  private:
    byte _outputs[8];          // Stores up to eight outputs
    byte _nbOutputs;           // Keep number of outputs (first output = 0 is end of chaser)
    unsigned long _stepTime;   // Duration of each step
    unsigned long _onTime;     // Duration of illumination
    unsigned long _pauseTime;  // Duration of pause between two chases
    byte _coexistence;         // Number of LEDs that should be ON at the same time
    byte _nbSteps;             // Number of steps on each serie
    unsigned long _lastStep;   // Time stamp of last step change
};

class ActionLightFlasher : public Action
{
  public:
    ActionLightFlasher (byte output, unsigned int millisOn, unsigned int millisOff, unsigned int millisPauseMin, unsigned int millisPauseMax, byte nbFlashMin, byte nbFlashMax)
    {
      _output = output;
      _onTime = (unsigned long)millisOn;
      _offTime = (unsigned long)millisOff;
      _pauseTimeMin = (unsigned long)millisPauseMin;
      _pauseTimeMax = (unsigned long)(millisPauseMax + 1);
      _nbFlashMin = nbFlashMin;
      _nbFlashMax = nbFlashMax + 1;
      _step = 0;
      _nbSteps = 0;
      lastRun = millis();
      interval = millisOff;
      randomSeed(analogRead(A6));
    }
    virtual uint8_t CheckOutput(byte output);
    virtual uint8_t Do();
    
  private:
    byte _output;                 // Output of the light
    unsigned long _onTime;        // Duration of illumination
    unsigned long _offTime;       // Duration of pause between illuminations
    unsigned long _pauseTimeMin;  // Minimum duration of pause between two flash series
    unsigned long _pauseTimeMax;  // Maximum duration of pause between two flash series
    byte _nbFlashMin;             // Minimum number of flashs per serie
    byte _nbFlashMax;             // Maximum number of flashs per serie
    byte _nbSteps;                // Length of current serie
};

class ActionLightFire : public Action
{
  public:
    ActionLightFire (byte output1, byte output2, unsigned int millisSpeed)
    {
      _output1 = output1;
      _output2 = output2;
      _step = 0;
      lastRun = millis();
      interval = millisSpeed;
      randomSeed(analogRead(A6));
    }
    virtual uint8_t CheckOutput(byte output);
    virtual uint8_t Do();
    
  private:
    byte _output1;             // Output of light 1 (maybe red)
    byte _output2;             // Output of light 2 (maybe orange)
};

#endif _ACTION_H

