export function Card({
    className,
    style,
    children
}){
    return(
        <div 
        className={className} 
        style={style}>
            {children}
        </div>
    )
}

export function Link({
    href,
    children,
    ...rest
}){
    return(
        <a 
        href={href}
        className="d-flex gap-2 align-items-center cardProfile text-link__title"
        {...rest}>
            {children}
        </a>
    )
}
export function Avatar({
    src,
    className = null,
    style = null
}){
    return(
       <img 
       src={src}
       className={`rounded-circle border ${className}`}
       alt="avatar"
       style={{ "maxWidth": "64px", height: "auto", ... style }}
       />
    )
}

export function Name({
    text, 
    className = null, 
    style = null}){
    return(
        <div
        className={className}
        style={style}>
            {text}
        </div>
    )
}

export function Labels(){
    return(
        <>
        </>
    )
}