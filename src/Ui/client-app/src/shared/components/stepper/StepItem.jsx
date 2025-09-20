import { GetStepIcon, GetStepState, GetConnectorClass } from "./Index";

export function StepItem({ step, index, length }) {
  const { textClass, label, labelClass } = GetStepState(step);
  return (
    <div className="d-flex">
      <div className="d-flex flex-column align-items-center me-4">
        {GetStepIcon(step, index)}
        {index < length  && (<div className={GetConnectorClass(index,step.status, length, false)}></div>)}       
      </div>

      <div className="flex-grow-1 pb-3">
        <div
          className={`mb-1 ${textClass}`}
          style={{ fontSize: "13.125px", fontWeight: 600 }}
        >
          {step.title}
        </div>

        {label && (
          <small           
            className={labelClass} 
            style={{ fontSize: "11.125px", fontWeight: 500 }}
          >
            {label}
          </small>
        )}
      </div>
    </div>
  );
}
