/*
 * Get icon in step
 */
export const GetStepIcon = (step, index) => {
  const states = {
    isActive: {
      className: "step-icon active pulse-animation",
      icon: <i className="fas fa-spinner fa-spin"></i>,
    },
    success: {
      className: "step-icon success",
      icon: <i className="fas fa-check"></i>,
    },
    error: {
      className: "step-icon error",
      icon: <i className="fas fa-times"></i>,
    },
    pending: { className: "step-icon pending", icon: index + 1 },
  };

  const key = step.isActive
    ? "isActive"
    : step.status === true
    ? "success"
    : step.status === false
    ? "error"
    : "pending";

  const { className, icon } = states[key];

  return (
    <div className={className} style={{ fontSize: "13.125px" }}>
      {icon}
    </div>
  );
};
/*
 * Get state in step
 */
export const GetStepState = (step) => {
  if (step.isActive)
    return {
      textClass: "text-primary",
      label: "In Progress...",
      labelClass: "text-primary",
    };
  if (step.status === true)
    return {
      textClass: "text-success",
      label: "Success",
      labelClass: "text-success",
    };
  if (step.status === false)
    return {
      textClass: "text-danger",
      label: "Failure",
      labelClass: "text-danger",
    };
  return { textClass: "text-dark", label: null, labelClass: "" };
};
/*
 * Get layout display horizontal/vertical
 */
export const GetConnectorClass = (index, status, length, isHorizontal = false) => {
  if (index >= length) return "";
  const currentStepStatus = status;
  const baseClass = isHorizontal
    ? "step-connector-horizontal"
    : "step-connector-vertical";

  if (currentStepStatus === true) {
    return `${baseClass} active`;
  }
  return baseClass;
};
