
import { css } from '@emotion/css'
export default function SignUpPage(){
  const flex__wrapper = css`
    display: flex;
    flex-direction: column;
    align-items: stretch;
    align-content: start;
  `;
  const flexbox = css`
    flex: 1 1 0%;
  `;
    return(
      <div>
          <div>
            <div  className='d-flex flex-row'>
              <div className='d-none d-md-block col-md-4'>
                <div 
                className='d-flex flex-column flex-grow-1 flex-shrink-1 vh-100'
                style={{ backgroundColor: "rgb(241, 243, 245)" }}>
                  <div 
                  className='d-flex flex-column flex-grow-1 flex-shrink-1 justify-content-center align-items-end'
                  style={{paddingBottom:'80px',paddingLeft:'40px',paddingRight:'40px'}}>
                    <div className='d-flex flex-column text-end'>
                      <div style={{fontSize: "clamp(2em, 3vmax, 3em)",color: "var(--color-blue)",fontFamily: '"Inter"',fontWeight: 800,}}>Create Account</div>
                      <div style={{fontWeight: 600,color:'#42576c' }}>Please enter your information in form!</div>
                    </div>
                  </div>
                </div>
              </div>
              <div className='col-12 col-md-8'>
                <div 
                className={`${flex__wrapper} ${flexbox} h-100`}
                style={{ backgroundColor: "rgb(255, 255, 255)" }}>
                  <div 
                  className={`${flex__wrapper} ${flexbox}`}
                  style={{padding:'0 clamp(0px, 4vw, 40px)'}}>
                    <div
                    className={`${flex__wrapper} my-auto`}
                    style={{maxWidth:'600px'}}>              
                        <div 
                        className={`${flex__wrapper} ${flexbox}`}
                        style={{paddingLeft:'20px',paddingRight:'20px',paddingTop:'24px'}}>
                          <div className="d-flex flex-column pb-1">
                            <div style={{paddingBottom: "8px",fontSize: "24.375px",fontWeight: 800}}>Your Account</div>
                            <div className="d-flex flex-column pt-3">                            
                              <div
                              className="d-flex flex-column"
                              style={{gap:'12px'}}>
                                <div className='d-flex flex-column'>
                                  <div style={{marginBottom:"8px",fontSize:'13.125px',fontFamily:"Inter",fontWeight:'600',color:'#42576c', lineHeight: '13.125px'}}>Your Name</div>
                                  <div className="textField__container">
                                  <div className="textField__icon">
                                      <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                                        <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M16 11.996V7.998m0 3.998c0-5.157-8-5.157-8 0c0 5.167 8 5.11 8 0m0 0c0 5 5 5 5 0C21 7.027 16.97 3 12 3s-9 4.027-9 8.996c0 4.968 4.03 8.995 9 8.995c1.675.084 3.938-.421 5.776-1.831" />
                                      </svg>
                                  </div>
                                  <input
                                    type="text"
                                    name="username_input"
                                    id="user-input"
                                    placeholder="Enter your name"
                                    className="border-0 w-100 textField"
                                    style={{
                                      fontFamily: '"Inter"',
                                      fontWeight: 500,
                                      fontSize:'15px',
                                    }}
                                  />
                                </div>
                                </div>
                                
                                 <div className='d-flex flex-column'>
                                  <div style={{marginBottom:"8px",fontSize:'13.125px',fontFamily:"Inter",fontWeight:'600',color:'#42576c', lineHeight: '13.125px'}}>Email</div>
                                  <div className="textField__container">
                                  <div className="textField__icon">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                                      <g fill="none" stroke="currentColor" stroke-width="1.5">
                                        <path d="M2 12c0-3.771 0-5.657 1.172-6.828S6.229 4 10 4h4c3.771 0 5.657 0 6.828 1.172S22 8.229 22 12s0 5.657-1.172 6.828S17.771 20 14 20h-4c-3.771 0-5.657 0-6.828-1.172S2 15.771 2 12Z" />
                                        <path stroke-linecap="round" d="m6 8l2.159 1.8c1.837 1.53 2.755 2.295 3.841 2.295s2.005-.765 3.841-2.296L18 8" />
                                      </g>
                                    </svg>
                                  </div>
                                  <input
                                    type="text"
                                    name="email_input"
                                    id="email-input"
                                    placeholder="Enter your email address"
                                    className="border-0 w-100 textField"
                                    style={{
                                      fontFamily: '"Inter"',
                                      fontSize:'15px',
                                      fontWeight: 500,
                                    }}
                                  />
                                </div>
                                </div>
                                {/* password */}
                                <div className='d-flex flex-column'>
                                  <div style={{marginBottom:"8px",fontSize:'13.125px',fontFamily:"Inter",fontWeight:'600',color:'#42576c', lineHeight: '13.125px'}}>Password</div>
                                  <div className="textField__container">
                                    <div className="textField__icon">
                                      <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        width="20"
                                        height="20"
                                        viewBox="0 0 24 24">
                                        <g
                                          fill="none"
                                          stroke="currentColor"
                                          stroke-width="1.5">
                                          <path d="M2 16c0-2.828 0-4.243.879-5.121C3.757 10 5.172 10 8 10h8c2.828 0 4.243 0 5.121.879C22 11.757 22 13.172 22 16s0 4.243-.879 5.121C20.243 22 18.828 22 16 22H8c-2.828 0-4.243 0-5.121-.879C2 20.243 2 18.828 2 16Z" />
                                          <path
                                            stroke-linecap="round"
                                            d="M6 10V8a6 6 0 1 1 12 0v2"/>
                                        </g>
                                      </svg>
                                    </div>
                                    <input
                                      type="password"
                                      name="password_input"
                                      id="pass-input"
                                      placeholder="Password"
                                      className="border-0 w-100 textField"
                                      style={{
                                        fontFamily: '"Inter"',
                                        fontSize:'15px',
                                        fontWeight: 500,                                                             
                                      }}
                                    />                                    
                                  </div>
                                </div>
                                {/* notice */}
                                <div style={{fontSize: '13.125px', color: '#42576c', fontWeight:500}}>
                                  By creating an account you agree to the <a href="#">Terms of Service</a> and <a href="#">Privacy Policy</a>.
                                </div>
                                {/* security */}
                                <div 
                                style={{backgroundColor:'rgb(241, 243, 245)'}}
                                className="border rounded p-2 d-flex gap-2 align-items-center">
                                    <a href="#">
                                      <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                                        <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10a6 6 0 0 0-6-6H3v2a6 6 0 0 0 6 6h3m0 2a6 6 0 0 1 6-6h3v1a6 6 0 0 1-6 6h-3m0 5V10" />
                                      </svg>
                                    </a>   
                                    <div className="d-flex flex-column gap-1 ">                                  
                                        <div style={{'font-size': '13.125px'}}>
                                          You also agree to <a href="#">BlueLock's Guidelines</a>. An <a href="#">updated version of our Application Guidelines</a> will take effect on immediately                                    
                                        </div>
                                    </div>
                                </div>  
                              </div>
                            </div>    
                            {/* button */}
                            <div
                              className="d-flex"
                              style={{ paddingTop: "28px", paddingBottom: "16px" }}>
                              <button
                                className="btn btn-light"
                                style={{ padding: "8px 25px"}}>
                                <span style={{fontFamily:'"Inter',fontSize: '15px',fontWeight:500,color:'#42576c' }}>Back</span>
                              </button>
                              <button
                                className="btn btn-primary ms-auto"
                                style={{ padding: "8px 25px",fontFamily:'"Inter',fontSize: "15px",fontWeight:500 }}>
                               <span style={{fontFamily:'"Inter',fontSize: '15px',fontWeight:500,color:'#fffff'}}>Next</span>
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
    )
}  