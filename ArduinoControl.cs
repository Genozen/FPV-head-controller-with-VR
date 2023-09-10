//credit: https://gist.github.com/tanyuan/2ba771a7c8fca2c8f10aead24e75db59

using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoControl : MonoBehaviour
{
    public string portName;
    SerialPort arduino;


    public GameObject headController;

    private float eulerAngX;
    private float eulerAngY;
    private float eulerAngZ;

    private float avg_eulerAngX = 0;
    private float avg_eulerAngY = 0;
    private float avg_eulerAngZ = 0;

    private float sum_eulerAngX = 0;
    private float sum_eulerAngY = 0;
    private float sum_eulerAngZ = 0;

    private int counter = 0;
    private int maxCount = 50;

    void Start()
    {
        arduino = new SerialPort(portName, 9600);
        arduino.Open();
        Debug.Log("Starting..." + arduino);
    }

    void Update()
    {
        // Debug.Log("Updating");

        eulerAngX = -(headController.transform.localEulerAngles.x);
        eulerAngY = headController.transform.localEulerAngles.y;
        eulerAngZ = headController.transform.localEulerAngles.z;


        //Compute for Calibration to set starting as 0 0 0 orienation
        if (counter < maxCount && eulerAngX != 0 && eulerAngY != 0 && eulerAngZ != 0)
        {
            Debug.Log(eulerAngX + "|" + eulerAngY + "|" + eulerAngZ);
            sum_eulerAngX += eulerAngX;
            sum_eulerAngY += eulerAngY;
            sum_eulerAngZ += eulerAngZ;
            counter++;
        }
        else if (counter == maxCount)
        {
            Debug.Log(counter);
            avg_eulerAngX = sum_eulerAngX / maxCount;
            avg_eulerAngY = sum_eulerAngY / maxCount;
            avg_eulerAngZ = sum_eulerAngZ / maxCount;
            Debug.Log("avg angle: " + avg_eulerAngX + "|" + avg_eulerAngY + "|" + avg_eulerAngZ);
            counter++;
        }
        else {
            //Debug.Log("=====" + (eulerAngX - avg_eulerAngX) + "|" + (eulerAngY - avg_eulerAngY) + "|" + (eulerAngZ - avg_eulerAngZ));
            int angX = (int)(eulerAngX - avg_eulerAngX);
            // flips the quadrants for continnuity
            if (angX >= 270) {
                angX = -(360 - angX);
            }

            int angY = (int)(eulerAngY - avg_eulerAngY);
            int angZ = (int)(eulerAngZ - avg_eulerAngZ);
            Debug.Log("=====" + angX + "|" + angY + "|" + angZ);

            if (arduino.IsOpen)
            {
                //arduino.WriteLine(angY.ToString());
                string dataToSend = angX.ToString() + "," + angY.ToString();
                arduino.WriteLine(dataToSend);
                Debug.Log("----: " + angY.ToString());
                //arduino.Close();
            }
        }



       /* 
        if (arduino.IsOpen)
        {
            if (Input.GetKey("1"))
            {
                arduino.Write("1");
                Debug.Log(1);
            }
            else if (Input.GetKey("0"))
            {
                arduino.Write("0");
                Debug.Log(0);
            }
        }
        */
    }
}