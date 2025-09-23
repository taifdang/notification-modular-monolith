import { useState } from "react";
import { ErrorStep } from "#shared/components/stepper/ErrorStep";
import { StepItem } from "#shared/components/stepper/StepItem";

export default function RightSide() {
  const [steps, setSteps] = useState([
    {
      id: 1,
      title: "Topup Confirm",
      status: true,
      error: "Couldn't connect database.",
      isActive: null,
    },
    {
      id: 2,
      title: "Update Balance",
      status: true,
      error: "Couldn't connect database.",
      isActive: null,
    },
    {
      id: 3,
      title: "Push Notification",
      status: false,
      error: "Couldn't connect database.",
      isActive: null,
    },
  ]);

  const errorSteps = steps.filter(
    (step) => step.status === false && step.error
  );

  return (
    <aside className="col-12 col-md-3 pt-4 px-3 d-none d-md-block">
      <div className="d-none d-md-block mb-4">
        {steps.map((step, index) => (
          <StepItem
            key={step.id}
            step={step}
            index={index}
            length={steps.length - 1}
          />
        ))}
        <ErrorStep errorSteps={errorSteps} />
      </div>
    </aside>
  );
}



