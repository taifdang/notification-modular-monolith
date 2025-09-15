import { css } from '@emotion/css'
export function View({children, className = "", style = {}, ...rest}){
    return(
        <div
        {...rest}
        className={`${className}`}
        style={style}>
            {children}
        </div>
    )
};