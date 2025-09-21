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
      status: false,
      error: "Couldn't connect database.",
      isActive: null,
    },
    {
      id: 3,
      title: "Push Notification",
      status: null,
      error: null,
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

{
  /* <ol class="stepper">
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
</ol>; */
}
