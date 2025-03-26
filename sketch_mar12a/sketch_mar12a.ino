#define button D6
#define volume A0

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  pinMode(volume, INPUT);
  pinMode(button, INPUT);
  //pinMode(13, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  bool b = digitalRead(button);
  int v = analogRead(volume);
  Serial.print("B: ");
  Serial.print(b);
  Serial.print(", V: ");
  Serial.println(v);
  delay(50);
}
