import { useState } from "react";
export default function RightSide() {
  //Data
  const [steps, setSteps] = useState([
    {
      id: 1,
      title: "Topup Confirm",
      status: false,
      error: "Couldn't connect database",
      isActive: null,
    },
    {
      id: 2,
      title: "Update Balance",
      status: true,
      error: "Couldn't connect database",
      isActive: null,
    },
    {
      id: 3,
      title: "Push Notification",
      status: null,
      error: null,
      isActive: true,
    },
  ]);
  const getStepIcon = (step, index) => {
    if (step.isActive) {
      return (
        <div className="step-icon active pulse-animation">
            <div role="status">
                <i className="fas fa-spinner fa-spin"></i> 
            </div>
        </div>
      );
    }
    if (step.status === true) {
      return (
        <div className="step-icon success">
          <i className="fas fa-check"></i>
        </div>
      );
    }
    if (step.status === false) {
      return (
        <div className="step-icon error">
          <i className="fas fa-times"></i>
        </div>
      );
    }
    //Content in box
    return (
      <div className="step-icon pending" style={{ fontSize: "13.125px" }}>
        {index + 1}
      </div>
    );
  };

  //Processing active class
  const getConnectorClass = (index, isHorizontal = false) => {
    if (index >= steps.length - 1) return "";

    const currentStepStatus = steps[index].status;
    const baseClass = isHorizontal
      ? "step-connector-horizontal"
      : "step-connector-vertical";

    if (currentStepStatus === true) {
      return `${baseClass} active`;
    }

    return baseClass;
  };
  
  //Get list error step
  const errorSteps = steps.filter(
    (step) => step.status === false && step.error
  );

  return (
    <aside className="col-12 col-md-3 pt-4 px-3 d-none d-md-block">
      <div className="d-none d-md-block mb-4">
        {/* Stepper */}
        {
          steps.map((step, index) => (
          <div key={step.id} className="d-flex">
            <div className="d-flex flex-column align-items-center me-4">
              {getStepIcon(step, index)}
              {index < steps.length - 1 && (
                <div className={getConnectorClass(index, false)}></div>
              )}
            </div>
            <div className="flex-grow-1 pb-3">
              <div
                className={`mb-1 ${
                  step.status === true
                    ? "text-success"
                    : step.status === false
                    ? "text-danger"
                    : step.isActive
                    ? "text-primary"
                    : "text-dark"
                }`}
                style={{ fontSize: "13.125px", fontWeight: 600 }}
              >
                {step.title}
              </div>

              {step.isActive && (
                <small 
                className="text-primary"
                style={{ fontSize: "11.125px", fontWeight: 500 }}
                >In Progress...</small>
              )}
              {step.status === true && (
                <small 
                className="text-success"
                style={{ fontSize: "11.125px", fontWeight: 500 }}
                >Success</small>
              )}
              {step.status === false && (
                <small 
                className="text-danger" 
                style={{ fontSize: "11.125px", fontWeight: 500 }}>Failure</small>
              )}
            </div>
          </div>
        ))}
        {/* Error Section - Display all errors at bottom */}
          {errorSteps.length > 0 && (
            <div className="error-section">
              <hr className="my-3" />
              <div 
              className="d-flex align-items-center mb-2 pt-2"
              style={{fontSize:'15px',fontWeight:'400'}}>
                <div className="text-danger">
                  <i className="bi bi-exclamation-triangle-fill me-2"></i>
                </div>
                
                <div className="d-flex flex-column gap-2">
                  <div style={{fontSize:'13.125px',fontFamily:'"Inter',fontWeight:600}}>Partially completed</div>
                 
                </div>
                <button 
                className="m-auto border px-3"
                style={{fontSize:'13.125px',fontFamily:'"Inter',fontWeight:500,borderRadius:'4px'}}>Retry</button>
              </div>
               {errorSteps.map((step) => (
                    <div
                      key={`error-${step.id}`}
                      className=" py-0 mb-2"
                      style={{fontSize:'11px',fontWeight:400,color: 'grey'}}
                      role="alert"
                    >
                      <span>Last updated: 2025/9/18, 16:33</span><br/>
                      <span>{step.error}</span> <a href="#">Learn why</a>
                    </div>
                  ))}
            </div>
          )}    
      </div>
    </aside>
  );
}


<ol class="stepper">
  <li class="stepper__item">
    <div class="stepper__circle">
      <div class="spinner-border" role="status"></div>
    </div>
    <div class="stepper__content">
      <h3 class="stepper__title">Topup Confirm</h3>
      <p class="stepper__desc">In Progress</p>
    </div>
  </li>
  <li class="stepper__item">
    <div class="stepper__circle"></div>
    <div class="stepper__content">
      <h3 class="stepper__title">STEP 2</h3>
      <p class="stepper__desc">Update Balance</p>
    </div>
  </li>
  <li class="stepper__item">
    <div class="stepper__circle"></div>
    <div class="stepper__content">
      <h3 class="stepper__title">STEP 3</h3>
      <p class="stepper__desc">Push Notification</p>
    </div>
  </li>
</ol>;

