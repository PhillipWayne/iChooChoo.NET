using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.Modules
{
    /// <summary>
    /// Interface for Lighting Modules.
    /// </summary>
    public interface ILightingModule : IOnOffOutputsModule, IDimmableOutputsModule, IScenariosModule
    {
        /// <summary>
        /// Start a progressive light variation from start to target value, using linear or exponential progression.
        /// </summary>
        /// <param name="output">Output pin. (Use only dimmable output pin)</param>
        /// <param name="millisStep">Variation step duration in milliseconds.</param>
        /// <param name="startValue">Start light value.</param>
        /// <param name="targetValue">Target light value (end of variation).</param>
        /// <param name="step">Value of each step. Must be negative if target value is less than start value. For exponential variation, this value will be divided by 100 and multiplicated on each step.</param>
        /// <param name="linear">1 = Linear, 0 = Exponential (value is multiplicated by step/100 on each step).</param>
        void StartProgressiveChange(byte output, int millisStep, byte startValue, byte targetValue, int step, byte linear);

        /// <summary>
        /// Start a tungsten-like turn on light.
        /// </summary>
        /// <param name="output">Output pin. (Use only dimmable output pin)</param>
        void StartTungstenOn(byte output);

        /// <summary>
        /// Start a tungsten-like turn off light.
        /// </summary>
        /// <param name="output">Output pin. (Use only dimmable output pin)</param>
        void StartTungstenOff(byte output);

        /// <summary>
        /// Starts a LED-style blinker (straight on/off) on one or two outputs. Set output2 to zero to blink only one output.
        /// </summary>
        /// <param name="output1">Output pin 1.</param>
        /// <param name="output2">Output pin 2.</param>
        /// <param name="millis1">Duration of pin 1 ON state in milliseconds.</param>
        /// <param name="millis2">Duration of pin 2 ON state in milliseconds (=pin 1 OFF state).</param>
        void StartBlinkerLed(byte output1, byte output2, int millis1, int millis2);

        /// <summary>
        /// Starts a Tungsten-style blinker (progressive on/off) on one or two outputs. Set output2 to zero to blink only one output.
        /// </summary>
        /// <param name="output1">Output pin 1. (Use only dimmable output pin)</param>
        /// <param name="output2">Output pin 2. (Use only dimmable output pin)</param>
        /// <param name="millis1">Duration of pin 1 ON state in milliseconds.</param>
        /// <param name="millis2">Duration of pin 2 ON state in milliseconds (=pin 1 OFF state).</param>
        void StartBlinkerOld(byte output1, byte output2, int millis1, int millis2);

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
        void StartTrafficLightsFrench(byte outputGreen1, byte outputYellow1, byte outputRed1, byte outputGreen2, byte outputYellow2, byte outputRed2, int millisGreen, int millisYellow, int millisRed);

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
        void StartTrafficLightsGerman(byte outputGreen1, byte outputYellow1, byte outputRed1, byte outputGreen2, byte outputYellow2, byte outputRed2, int millisGreen, int millisYellow, int millisRed);

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
        void StartChaser(int output1, int output2, int output3, int output4, int output5, int output6, int output7, int output8, int millisStep, int millisOn, int millisPause, int coexistence);

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
        void StartArcWelding(int output, int millisOn, int millisOff, int pauseMin, int pauseMax, int flashMin, int flashMax);

        /// <summary>
        /// Start a Camera Flash simulator with random pause between each flash.
        /// </summary>
        /// <param name="output">Output pin.</param>
        /// <param name="millisOn">Duration of each flash in milliseconds.</param>
        /// <param name="pauseMin">Minimum duration of pause between each flash in milliseconds.</param>
        /// <param name="pauseMax">Maximum duration of pause between each flash in milliseconds.</param>
        void StartCameraFlash(int output, int millisOn, int pauseMin, int pauseMax);

        /// <summary>
        /// Start a Fire simulator. For best results use a red and an orange LED.
        /// </summary>
        /// <param name="output1">Output pin 1.</param>
        /// <param name="output2">Output pin 2.</param>
        /// <param name="fireSpeed">Fire speed = time between each variation in milliseconds.</param>
        void StartFire(int output1, int output2, int fireSpeed);
    }
}
