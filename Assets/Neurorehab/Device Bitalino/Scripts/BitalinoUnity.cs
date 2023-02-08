using System;
using Neurorehab.Scripts.Devices.Abstracts;
using Neurorehab.Scripts.Devices.Data;
//using Neurorehab.Scripts.Plotter;
using UnityEngine;
using UnityEngine.UI;

namespace Neurorehab.Device_Bitalino.Scripts
{
    /// <summary>
    /// Responsible for updating the Unity Bitalino data according to its <see cref="BitalinoData"/>.
    /// </summary>
    public class BitalinoUnity : GenericDeviceUnity
    {
        [Header("Settings")]
        public Text A0Value;
        public Text A1Value;
        public Text A3Value;
        public Text A4Value;
        public Text D0Value;
        public Text D1Value;
        public Text D2Value;
        public Text D3Value;
        public Text BatValue;

        [Header("GUI")]
        public Text IdValue;


        /// <summary>
        /// Initializes all the plotters
        /// </summary>
        private void Start()
        {
            IdValue.text = "BITALINO - ID: " + GenericDeviceData.Id;
        }

        
        private void LateUpdate()
        {
            try
            {
                UpdateGuiValues();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Updates the values of each bioplux signal in the GUI according to its <see cref="GenericDeviceUnity.GenericDeviceData"/> 
        /// </summary>
        private void UpdateGuiValues()
        {
            A0Value.text = Neurorehab.Scripts.Enums.Bitalino.a0.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.a0.ToString());

            A1Value.text = Neurorehab.Scripts.Enums.Bitalino.a1.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.a0.ToString());

            A3Value.text = Neurorehab.Scripts.Enums.Bitalino.a3.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.a1.ToString());

            A4Value.text = Neurorehab.Scripts.Enums.Bitalino.a4.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.a3.ToString());

            D0Value.text = Neurorehab.Scripts.Enums.Bitalino.d0.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.d0.ToString());

            D1Value.text = Neurorehab.Scripts.Enums.Bitalino.d1.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.d1.ToString());

            D2Value.text = Neurorehab.Scripts.Enums.Bitalino.d2.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.d2.ToString());

            D3Value.text = Neurorehab.Scripts.Enums.Bitalino.d3.ToString().ToUpper() + ": " +
                           GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.d3.ToString());

            BatValue.text = Neurorehab.Scripts.Enums.Bitalino.battery.ToString().ToUpper() + ": " +
                            GenericDeviceData.GetFloat(Neurorehab.Scripts.Enums.Bitalino.battery.ToString());
        }
    }
}