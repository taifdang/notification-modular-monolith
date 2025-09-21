export function createSinglePathSVG({ path }) {
  return function Icon({
    size = 24,
    className,
    stroke = "currentColor",
    fill = "none",
    strokeWidth = 1.5,
    ...rest
  }) {
    return (
      <svg
        xmlns="http://www.w3.org/2000/svg"
        {...rest}
        width={size}
        height={size}
        viewBox="0 0 24 24"
        className={className}
      >
        {/* fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" */}
        <path d={path} stroke={stroke} fill={fill} strokeWidth={strokeWidth} />
      </svg>
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
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width={size}
        height={size}
        viewBox="0 0 24 24"
        className={className}
        {...rest}
      >
        <g stroke={stroke} fill={fill} strokeWidth={strokeWidth}>
          {children}
        </g>
      </svg>
    );
  };
}
export function createMultipleSimplePathSVG({ children }) {
  return function Icon({ size = 24, className, ...rest }) {
    return (
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width={size}
        height={size}
        viewBox="0 0 24 24"
        {...rest}
      >
        {children}
      </svg>
    );
  };
}
