using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Library.Actions
{
    /// <summary>
    /// Class containing a lighting module start scenario action.
    /// </summary>
    public class ICCActionLightingScenarioStart : ICCActionLighting
    {
        /// <summary>
        /// Instantiates a new ICCActionLightingScenarioStart.
        /// </summary>
        public ICCActionLightingScenarioStart() : base()
        {
            Command = ICCConstants.BICCP_CMD_LIGHT_SCENSTART;
            ICCPCommand = ICCConstants.ICCP_COMMANDS.DO_SCENSTART;
        }

        /// <summary>
        /// Generates the ICCP command for the current action.
        /// </summary>
        /// <returns>ICCP command for the current action.</returns>
        public override string ToICCP()
        {
            return string.Format("DO_SCENSTART {0:X2} {1:X2} {2}\n", Address, (byte)Data[0], string.Join(" ", (int[])Data[1]));
        }

        /// <summary>
        /// Log an argument count error.
        /// </summary>
        /// <param name="parametersCount">Number of expected arguments.</param>
        protected virtual void LogArgumentsError(int parametersCount)
        {
            Log?.LogText(string.Format("ModLightingManager.ModScenarioStart: scenario '{0:X2}' takes {1} parameters.", (byte)Data[0], parametersCount));
        }

        /// <summary>
        /// Log the start of a scenario.
        /// </summary>
        /// <param name="status">Status of the action.</param>
        /// <param name="parameters">Action parameters.</param>
        protected virtual void LogStart(bool status, int[] parameters)
        {
            Log?.LogMessage(status, string.Format("LightingModule: Start scenario '{0:X2}' with parameters {1}", (byte)Data[0], string.Join(", ", parameters.Select(x => x.ToString()))), Address);
        }

        /// <summary>
        /// Factory to create an ICCAction instance depending on Scenario ID.
        /// </summary>
        /// <param name="scenarioId">ICCP Scenario ID.</param>
        /// <param name="address">Module address.</param>
        /// <param name="log">Log Manager.</param>
        /// <returns>An instance of the right ICCAction type.</returns>
        public static IICCAction GetAction(byte scenarioId, byte address, ILogger log = null)
        {
            IICCAction a = null;

            if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_ARCWELDING)
                a = new ICCActionLightingScenarioStartArcWelding();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_BLKLED)
                a = new ICCActionLightingScenarioStartBlinker();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_BLKOLD)
                a = new ICCActionLightingScenarioStartBlinker();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_CAMERAFLASH)
                a = new ICCActionLightingScenarioStartCameraFlash();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_CHASER)
                a = new ICCActionLightingScenarioStartChaser();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_FIRE)
                a = new ICCActionLightingScenarioStartFire();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_PROGCHANGE)
                a = new ICCActionLightingScenarioStartProgChange();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_TRFCDEU)
                a = new ICCActionLightingScenarioStartTrafficLights();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_TRFCFRA)
                a = new ICCActionLightingScenarioStartTrafficLights();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_TUNGOFF)
                a = new ICCActionLightingScenarioStartTungsten();
            else if (scenarioId == ICCConstants.BICCP_SCN_LIGHT_TUNGON)
                a = new ICCActionLightingScenarioStartTungsten();
            else
                throw new NotImplementedException(string.Format("ICCActionLightingScenarioStart doesn't recognize scenario {0:X2}.", scenarioId));

            a.Address = address;
            a.Log = log;

            return a;
        }
    }
}
