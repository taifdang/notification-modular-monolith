
import { css } from '@emotion/css'
export default function RegisterPage(){
    const wrapper = css`
      display: flex;    
      flex-direction: column;
      align-content: flex-start;
      align-items: stretch;
      margin: 0;
      padding: 0;
      z-index: 0;
      position: relative;
      background-color: rgb(241, 243, 245);
      flex: 1 1 0%;
      text-decoreation: none;
      `;
    return(
        <div>
             <div className="container-fluid vh-100 p-0">
                <div className="row h-100 w-100 m-0 p-0">   
                     {/* <div className="col-md-4 box-left__wrapper">
                        <div className="d-flex flex-column">
                             <div 
                              style={{fontSize:'3em'}}
                              className="text-color__header text-content__header">
                                  Create Account
                              </div>
                              <div className="text-content__title">Please enter your information in form!</div>
                        </div>
                    </div>                */}
                    <div 
                    className="d-none d-md-block col-md-4 p-0"
                    >
                      {/* left__wrapper */}
                       <div 
                       className="left__wrapper"
                       style={{height:'100vh'}}
                      //  className="left__wrapper"
                       >
                          <div                          
                          className="left__subwrapper "
                          >
                             <div 
                              // style={{fontSize:'3em'}}
                              className="text-color__header text-content__header ">
                                  Create Account
                              </div>
                              <div className="text-content__title">Please enter your information in form!</div>
                          </div>
                       </div>
                    </div>
                    <div className="col-md-8 p-0">
                        <div className="test__wrapper">
                           <div className="test__subwrapper">
                              <div className="test-box__space">
                                 <div className="test-box__margin">
                                    <div className="test-createAccount">
                                       <div className="test__form">
                                         {/* title */}
                                          <div>
                                            <span style={{fontSize:'24px',fontWeight:'600',paddingBottom:'8px'}}>Your Account</span>
                                          </div>
                                          {/* form */}
                                          <div style={{paddingTop:'16px'}}>
                                            <div className="group-input__wrapper ">
                                              {/* email */}
                                              <div className="d-flex flex-column">
                                                <div style={{marginBottom:'8px'}}>Email</div>
                                                <div className="group-input">
                                                  <div className="group-input__icon">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                                                      <g fill="none" stroke="currentColor" stroke-width="1.5">
                                                        <path d="M2 12c0-3.771 0-5.657 1.172-6.828S6.229 4 10 4h4c3.771 0 5.657 0 6.828 1.172S22 8.229 22 12s0 5.657-1.172 6.828S17.771 20 14 20h-4c-3.771 0-5.657 0-6.828-1.172S2 15.771 2 12Z" />
                                                        <path stroke-linecap="round" d="m6 8l2.159 1.8c1.837 1.53 2.755 2.295 3.841 2.295s2.005-.765 3.841-2.296L18 8" />
                                                      </g>
                                                    </svg>
                                                  </div>
                                                  <input 
                                                  type="email" 
                                                  name="email_input" 
                                                  id="email-input" 
                                                  placeholder="Enter your email address"
                                                  className="border-0 w-100 input__info"/>
                                                </div>
                                              </div>
                                              {/* username */}
                                              <div className="d-flex flex-column">
                                                <div style={{marginBottom:'8px'}}>Username</div>
                                                <div className="group-input">
                                                  <div                                                
                                                  className="group-input__icon">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                                                      <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M16 11.996V7.998m0 3.998c0-5.157-8-5.157-8 0c0 5.167 8 5.11 8 0m0 0c0 5 5 5 5 0C21 7.027 16.97 3 12 3s-9 4.027-9 8.996c0 4.968 4.03 8.995 9 8.995c1.675.084 3.938-.421 5.776-1.831" />
                                                    </svg>
                                                  </div>
                                                  <input 
                                                  type="text" 
                                                  name="username_input" 
                                                  id="username-input" 
                                                  placeholder="Enter your username"
                                                  className="border-0 w-100 input__info"/>
                                                </div>
                                              </div>
                                              {/* password */}
                                              <div className="d-flex flex-column">
                                                <div style={{marginBottom:'8px'}}>Password</div>
                                                <div className="group-input">
                                                  <div className="group-input__icon">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-width="1.5"><path d="M2 16c0-2.828 0-4.243.879-5.121C3.757 10 5.172 10 8 10h8c2.828 0 4.243 0 5.121.879C22 11.757 22 13.172 22 16s0 4.243-.879 5.121C20.243 22 18.828 22 16 22H8c-2.828 0-4.243 0-5.121-.879C2 20.243 2 18.828 2 16Z"/><path stroke-linecap="round" d="M6 10V8a6 6 0 1 1 12 0v2"/></g></svg>
                                                  </div>
                                                  <input 
                                                  type="password" 
                                                  name="password_input" 
                                                  id="pass-input" 
                                                  placeholder="Password"
                                                  className="border-0 w-100 input__info"/>
                                                  {/* <button className="button__forgot">
                                                    Forgot?
                                                  </button>   */}
                                                </div>   
                                              </div>    
                                               {/* notice-1 */}
                                              <div style={{'font-size': '14px', 'color': 'gray'}}>
                                                By creating an account you agree to the <a href="#">Terms of Service</a> and <a href="#">Privacy Policy</a>.
                                              </div>
                                              {/* notice-2 */}
                                              <div 
                                              style={{backgroundColor:'rgb(241, 243, 245)'}}
                                              className="border rounded p-2 d-flex gap-2 align-items-center">
                                                  <a href="#">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                                                      <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10a6 6 0 0 0-6-6H3v2a6 6 0 0 0 6 6h3m0 2a6 6 0 0 1 6-6h3v1a6 6 0 0 1-6 6h-3m0 5V10" />
                                                    </svg>
                                                  </a>   
                                                  <div className="d-flex flex-column gap-1 ">                                  
                                                      <div style={{'font-size': '14px'}}>
                                                        You also agree to <a href="#">BlueLock's Guidelines</a>. An <a href="#">updated version of our Application Guidelines</a> will take effect on immediately                                    
                                                      </div>
                                                  </div>
                                              </div>                                                        
                                            </div>
                                          </div>
                       
                                          {/* button */}
                                            <div 
                                            style={{paddingTop:'28px',paddingBottom:'16px'}}
                                            className="d-flex">
                                              <button className="btn btn-light button__wrapper">
                                                Back
                                              </button>
                                              <button className="btn btn-primary ms-auto button__wrapper">
                                                Next
                                              </button>
                                            </div>                                    
                                       </div>
                                    </div>
                                 </div>
                            </div>
                           </div>
                        </div>
                    </div>
                </div>
              </div>
          </div>
    )
}  