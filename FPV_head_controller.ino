#include <Servo.h>

//create servos
Servo servo_1; //neck
Servo servo_2; // eyes
Servo servo_3; // left shoulder
Servo servo_4; // left hand
Servo servo_5; // right shoulder
Servo servo_6; // right hand

//assign servo pin numbers
const int SERVO_PIN1 = 10; // neck
const int SERVO_PIN2 = 9; // eyes
const int SERVO_PIN3 = 11; // left shoulder
const int SERVO_PIN4 = 6; // left hand
const int SERVO_PIN5 = 5; // right shoulder
const int SERVO_PIN6 = 4; // right hand

int test_angle = -999;
int compressed_angle = 0;

const int ledPin = 13;
int ledState = 0;

String temp_String = ",";


void setup() {
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600);

  // attach servo to corresponding pin #
  servo_1.attach(SERVO_PIN1);
  servo_2.attach(SERVO_PIN2);
  servo_3.attach(SERVO_PIN3);
  servo_4.attach(SERVO_PIN4);
  servo_5.attach(SERVO_PIN5);
  servo_6.attach(SERVO_PIN6);
}

void loop() {
//  servo_1.write(90);
  if (Serial.available()) {
    String serialData = Serial.readStringUntil('\n');
    int commaIndex1 = serialData.indexOf(','); // find the first comma
    int commaIndex2 = serialData.indexOf(',', commaIndex1 + 1); // 2nd comma
    int commaIndex3 = serialData.indexOf(',', commaIndex2 + 1); // 3rd comma
    

    if (commaIndex1 > -1 && commaIndex2 > -1 && commaIndex3 > -1) {
      // cast the substrings between commas for the actual data
      String strAngX = serialData.substring(0, commaIndex1);
      String strAngY = serialData.substring(commaIndex1 + 1, commaIndex2);
      String left_strAngX = serialData.substring(commaIndex2 + 1, commaIndex3);
      String left_strAngY = serialData.substring(commaIndex3 + 1);

      int angX = strAngX.toInt();
      int angY = strAngY.toInt();
      int left_angX = left_strAngX.toInt();
      int left_angY = left_strAngY.toInt();

      int compressed_angX = map(angX, 90, -90, 0, 180);
      int compressed_angY = map(angY, 90, -90, 0, 180);
      int left_compressed_angX = map(left_angX, 90, -90, 0, 180);
      int left_compressed_angY = map(left_angY, 90, -90, 0, 180);

      servo_1.write(compressed_angY); // neck
      servo_2.write(compressed_angX); // eye
      servo_3.write(left_compressed_angY); // left shoulder
      servo_4.write(left_compressed_angX); // left hand
      
//    test_angle = serialData.toInt();
//    compressed_angle = map(test_angle, 90, -90, 0, 180);
//    servo_1.write(compressed_angle);
//    delay(15);
    }
  }
  
//  ledState = recvSerial();
////  Serial.println(ledState);
//  
//  delay(100);
//  if (ledState == 1)
//    digitalWrite(ledPin, HIGH);
//  else
//    digitalWrite(ledPin, LOW);
}

int recvSerial() {
  if (Serial.available()) {
    int serialData = Serial.read();
    switch (serialData) {
      case '1':
        return 1;
        break;
      case '0':
        return 0;
        break;
      default:
        return -1;
    }
  }
}
