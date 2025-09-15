import { View } from '#shared/components/View'
import { Text } from '#shared/components/Text'
import { css } from '@emotion/css'

export function FormContainer({
    titleText = "",
    children,
    style
}){ 

    const baseStyle = css`background-color: hotpink;`;
    const baseStyle1 = css`display:flex;flex:1 1 0%;`
    return(
        <View
        className={`${baseStyle} ${baseStyle1}`}
        style={style}>
            <Text>
                {titleText}
            </Text>
            {children}
        </View>
    )
}