import mqtt from 'mqtt';

const mqttClient = mqtt.connect('ws://localhost:9001');

export default mqttClient;