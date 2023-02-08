using Neurorehab.Scripts.Devices.Abstracts;
using Neurorehab.Scripts.Devices.Data;
using Neurorehab.Scripts.Enums;

namespace Neurorehab.Device_Bitalino.Scripts
{
    /// <summary>
    /// The controller of all the <see cref="BitalinoData"/>. Responsible for creating, deleting and updating all the <see cref="BitalinoData"/> according to what is receiving by UDP.
    /// </summary>
    public class Bitalino : GenericDeviceController
    {
        protected override void Awake()
        {
            base.Awake();

            DeviceName = Devices.bitalino.ToString();
        }
    }
}