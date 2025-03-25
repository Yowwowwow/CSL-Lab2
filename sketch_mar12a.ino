#define button 7
#define volume A0

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(volume, INPUT);
  pinMode(button, INPUT);
  //pinMode(13, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  int v = analogRead(volume);
  Serial.println(v);
  delay(10);
  //bool b = digitalRead(button);
  //if(b) digitalWrite(13, HIGH);
  //else digitalWrite(13, LOW);
}
