import { useState } from 'react';
import ControlButtons from './components/ControlButtons';
import StatusDisplay from './components/StatusDisplay';
import moment from 'moment/moment';
import mqtt from 'mqtt';



const App = () => {
  const [status, setStatus] = useState('None');
  const [energy, setEnergy] = useState(0);
  const [startTime, setStartTime] = useState('');
  const [data, setData] = useState({});
  const [disabledView, setDisabledView] = useState(false);
  const buttonStyle = {
    margin:"10px"
  };
  const options = {
    clientId :  'emqx_react_' + Math.random().toString(16).substring(2, 8),
    username : 'emqx_test',
    password : 'emqx_test',
    clean: true,
    reconnectPeriod: 1000, // ms
    connectTimeout: 1000 * 1000, // ms
  }
  
  const client = mqtt.connect('wss://broker.emqx.io:8084/mqtt', options);

    async function startCharging() {
    setDisabledView(true);
    setStartTime(moment().format());
    setStatus('Charging');
    setEnergy(0);
    setData({starttime:"",endtime:""});
    console.log(data,'start data');

    client.on('connect', () => {
      console.log('check start publish')
      client.publish('start', 'Charging Start');
     });

    }

    async function stopCharging() {

      let endts = moment(moment().format());
      let sts = moment(startTime);
      
      setStatus('Stopped');
      let energyConsumed = endts.diff(sts, 'seconds') * 0.5 ;
      setEnergy(energyConsumed);
      setData({starttime:startTime,endtime:moment().format()});

      const getData = async () => {
        const response = await fetch('https://zpnwebapiproject.azurewebsites.net/api/ChargingSession?status=charging');
        const result = await response.json();
        return result;
      };
      
      const getDataResult =await getData();
   
      client.on('connect', () => {
        console.log('check stop publish')
        client.publish('stop', JSON.stringify({ endTime:endts,sessionId:getDataResult.sessionId,energyConsumed:energyConsumed}));
       });
  
      setDisabledView(false);
  };
  return (
    <div>
      <h1>EV Charging System</h1>
      <ControlButtons onStart={startCharging} disabledEvent={disabledView} buttonStyle={buttonStyle}  onStop={stopCharging} />
      <StatusDisplay status={status} energy={energy} data={data} />
    </div>
  );
};
export default App;