using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library.Actions;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Represents a Lighting Module.
    /// </summary>
    public class LightingModule : Module, IOnOffOutputsModule, ILightingModule
    {
        private bool[] _bOutputs = new bool[10];
        private byte[] _bDimmableOutputs = new byte[6];

        /// <summary>
        /// Instantiates a new Lighting Module.
        /// </summary>
        public LightingModule() : base()
        {
#if VERBOSEDEBUG
   Console.Writeline("LightingModule()");         
#endif
            Type = ICCConstants.BICCP_GRP_LIGHTING;
            TypeDescription = ICCConstants.GetTypeDescription((byte)Type);
        }

        /// <summary>
        /// Get the output value of a dimmable output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <returns>Value of the dimmable output.</returns>
        public virtual byte getDimmableOutput(int output)
        {
            if (output >= 10 && output < 16)
                return _bDimmableOutputs[output - 10];
            else
                throw new ArgumentOutOfRangeException("LightingModule: Dimmable output can only be 10 to 15.");
        }

        /// <summary>
        /// Get the output value of an On/Off output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <returns>Value of the output.</returns>
        public virtual bool getOutput(int output)
        {
            if (output >= 0 && output < 10)
                return _bOutputs[output];
            else if (output >= 10 && output < 16)
                return (_bDimmableOutputs[output - 10] > 0);
            else
                throw new ArgumentOutOfRangeException("LightingModule: Output can only be 0 to 15.");
        }

        /// <summary>
        /// Set the value of a dimmable output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="value">Value to be set.</param>
        public virtual void setDimmableOutput(byte output, byte value)
        {
            if (output >= 10 && output < 16)
            {
                _bDimmableOutputs[output - 10] = value;
                var a = new ICCActionLightingSetDimmableOutput() { Address = (byte)ID, Log = Log };
                a.Data = new object[] { output, value };
                RaiseAction(a);
            }
            else
                throw new ArgumentOutOfRangeException("LightingModule: Dimmable output can only be 10 to 15.");
        }

        /// <summary>
        /// Set the value of an On/Off output.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="value">Value to be set.</param>
        public virtual void setOutput(byte output, bool value)
        {
            byte outValue = (byte)(value ? 0xFF : 0x00);
            if (output >= 0 && output < 10)
            {
                _bOutputs[output] = value;
                var a = new ICCActionLightingSetOutput { Address = (byte)ID, Log = Log };
                a.Data = new object[] { output, value };
                RaiseAction(a);
            }
            else if (output >= 10 && output < 16)
            {
                setDimmableOutput(output, (byte)(value ? 255 : 0));
            }
            else
                throw new ArgumentOutOfRangeException("LightingModule: Output can only be 0 to 15.");
        }

        /// <summary>
        /// Writes the status of the module.
        /// </summary>
        /// <returns>Status of the module.</returns>
        public override string writeStatus()
        {
            string s = string.Empty;

            for (int i = 0; i < 10; i++)
            {
                s += string.Format(" {0}", _bOutputs[i] ? 1 : 0);
            }

            for (int i = 0; i < 6; i++)
            {
                s += string.Format(" {0}", _bDimmableOutputs[i]);
            }

            return s;
        }

        /// <summary>
        /// Start a lighting scenario. Use preferably direct scenario methods that provide a clear explanation of parameters.
        /// </summary>
        /// <param name="action">Scenario ID (see ICCConstants).</param>
        /// <param name="args">Arguments to be passed to the scenario.</param>
        public virtual void StartScenario(byte action, params int[] args)
        {
            var a = ICCActionLightingScenarioStart.GetAction(action, (byte)ID, Log);
            a.Data = new object[] { action, args };
            RaiseAction(a);
        }

        /// <summary>
        /// Stop a lighting scenario.
        /// </summary>
        /// <param name="output">One of the output pins used by the scenario to be stopped.</param>
        public virtual void StopScenario(byte output)
        {
            if (output < ICCConstants.MAX_OUTPUTS)
            {
                var a = new ICCActionLightingScenarioStop() { Address = (byte)ID, Log = Log };
                a.Data = new object[] { output };
                RaiseAction(a);
            }
            else
                throw new ArgumentException(string.Format("LightingModule.StopScenario: Cannot stop scenario, invalid pin {0}", output));
        }

        /// <summary>
        /// Start a progressive light variation from start to target value, using linear or exponential progression.
        /// </summary>
        /// <param name="output">Output pin. (Use only dimmable output pin)</param>
        /// <param name="millisStep">Variation step duration in milliseconds.</param>
        /// <param name="startValue">Start light value.</param>
        /// <param name="targetValue">Target light value (end of variation).</param>
        /// <param name="step">Value of each step. Must be negative if target value is less than start value. For exponential variation, this value will be divided by 100 and multiplicated on each step.</param>
        /// <param name="linear">1 = Linear, 0 = Exponential (value is multiplicated by step/100 on each step).</param>
        public void StartProgressiveChange(byte output, int millisStep, byte startValue, byte targetValue, int step, byte linear)
        {
            var a = new ICCActionLightingScenarioStartProgChange() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_PROGCHANGE, new int[] { output, millisStep, startValue, targetValue, step, linear } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start a tungsten-like turn on light.
        /// </summary>
        /// <param name="output">Output pin. (Use only dimmable output pin)</param>
        public void StartTungstenOn(byte output)
        {
            var a = new ICCActionLightingScenarioStartTungsten() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_TUNGON, new int[] { output } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start a tungsten-like turn off light.
        /// </summary>
        /// <param name="output">Output pin. (Use only dimmable output pin)</param>
        public void StartTungstenOff(byte output)
        {
            var a = new ICCActionLightingScenarioStartTungsten() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_TUNGOFF, new int[] { output } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Starts a LED-style blinker (straight on/off) on one or two outputs. Set output2 to zero to blink only one output.
        /// </summary>
        /// <param name="output1">Output pin 1.</param>
        /// <param name="output2">Output pin 2.</param>
        /// <param name="millis1">Duration of pin 1 ON state in milliseconds.</param>
        /// <param name="millis2">Duration of pin 2 ON state in milliseconds (=pin 1 OFF state).</param>
        public void StartBlinkerLed(byte output1, byte output2, int millis1, int millis2)
        {
            var a = new ICCActionLightingScenarioStartBlinker() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_BLKLED, new int[] { output1, output2, millis1, millis2 } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Starts a Tungsten-style blinker (progressive on/off) on one or two outputs. Set output2 to zero to blink only one output.
        /// </summary>
        /// <param name="output1">Output pin 1. (Use only dimmable output pin)</param>
        /// <param name="output2">Output pin 2. (Use only dimmable output pin)</param>
        /// <param name="millis1">Duration of pin 1 ON state in milliseconds.</param>
        /// <param name="millis2">Duration of pin 2 ON state in milliseconds (=pin 1 OFF state).</param>
        public void StartBlinkerOld(byte output1, byte output2, int millis1, int millis2)
        {
            var a = new ICCActionLightingScenarioStartBlinker() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_BLKOLD, new int[] { output1, output2, millis1, millis2 } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start a french-style traffic light (Green/Yellow/Red->Green...). Set output pins for traffic light 2 to zero to cycle only one traffic light.
        /// </summary>
        /// <param name="outputGreen1">Output pin for traffic light 1 Green.</param>
        /// <param name="outputYellow1">Output pin for traffic light 1 Yellow.</param>
        /// <param name="outputRed1">Output pin for traffic light 1 Red.</param>
        /// <param name="outputGreen2">Output pin for traffic light 2 Green.</param>
        /// <param name="outputYellow2">Output pin for traffic light 2 Yellow.</param>
        /// <param name="outputRed2">Output pin for traffic light 2 Red.</param>
        /// <param name="millisGreen">Duration of Green state in milliseconds.</param>
        /// <param name="millisYellow">Duration of Yellow state in milliseconds.</param>
        /// <param name="millisRed">Duration of Red state before next Green in milliseconds. Should be higher if only using a single traffic light.</param>
        public void StartTrafficLightsFrench(byte outputGreen1, byte outputYellow1, byte outputRed1, byte outputGreen2, byte outputYellow2, byte outputRed2, int millisGreen, int millisYellow, int millisRed)
        {
            var a = new ICCActionLightingScenarioStartTrafficLights() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_TRFCFRA, new int[] { outputGreen1, outputYellow1, outputRed1, outputGreen2, outputYellow2, outputRed2, millisGreen, millisYellow, millisRed } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start a german-style traffic light (Green/Yellow/Red/Yellow+Red->Green...). Set output pins for traffic light 2 to zero to cycle only one traffic light.
        /// </summary>
        /// <param name="outputGreen1">Output pin for traffic light 1 Green.</param>
        /// <param name="outputYellow1">Output pin for traffic light 1 Yellow.</param>
        /// <param name="outputRed1">Output pin for traffic light 1 Red.</param>
        /// <param name="outputGreen2">Output pin for traffic light 2 Green.</param>
        /// <param name="outputYellow2">Output pin for traffic light 2 Yellow.</param>
        /// <param name="outputRed2">Output pin for traffic light 2 Red.</param>
        /// <param name="millisGreen">Duration of Green state in milliseconds.</param>
        /// <param name="millisYellow">Duration of Yellow state in milliseconds.</param>
        /// <param name="millisRed">Duration of Red state before next Green in milliseconds. Should be higher if only using a single traffic light.</param>
        public void StartTrafficLightsGerman(byte outputGreen1, byte outputYellow1, byte outputRed1, byte outputGreen2, byte outputYellow2, byte outputRed2, int millisGreen, int millisYellow, int millisRed)
        {
            var a = new ICCActionLightingScenarioStartTrafficLights() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_TRFCDEU, new int[] { outputGreen1, outputYellow1, outputRed1, outputGreen2, outputYellow2, outputRed2, millisGreen, millisYellow, millisRed } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start a chaser. Multiple modes allowed : linear, progressive, circular. Shows flashes if millisOn < millisStep.
        /// </summary>
        /// <param name="output1">Output pin 1.</param>
        /// <param name="output2">Output pin 2.</param>
        /// <param name="output3">Output pin 3.</param>
        /// <param name="output4">Output pin 4.</param>
        /// <param name="output5">Output pin 5.</param>
        /// <param name="output6">Output pin 6.</param>
        /// <param name="output7">Output pin 7.</param>
        /// <param name="output8">Output pin 8.</param>
        /// <param name="millisStep">Duration of each step in milliseconds.</param>
        /// <param name="millisOn">Duration of ON state in milliseconds. Should never be higher than millisStep value. Shows flashes if less than millisStep.</param>
        /// <param name="millisPause">Duration of pause between cycles in milliseconds. Not used in circular mode.</param>
        /// <param name="coexistence">Use HEX representation. MSB should be 0=Linear, 1=Circular, 2=Progressive. LSB is the number of outputs that should be used. (Ex.: 0x06 is linear mode with 6 outputs. 0x14 is circular mode with 4 outputs.)</param>
        public void StartChaser(int output1, int output2, int output3, int output4, int output5, int output6, int output7, int output8, int millisStep, int millisOn, int millisPause, int coexistence)
        {
            var a = new ICCActionLightingScenarioStartChaser() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_CHASER, new int[] { output1, output2, output3, output4, output5, output6, output7, output8, millisStep, millisOn, millisPause, coexistence } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start an Arc Welding simulator with random flashes and random pauses between flash sequences.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="millisOn">Duration of each flash in milliseconds.</param>
        /// <param name="millisOff">Duration of pause between each flash in milliseconds.</param>
        /// <param name="pauseMin">Minimum duration of pause between welding cycles in milliseconds.</param>
        /// <param name="pauseMax">Maximum duration of pause between welding cycles in milliseconds.</param>
        /// <param name="flashMin">Minimum number of flashes in a sequence.</param>
        /// <param name="flashMax">Maximum number of flashes in a sequence.</param>
        public void StartArcWelding(int output, int millisOn, int millisOff, int pauseMin, int pauseMax, int flashMin, int flashMax)
        {
            var a = new ICCActionLightingScenarioStartArcWelding() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_ARCWELDING, new int[] { output, millisOn, millisOff, pauseMin, pauseMax, flashMin, flashMax } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start a Camera Flash simulator with random pause between each flash.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="millisOn">Duration of each flash in milliseconds.</param>
        /// <param name="pauseMin">Minimum duration of pause between each flash in milliseconds.</param>
        /// <param name="pauseMax">Maximum duration of pause between each flash in milliseconds.</param>
        public void StartCameraFlash(int output, int millisOn, int pauseMin, int pauseMax)
        {
            var a = new ICCActionLightingScenarioStartCameraFlash() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_CAMERAFLASH, new int[] { output, millisOn, pauseMin, pauseMax } } };
            RaiseAction(a);
        }

        /// <summary>
        /// Start a Fire simulator. For best results use a red and an orange LED.
        /// </summary>
        /// <param name="output1">Output pin 1.</param>
        /// <param name="output2">Output pin 2.</param>
        /// <param name="fireSpeed">Fire speed = time between each variation in milliseconds.</param>
        public void StartFire(int output1, int output2, int fireSpeed)
        {
            var a = new ICCActionLightingScenarioStartFire() { Address = (byte)ID, Log = Log, Data = new object[] { ICCConstants.BICCP_SCN_LIGHT_FIRE, new int[] { output1, output2, fireSpeed } } };
            RaiseAction(a);
        }
    }
}
