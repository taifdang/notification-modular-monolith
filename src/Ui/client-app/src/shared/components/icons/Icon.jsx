export function createSinglePathSVG({ children }) {
  return function Icon({ size = 24, className }) {
    return (
      <div className={className}>
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width={size}
          height={size}
          viewBox="0 0 24 24"
        >
          {children}
        </svg>
      </div>
    );
  };
}

export function createMultiplePathSVG({ children }) {
  return function Icon({
    size = 24,
    className,
    stroke = "currentColor",
    fill = "none",
    strokeWidth = 1.5,
    ...rest
  }) {
    return (
      <div className={className}>
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width={size}
          height={size}
          viewBox="0 0 24 24"
          {...rest}
        >
          <g fill="none" stroke="currentColor" strokeWidth="1.5">
            {children}
          </g>
        </svg>
      </div>
    );
  };
}
export function createMultipleSimplePathSVG({ children }) {
  return function Icon({ size = 24, className, ...rest }) {
    return (
      <div className={className}>
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width={size}
          height={size}
          viewBox="0 0 24 24"
          {...rest}
        >
          {children}
        </svg>
      </div>
    );
  };
}
