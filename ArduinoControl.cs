//credit: https://gist.github.com/tanyuan/2ba771a7c8fca2c8f10aead24e75db59

using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoControl : MonoBehaviour
{
    public string portName;
    SerialPort arduino;


    // Create head and both hands controller
    public GameObject headController;
    public GameObject leftController;
    public GameObject rightController;


    // for computing head
    private float eulerAngX;
    private float eulerAngY;
    private float eulerAngZ;

    private float avg_eulerAngX = 0;
    private float avg_eulerAngY = 0;
    private float avg_eulerAngZ = 0;

    private float sum_eulerAngX = 0;
    private float sum_eulerAngY = 0;
    private float sum_eulerAngZ = 0;

    // for computing left side arm (hand + shoulder)
    private float left_eulerAngX;
    private float left_eulerAngY;
    private float left_eulerAngZ;

    private float left_avg_eulerAngX = 0;
    private float left_avg_eulerAngY = 0;
    private float left_avg_eulerAngZ = 0;

    private float left_sum_eulerAngX = 0;
    private float left_sum_eulerAngY = 0;
    private float left_sum_eulerAngZ = 0;

    // TODO: compute right side arm (hand + shoulder)


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
        // match direction of headset to the robot head
        eulerAngX = -(headController.transform.localEulerAngles.x);
        eulerAngY = headController.transform.localEulerAngles.y;
        eulerAngZ = headController.transform.localEulerAngles.z;

        left_eulerAngX = -(leftController.transform.localEulerAngles.x);
        left_eulerAngY = leftController.transform.localEulerAngles.y;
        left_eulerAngZ = leftController.transform.localEulerAngles.z;


        //Compute for Calibration to set starting as 0 0 0 orienation
        if (counter < maxCount && eulerAngX != 0 && eulerAngY != 0 && eulerAngZ != 0)
        {
            //Debug.Log(eulerAngX + "|" + eulerAngY + "|" + eulerAngZ);
            sum_eulerAngX += eulerAngX;
            sum_eulerAngY += eulerAngY;
            sum_eulerAngZ += eulerAngZ;

            left_sum_eulerAngX += left_eulerAngX;
            left_sum_eulerAngY += left_eulerAngY;
            left_sum_eulerAngZ += left_eulerAngZ;

            counter++;
        }
        // Reached calibration timer, compute the average, this will be used as initial baseline (0 degree)
        else if (counter == maxCount)
        {
            //Debug.Log(counter);
            avg_eulerAngX = sum_eulerAngX / maxCount;
            avg_eulerAngY = sum_eulerAngY / maxCount;
            avg_eulerAngZ = sum_eulerAngZ / maxCount;

            left_avg_eulerAngX = left_sum_eulerAngX / maxCount;
            left_avg_eulerAngY = left_sum_eulerAngY / maxCount;
            left_avg_eulerAngZ = left_sum_eulerAngZ / maxCount;


            //head debug
            //Debug.Log("Head avg angle: " + avg_eulerAngX + "|" + avg_eulerAngY + "|" + avg_eulerAngZ);
            //left side debug
            Debug.Log("Left abg angle: " + left_avg_eulerAngX + "|" + left_avg_eulerAngY + "|" + left_avg_eulerAngZ);

            counter++;
        }
        else {
            // compute current angle to the baseline calculated
            int angX = (int)(eulerAngX - avg_eulerAngX);
            int left_angX = (int)(left_eulerAngX - left_avg_eulerAngX);


            // flips the quadrants for continnuity
            if (angX >= 270) {
                angX = -(360 - angX);
            }
            if (left_angX >= 270)
            {
                left_angX = -(360 - left_angX);
            }

            int angY = (int)(eulerAngY - avg_eulerAngY);
            int angZ = (int)(eulerAngZ - avg_eulerAngZ);

            int left_angY = (int)(left_eulerAngY - left_avg_eulerAngY);
            int left_angZ = (int)(left_eulerAngZ - left_avg_eulerAngZ);


            Debug.Log("=== Head: " + angX + "|" + angY + "|" + angZ);

            Debug.Log("=== Left Side: " + left_angX + "|" + left_angY + "|" + left_angZ);





            // Wrtie Data to Arduino
            if (arduino.IsOpen)
            {
                //arduino.WriteLine(angY.ToString());
                string dataToSend = angX.ToString() + "," + angY.ToString() + "," + left_angX.ToString() + "," + left_angY.ToString();
                arduino.WriteLine(dataToSend);
                //Debug.Log("----: " + angY.ToString());
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