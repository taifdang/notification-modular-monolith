
export function Text({
    children,
    style,
    title,
    ...rest
}){
    return(
        <span 
        {...rest}
        style={style}>
            {children}
        </span>
    )
}