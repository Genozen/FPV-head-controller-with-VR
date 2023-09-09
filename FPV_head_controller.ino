#include <Servo.h>

Servo servo_1;
const int SERVO_PIN1 = 10;
int test_angle = -999;
int compressed_angle = 0;

const int ledPin = 13;
int ledState = 0;

String temp_String = ",";


void setup() {
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600);
  servo_1.attach(SERVO_PIN1);
}

void loop() {
  if (Serial.available()) {
    String serialData = Serial.readStringUntil('\n');
    test_angle = serialData.toInt();
    compressed_angle = map(test_angle, 90, -90, 0, 180);
    servo_1.write(compressed_angle);
//    delay(15);
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
