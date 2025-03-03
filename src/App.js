import { useState, useEffect } from 'react';
import ControlButtons from './components/ControlButtons';
import StatusDisplay from './components/StatusDisplay';
import mqttClient from './mqttClient';
import moment from 'moment/moment';

const App = () => {
  const [status, setStatus] = useState('None');
  const [energy, setEnergy] = useState(0);
  const [startTime, setStartTime] = useState('');
  const [disabledView, setDisabledView] = useState(false);
  const buttonStyle = {
    margin:"10px"
  };

  const startCharging = () => {
    setDisabledView(true);
    setStartTime(moment().format());
    setStatus('Charging');
    mqttClient.publish('charging/start', JSON.stringify({ startTime:moment().format(),status: 'Charging',energyConsumed:0 }));
  };

  const stopCharging = () => {

    let endts = moment(moment().format());
    let sts = moment(startTime);
    
    setStatus('Stopped');
    let energyConsumed = endts.diff(sts, 'seconds') * 0.5 ;
    setEnergy(energyConsumed)
    mqttClient.publish('charging/stop', JSON.stringify({ startTime:startTime,endTime:moment().format(),status: 'Stopped',energyConsumed: energyConsumed}));
    setDisabledView(false);
  };

  useEffect(() => {
    console.log('useeffectss')
    mqttClient.on('message', (status, message) => {
      console.log('useeffect',status)
      if (status === 'charging/status') {
        const data = JSON.parse(message.toString());
        setStatus(data.status);
        setEnergy(data.energyConsumed);
      }
    });

    return () => {
      mqttClient.end();
    };
  }, []);

  return (
    <div>
      <h1>EV Charging System</h1>
      <ControlButtons onStart={startCharging} disabledEvent={disabledView} buttonStyle={buttonStyle}  onStop={stopCharging} />
      <StatusDisplay status={status}  energy={energy} />
    </div>
  );
};
export default App;