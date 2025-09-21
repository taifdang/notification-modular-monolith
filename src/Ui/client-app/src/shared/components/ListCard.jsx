export function ListCard({
    className,
    style,
    chidlren
}){
    return(
        <div 
        className={className} 
        style={style}>
            {chidlren}
        </div>
    )
}