using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{
    private PluxDeviceManager PluxDevManager;
    public List<int> ActiveChannels;
    public List<string> ListDevices;
    public int samplingRate;
    public int resolution = 8;
    public Text OutputMsgText;
    public Slider SliderSlider;
    public Image image;
    private int BitalinoPID = 1538;
    
    [System.NonSerialized]
    public List<string> domains = new List<string>() { "BTH" };

    // Start is called before the first frame update
    void Start()
    {
        StartConnection();
    }

    private void Update()
    {
        print(SliderSlider.value);
        if (SliderSlider.value >= 0.2f)
        {
            image.color = Color.red;
        }
        else { image.color = Color.green; }
    }

    public void StartConnection()
    {
        PluxDevManager = new PluxDeviceManager(ScanResults, ConnectionDone, AcquisitionStarted, OnDataReceived, OnEventDetected, OnExceptionRaised);
        int welcomeNumber = PluxDevManager.WelcomeFunctionUnity();
        Debug.Log("Connection between C++ Interface and Unity established with success !\n");
        Debug.Log("Welcome Number: " + welcomeNumber);

        // Initialization of Variables.       
        ActiveChannels = new List<int>();
        ScanFunction();
    }
    
    // Callback invoked once the connection with a PLUX device was established.
    // connectionStatus -> A boolean flag stating if the connection was established with success (true) or not (false).
    public void ConnectionDone(bool connectionStatus)
    {
        if (connectionStatus)
        {
            print("connectionstatus " + connectionStatus);
            print("starting the acquisition");
            startACQ();
        }
        else
        {
            print("connectionstatus " + connectionStatus);
        }
    }
    
    public void ConnectButtonFunction()
    {

        // Connect to the device selected in the Dropdown list.
        PluxDevManager.PluxDev("20:18:05:28:74:01");
    }


    // Method called when the "Scan for Devices" button is pressed.
    public void ScanFunction()
    {
        print("scan started");
        // Search for PLUX devices
        PluxDevManager.GetDetectableDevicesUnity(domains);
        print("test devices" + domains);
    }

    // Callback invoked every time an exception is raised in the PLUX API Plugin.
    // exceptionCode -> ID number of the exception to be raised.
    // exceptionDescription -> Descriptive message about the exception.
    public void OnExceptionRaised(int exceptionCode, string exceptionDescription)
    {
        if (PluxDevManager.IsAcquisitionInProgress())
        {
            print(exceptionCode + " " + exceptionDescription);
        }
    }

    // Callback that receives the events raised from the PLUX devices that are streaming real-time data.
    // pluxEvent -> Event object raised by the PLUX API.
    public void OnEventDetected(PluxDeviceManager.PluxEvent pluxEvent)
    {
        if (pluxEvent is PluxDeviceManager.PluxDisconnectEvent)
        {
            // Present an error message.
            print("kaput");

            // Securely stop the real-time acquisition.
            PluxDevManager.StopAcquisitionUnity(-1);

        }
        else if (pluxEvent is PluxDeviceManager.PluxDigInUpdateEvent)
        {
            PluxDeviceManager.PluxDigInUpdateEvent digInEvent = (pluxEvent as PluxDeviceManager.PluxDigInUpdateEvent);
            print("Digital Input Update Event Detected on channel " + digInEvent.channel + ". Current state: " + digInEvent.state);
        }
    }


    public void AcquisitionStarted(bool acquisitionStatus, bool exceptionRaised = false, string exceptionMessage = "")
    {
        if (acquisitionStatus)
        {
            // Enable the "Stop Acquisition" button and disable the "Start Acquisition" button.
            print("acquisition = " + acquisitionStatus);
        }

    }
    
     public void startACQ()
    {
        // Get the Sampling Rate and Resolution values.
        samplingRate = 100;
        resolution = 16;

        // Initializing the sources array.
        List<PluxDeviceManager.PluxSource> pluxSources = new List<PluxDeviceManager.PluxSource>();

        // BITalino (2 Analog sensors)
        if (PluxDevManager.GetProductIdUnity() == BitalinoPID)
        {
            // Starting a real-time acquisition from:
            // >>> BITalino [Channels A2 and A5 active]
            PluxDevManager.StartAcquisitionUnity(samplingRate, new List<int> { 2, 5 }, 10);
        }

    }

    // Callback that receives the data acquired from the PLUX devices that are streaming real-time data.
    // nSeq -> Number of sequence identifying the number of the current package of data.
    // data -> Package of data containing the RAW data samples collected from each active channel ([sample_first_active_channel, sample_second_active_channel,...]).
    public void OnDataReceived(int nSeq, int[] data)
    {
        // Show samples with a 1s interval.
        if (nSeq % samplingRate == 0)
        {
            // Show the current package of data.
            string outputString = "";
            float stringToInt;
            float nseqFloat;
            for (int j = 0; j < data.Length; j++)
            {
                outputString += data[j] + "\t";
            }
            string[] x = outputString.Split('\t');
            print(x[0]);
            float.TryParse(x[0], out stringToInt);
            print(stringToInt);
            stringToInt /= 1000f;
            OutputMsgText.text = outputString;
          // convert nseq if needed test  nseqFloat = (int)nSeq;
        }
    }

    // Callback that receives the list of PLUX devices found during the Bluetooth scan.
    public void ScanResults(List<string> listDevices)
    {
        if (listDevices.Count > 0)
        {
            // Show an informative message about the number of detected devices.
            print("Scan completed.\nNumber of devices found: " + listDevices.Count);
            
            //start connect
            print("connecting.....");
            ConnectButtonFunction();
        }
        else
        {
            // Show an informative message stating the none devices were found.
            print("Bluetooth device scan didn't found any valid devices.");
        }
    }
}
