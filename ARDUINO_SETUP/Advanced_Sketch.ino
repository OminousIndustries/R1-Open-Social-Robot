//Include the Arduino Stepper Library
#include <Stepper.h>

// Define sound level sensor pins
const int right_sound = 2;  // Assuming the right sensor's OUT is connected to pin 2
const int left_sound = 3;   // Assuming the left sensor's OUT is connected to pin 3

// Define Constants 

// Number of steps per internal motor revolution
const float STEPS_PER_REV = 32;

// Amount of Gear Reduction
const float GEAR_RED = 64;

// Number of steps per geared output rotation
const float STEPS_PER_OUT_REV = STEPS_PER_REV * GEAR_RED;

// Define Variables
// Number of Steps Required
int StepsRequired;

// Create Instance of Stepper Class
// Specify Pins used for motor coils
// The pins used are 8,9,10,11
// Connected to ULN2003 Motor Driver In1, In2, In3, In4
// Pins entered in sequence 1-3-2-4 for proper step sequencing
Stepper steppermotor(STEPS_PER_REV, 8, 10, 9, 11);

void setup() {
  // Set up sound sensor input pins
  pinMode(right_sound, INPUT);
  pinMode(left_sound, INPUT);
  // Initialize Serial for debugging (optional)
  Serial.begin(9600);
}

void loop() {
  bool right_detected = (digitalRead(right_sound) == LOW); // True if sound detected on right
  bool left_detected = (digitalRead(left_sound) == LOW);   // True if sound detected on left

// CW (Clockwise) turns the head to the right
// CCW (Counter-Clockwise) turns the head to the left


  // Check if only the right_sound sensor detects sound
  if (right_detected && !left_detected) {
    // Rotate CW 1/2 turn
    StepsRequired = STEPS_PER_OUT_REV / 1;
    steppermotor.setSpeed(700);
    steppermotor.step(StepsRequired);
    delay(5000); // Wait for 5 Seconds until next instruction

    // Rotate CCW 1/2 turn to return to original position
    StepsRequired = -STEPS_PER_OUT_REV / 1;
    steppermotor.setSpeed(700);
    steppermotor.step(StepsRequired);
    delay(1000); // Wait for 1 Second before next measurement
  }
  // Check if only the left_sound sensor detects sound
  else if (left_detected && !right_detected) {
    // Rotate CCW 1/2 turn
    StepsRequired = -STEPS_PER_OUT_REV / 1;
    steppermotor.setSpeed(700);
    steppermotor.step(StepsRequired);
    delay(5000); // Wait for 5 Seconds until next instruction

    // Rotate CW 1/2 turn to return to original position
    StepsRequired = STEPS_PER_OUT_REV / 1;
    steppermotor.setSpeed(700);
    steppermotor.step(StepsRequired);
    delay(1000); // Wait for 1 Second before next measurement
  }
  // If both sensors detect sound or neither detects sound, do nothing
  else {
    delay(100);
  }
}
