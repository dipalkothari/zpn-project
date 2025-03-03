const StatusDisplay = ({ status, energy }) => (
    <div>
      <h2>Status: {status}</h2>
      <h3>Energy Consumed: {energy} kWh</h3>
    </div>
  );
  
  export default StatusDisplay;