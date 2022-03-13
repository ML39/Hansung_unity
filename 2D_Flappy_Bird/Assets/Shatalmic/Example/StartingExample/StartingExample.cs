/* This is a simple example to show the steps and one possible way of
 * automatically scanning for and connecting to a device to receive
 * notification data from the device.
 *
 * It works with the esp32 sketch included at the bottom of this source file.
 */

using System;
using UnityEngine;
using UnityEngine.UI;

public class StartingExample : MonoBehaviour
{
    //public string DeviceName = "SensorTag 2.0"; //"ledbtn";
    public string DeviceName = "Lungrow-data-comm"; //"ledbtn";
    public string ServiceUUID = "0000aa40-0000-1000-8000-00805f9b34fb";
    //public string ServiceUUID = "f000aa20-0451-4000-b000-000000000000"; // SensorTag Humidity sensor service
    public string CharDataUUID = "0000aa41-0000-1000-8000-00805f9b34fb"; // Humidity data characterisitic UUID
    public string CharConfigUUID = "0000aa42-0000-1000-8000-00805f9b34fb"; // Humidity configuration characteristic UUID
    

    enum States
    {
        None,
        Scan,
        ScanRSSI,
        ReadRSSI,
        Connect,
        RequestMTU,
        Subscribe,
        Unsubscribe,
        Disconnect,
    }

    private bool _connected = false;
    private float _timeout = 100f;
    private States _state = States.None;
    private string _deviceAddress;
    private bool _foundCharDataUUID = false;
    private bool _foundCharHumidConfigUUID = false;
    private bool _rssiOnly = false;
    private int _rssi = 0;

    public Text StatusText;
    public Text ButtonPositionText;

    public int exhaleCnt;
    public int inhaleCnt;
    public float exhaleSpd;
    public float inhaleSpd;

    private string StatusMessage
    {
        set
        {
            BluetoothLEHardwareInterface.Log(value);
            StatusText.text = value;
        }
    }

    void Reset()
    {
        _connected = false;
        _timeout = 0.1f;
        _state = States.None;
        _deviceAddress = null;
        _foundCharDataUUID = false;
       _foundCharHumidConfigUUID = false;
        _rssi = 0;
    }

