#include <Servo.h>

Servo servo_1;
Servo servo_2;
const int SERVO_PIN1 = 10;
const int SERVO_PIN2 = 9;
int test_angle = -999;
int compressed_angle = 0;

const int ledPin = 13;
int ledState = 0;

String temp_String = ",";


void setup() {
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600);
  servo_1.attach(SERVO_PIN1);
  servo_2.attach(SERVO_PIN2);
}

void loop() {
//  servo_1.write(90);
  if (Serial.available()) {
    String serialData = Serial.readStringUntil('\n');
    int commaIndex = serialData.indexOf(',');

    if (commaIndex > -1) {
      String strAngX = serialData.substring(0, commaIndex);
      String strAngY = serialData.substring(commaIndex + 1);

      int angX = strAngX.toInt();
      int angY = strAngY.toInt();

      int compressed_angX = map(angX, 90, -90, 0, 180);
      int compressed_angY = map(angY, 90, -90, 0, 180);

      servo_1.write(compressed_angY);
      servo_2.write(compressed_angX);
      
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
