import * as tokens from './tokens'
import { css } from '@emotion/css'

export const flexbox = css``

export const atoms = {
     /*
    Position
    */
    fixed:{
        position:'fixed'
    },
    absolute:{
        position:'absolute'
    },
    static:{
        position:'static'
    },
    sticky:{
        position:'sticky'
    },
    /*
    Width & Height
    */
   w_full:{
        width:'100%',
   },
   h_full:{
        height:'100%',
   },
   h_full_vh:{
        height:'100vh',
   },
   max_w_full:{
        maxWidth:'100%',
   },
   max_h_full:{
        maxHeight:'100%'
   },
   /*
   Flex
    */
   gap_0:{
        gap:0
   },
   gap_2xs:{
        gap:tokens.space._2xs
   },
   gap_xs:{
        gap:tokens.space.xs
   }

} as const