import mqtt from 'mqtt';
const mqttClient = mqtt.connect('ws://broker.emqx.io:8083/mqtt');

export default mqttClient;