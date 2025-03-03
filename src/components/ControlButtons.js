const ControlButtons = ({ onStart, onStop,disabledEvent, buttonStyle }) => (
  <div>
    <button onClick={onStart} disabled={disabledEvent} style={buttonStyle}  >Start Charging</button>
    <button onClick={onStop}  disabled={!disabledEvent} style={buttonStyle}>Stop Charging</button>
  </div>
);
export default ControlButtons;