    void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    void StartProcess()
    {
        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {

            SetState(States.Scan, 0.1f);

        }, (error) =>
        {

            StatusMessage = "Error during initialize: " + error;
        });
    }

    // Use this for initialization
    void Start()
    {
        StartProcess();
    }

    private void ProcessData(byte[] bytes)
    {
    
        int exhaleCount = bytes[0]*256 + bytes[1]; // exhale count
        int inhaleCount = bytes[2]*256 + bytes[3]; // inhale count
        float conversionFactor = 0.0127f; // conversion factor from measurement to exhale speed in L/s
        float exhaleSpeed = (bytes[4]*256 + bytes[5])/10f*conversionFactor; // exhale speed
        float inhaleSpeed = (bytes[6]*256 + bytes[7])/10f*conversionFactor; // inhale speed
        // data formatting
        string exhaleCountString = exhaleCount.ToString("00000").TrimStart('0');
        string inhaleCountSring = inhaleCount.ToString("00000").TrimStart('0');
        string exhaleSpeedString = exhaleSpeed.ToString("0.00");
        string inhaleSpeedString = inhaleSpeed.ToString("0.00");

        string str = String.Format("ex-cnt:{0},in-cnt:{1},ex-spd:{2},ex:spd:{3}",
                                     exhaleCountString, inhaleCountSring,exhaleSpeedString,inhaleSpeedString);
        ButtonPositionText.text = str;

        exhaleCnt = int.Parse(exhaleCountString);
        inhaleCnt = int.Parse(inhaleCountSring);
        exhaleSpd = float.Parse(exhaleSpeedString);
        inhaleSpd = float.Parse(inhaleSpeedString);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;

                switch (_state)
                {
                    case States.None:
                        break;

                    case States.Scan:
                        StatusMessage = "Scanning FOR " + DeviceName;

                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                        {
                            // if your device does not advertise the rssi and manufacturer specific data
                            // then you must use this callback because the next callback only gets called
                            // if you have manufacturer specific data
                            

                            if (!_rssiOnly)
                            {
                                StatusMessage = "name : " + name  + " address: " + address;


                                if (name.Contains(DeviceName))
                                {
                                    StatusMessage = "Found " + name;

                                    BluetoothLEHardwareInterface.StopScan();

                                    // found a device with the name we want
                                    // this example does not deal with finding more than one
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }

                        }, (address, name, rssi, bytes) =>
                        {

                            // use this one if the device responses with manufacturer specific data and the rssi

                            if (name.Contains(DeviceName))
                            {
                                StatusMessage = "Found " + name;

                                if (_rssiOnly)
                                {
                                    _rssi = rssi;
                                }
                                else
                                {
                                    BluetoothLEHardwareInterface.StopScan();

                                    // found a device with the name we want
                                    // this example does not deal with finding more than one
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }

                        }, _rssiOnly); // this last setting allows RFduino to send RSSI without having manufacturer data

                        if (_rssiOnly)
                            SetState(States.ScanRSSI, 0.5f);
                        break;

                    case States.ScanRSSI:
                        break;

                    case States.ReadRSSI:
                        StatusMessage = $"Call Read RSSI";
                        BluetoothLEHardwareInterface.ReadRSSI(_deviceAddress, (address, rssi) =>
                        {
                            StatusMessage = $"Read RSSI: {rssi}";
                        });

                        SetState(States.ReadRSSI, 2f);
                        break;

                    case States.Connect:
                        StatusMessage = "Connecting...";

                        // set these flags
                        _foundCharDataUUID = false;
                       _foundCharHumidConfigUUID = false;

                        // note that the first parameter is the address, not the name. I have not fixed this because
                        // of backwards compatiblity.
                        // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                        // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                        // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                        {


                            if (IsEqual(serviceUUID, ServiceUUID))
                            {
                                StatusMessage = "Found Service UUID";

                                _foundCharDataUUID = _foundCharDataUUID || IsEqual(characteristicUUID, CharDataUUID);
                               _foundCharHumidConfigUUID =_foundCharHumidConfigUUID || IsEqual(characteristicUUID,CharConfigUUID);

                                // if we have found both characteristics that we are waiting for
                                // set the state. make sure there is enough timeout that if the
                                // device is still enumerating other characteristics it finishes
                                // before we try to subscribe
                                if (_foundCharDataUUID && _foundCharHumidConfigUUID)
                                {
                                    _connected = true;
                                    //SetState(States.RequestMTU, 2f);
                                    // SetState(States.Subscribe, 0.1f);
                                     StatusText.text = "data UUID:" + _foundCharDataUUID + "\n"  + 
                                                                    " config UUID:" + _foundCharHumidConfigUUID  + "\n" +
                                                                    "SCAN SUCCESS !";
                                    // StatusText.text = "SRV UUID:" + serviceUUID + "\n"  + " char UUID:" + characteristicUUID ;
                                }
                            }
                        });
                        break;

                    case States.RequestMTU:
                        StatusMessage = "Requesting MTU";

                        BluetoothLEHardwareInterface.RequestMtu(_deviceAddress, 185, (address, newMTU) =>
                        {
                            StatusMessage = "MTU set to " + newMTU.ToString();

                            SetState(States.Subscribe, 0.1f);
                        });
                        break;

                    case States.Subscribe:
                        StatusMessage = "Subscribing to characteristics...";
                        

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, ServiceUUID, CharDataUUID, (notifyAddress, notifyCharacteristic) =>
                        {
                            StatusMessage = "Waiting for data to change: notification READY";
                            _state = States.None;

                           // read the initial state of the button
                            // BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, ServiceUUID, CharDataUUID, (characteristic, bytes) =>
                            // {
                            //     ProcessData(bytes);
                            // });
                    

                            //SetState(States.ReadRSSI, 1f);

                        }, (address, characteristicUUID, bytes) =>
                        {
                            if (_state != States.None)
                            {
                                // some devices do not properly send the notification state change which calls
                                // the lambda just above this one so in those cases we don't have a great way to
                                // set the state other than waiting until we actually got some data back.
                                // The esp32 sends the notification above, but if yuor device doesn't you would have
                                // to send data like pressing the button on the esp32 as the sketch for this demo
                                // would then send data to trigger this.
                                SetState(States.ReadRSSI, 1f);
                            }

                            // we received some data from the device
                             StatusMessage = "Notification arrived !";
                            ProcessData(bytes);
                        });
                        break;

                    case States.Unsubscribe:
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, ServiceUUID, CharDataUUID, null);
                        SetState(States.Disconnect, 4f);
                        break;

                    case States.Disconnect:
                        StatusMessage = "Commanded disconnect.";

                        if (_connected)
                        {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                            {
                                StatusMessage = "Device disconnected";
                                BluetoothLEHardwareInterface.DeInitialize(() =>
                                {
                                    _connected = false;
                                    _state = States.None;
                                });
                            });
                        }
                        else
                        {
                            BluetoothLEHardwareInterface.DeInitialize(() =>
                            {
                                _state = States.None;
                            });
                        }
                        break;
                }
            }
        }
    }

    private bool ledON = false;
    public void EnableHumiditySensor()
    {
        //
        StatusText.text = "IR sensor is enabled.";
        byte enabled = 0x01;
        WriteChar(CharConfigUUID, enabled); // enable IR sensor
    }

    string FullUUID(string uuid)
    {
        string fullUUID = uuid;
        if (fullUUID.Length == 4)
            fullUUID = "0000" + uuid + "-0000-1000-8000-00805f9b34fb";

        return fullUUID;
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    void SendByte(byte value)
    {
        byte[] data = { value };
        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID,CharConfigUUID, data, data.Length, true, (characteristicUUID) =>
        {

            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }

    // Read characteristics
    void ReadChar(string CharUUID)
    {
        BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, ServiceUUID, CharUUID, (characteristic, bytes) =>
        {
                StatusText.text = "char read: " + bytes[0];
        });

    
    }

    // Read ** characteristics
    public void ReadCharHumidityConfig()
    {
        ReadChar(CharConfigUUID);
    }



    // Write characteristics
    void WriteChar(string uuid, byte value )
    {
        byte[] data ={ value };
       //string uuid = "f000aa22-0451-4000-b000-000000000000";
        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID, uuid, data, data.Length, true, (characteristicUUID) =>
        {

            ButtonPositionText.text ="write success ...";
        });
    }


    // Subscribe
    public void Subscribe()
    {
        if(_connected)
          SetState(States.Subscribe, 0.1f);
        else
            StatusText.text = "Not connected !";
    }

    public void Disconnect()  {
        if(_connected)
            SetState(States.Disconnect, 0.1f);
        else
            StatusText.text = "Not connected !";
    }

    public void Scan() {
        StartProcess();
        StatusText.text = "Connected !";
    }
}

