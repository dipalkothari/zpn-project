const StatusDisplay = ({ status, energy,data }) => (
    <div>
      <h2>Status: {status}</h2>
      <h3>Energy Consumed: {energy} kWh</h3>
      <h3>Start Time : {data.starttime}</h3>
      <h3>End Time : {data.endtime}</h3>
     
    </div>
  );
  
  export default StatusDisplay;