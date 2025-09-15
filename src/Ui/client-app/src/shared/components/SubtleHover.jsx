import { css } from "@emotion/css"
import { View } from "./View"
export function SubtleHover(){
    const wrapper = css`
        position: absolute;
        opacity: 0.5;`
    return(
        <View
        css={wrapper}/>
    )
}