/*

COPY FROM BELOW THIS LINE >>>
// This sketch is a companion to the Bluetooth LE for iOS, tvOS and Android plugin for Unity3D.
// It is the hardware side of the StartingExample.

// The URL to the asset on the asset store is:
// https://assetstore.unity.com/packages/tools/network/bluetooth-le-for-ios-tvos-and-android-26661

// This sketch simply advertises as ledbtn and has a single service with 2 characteristics.
// TheCharConfigUUID characteristic is used to turn the LED on and off. Writing 0 turns it off and 1 turns it on.
// The CharDataUUID characteristic can be read or subscribe to. When the button is down the characteristic
// value is 1. When the button is up the value is 0.

// This sketch was written for the Hiletgo ESP32 dev board found here:
// https://www.amazon.com/HiLetgo-ESP-WROOM-32-Development-Microcontroller-Integrated/dp/B0718T232Z/ref=sr_1_3?keywords=hiletgo&qid=1570209988&sr=8-3

// Many other ESP32 devices will work fine.

#include "BLEDevice.h"
#include "BLE2902.h"

// pin 2 on the Hiletgo
// (can be turned on/off from the iPhone app)
const uint32_t led = 2;

// pin 5 on the RGB shield is button 1
// (button press will be shown on the iPhone app)
const uint32_t button = 0;

static BLEUUID serviceUUID("A9E90000-194C-4523-A473-5FDF36AA4D20");
static BLEUUIDCharConfigUUID("A9E90001-194C-4523-A473-5FDF36AA4D20");
static BLEUUID CharDataUUID("A9E90002-194C-4523-A473-5FDF36AA4D20");

bool deviceConnected = false;
bool oldDeviceConnected = false;

bool lastButtonState = false;

BLEServer* pServer = 0;
BLECharacteristic* pCharacteristicCommand = 0;
BLECharacteristic* pCharacteristicData = 0;

class BTServerCallbacks : public BLEServerCallbacks
{
    void onConnect(BLEServer* pServer)
{
    Serial.println("Connected...");
    deviceConnected = true;
};

void onDisconnect(BLEServer* pServer)
{
    Serial.println("Disconnected...");
    deviceConnected = false;

    // don't leave the led on if they disconnect
    digitalWrite(led, LOW);
}
};


class BTCallbacks : public BLECharacteristicCallbacks
{
    void onRead(BLECharacteristic* pCharacteristic)
{
}

void onWrite(BLECharacteristic* pCharacteristic)
{
    uint8_t* data = pCharacteristic->getData();
    int len = pCharacteristic->getValue().empty() ? 0 : pCharacteristic->getValue().length();

    if (len > 0)
    {
        // if the first byte is 0x01 / on / true
        if (data[0] == 0x01)
            digitalWrite(led, HIGH);
        else
            digitalWrite(led, LOW);
    }
}
};

// debounce time (in ms)
int debounce_time = 10;

// maximum debounce timeout (in ms)
int debounce_timeout = 100;

void BluetoothStartAdvertising()
{
    if (pServer != 0)
    {
        BLEAdvertising* pAdvertising = pServer->getAdvertising();
        pAdvertising->start();
    }
}

void BluetoothStopAdvertising()
{
    if (pServer != 0)
    {
        BLEAdvertising* pAdvertising = pServer->getAdvertising();
        pAdvertising->stop();
    }
}

void setup()
{
    Serial.begin(115200);

    // led turned on/off from the iPhone app
    pinMode(led, OUTPUT);

    // button press will be shown on the iPhone app)
    pinMode(button, INPUT);

    BLEDevice::init("ledbtn");
    // BLEDevice::setCustomGattsHandler(my_gatts_event_handler);
    // BLEDevice::setCustomGattcHandler(my_gattc_event_handler);

    pServer = BLEDevice::createServer();
    BLEService* pService = pServer->createService(serviceUUID);
    pServer->setCallbacks(new BTServerCallbacks());

    pCharacteristicCommand = pService->createCharacteristic(
        CharDataUUID,
        BLECharacteristic::PROPERTY_READ |
            BLECharacteristic::PROPERTY_WRITE |
            BLECharacteristic::PROPERTY_NOTIFY);

    pCharacteristicCommand->setCallbacks(new BTCallbacks());
    pCharacteristicCommand->setValue("");
    pCharacteristicCommand->addDescriptor(new BLE2902());

    pCharacteristicData = pService->createCharacteristic(
       CharConfigUUID,
        BLECharacteristic::PROPERTY_READ |
            BLECharacteristic::PROPERTY_WRITE |
            BLECharacteristic::PROPERTY_NOTIFY);

    pCharacteristicData->setCallbacks(new BTCallbacks());
    pCharacteristicData->setValue("");
    pCharacteristicData->addDescriptor(new BLE2902());

    pService->start();
    BluetoothStartAdvertising();
}

void loop()
{
    if (pServer != 0)
    {
        // disconnecting
        if (!deviceConnected && oldDeviceConnected)
        {
            delay(500);                  // give the bluetooth stack the chance to get things ready
            pServer->startAdvertising(); // restart advertising
            Serial.println("start advertising");
            oldDeviceConnected = deviceConnected;
        }

        // connecting
        if (deviceConnected && !oldDeviceConnected)
        {
            oldDeviceConnected = deviceConnected;
        }

        uint8_t buttonState = digitalRead(button);

        if (deviceConnected && pCharacteristicCommand != 0 && buttonState != lastButtonState)
        {
            lastButtonState = buttonState;

            uint8_t packet[1];
            packet[0] = buttonState == HIGH ? 0x00 : 0x01;
            pCharacteristicCommand->setValue(packet, 1);
            pCharacteristicCommand->notify();
        }
    }
}

<<< COPY TO ABOVE THIS LINE
*/
