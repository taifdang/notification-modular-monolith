import { css } from "@emotion/css";
export default function LoginPage() {
  const flex__wrapper = css`
    display: flex;
    flex-direction: column;
    align-items: stretch;
    align-content: start;
  `;
  const flexbox = css`
    flex: 1 1 0%;
  `;
  return (
    <div>
      <div className="d-flex flex-row">
        <div className='d-none d-md-block col-md-4'>
          <div 
          className='d-flex flex-column flex-grow-1 flex-shrink-1 vh-100'
          style={{ backgroundColor: "rgb(241, 243, 245)" }}>
            <div 
            className='d-flex flex-column flex-grow-1 flex-shrink-1 justify-content-center align-items-end'
            style={{paddingBottom:'80px',paddingLeft:'40px',paddingRight:'40px'}}>
              <div className='d-flex flex-column text-end'>
                <div style={{fontSize: "clamp(2em, 3vmax, 3em)",color: "var(--color-blue)",fontFamily: '"Inter"',fontWeight: 800,}}>Sign in</div>
                <div style={{fontWeight: 600,color:'#42576c' }}>Enter your username and password</div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-12 col-md-8">
          <div
            className={`${flex__wrapper} ${flexbox} h-100`}
            style={{ backgroundColor: "rgb(255, 255, 255)" }}>
            <div className={`${flex__wrapper} ${flexbox}`}>
              <div
                className={`${flex__wrapper} ${flexbox}`}
                style={{ paddingLeft: "40px", paddingRight: "40px" }}>
                <div
                  className={`${flex__wrapper} my-auto`}
                  style={{ maxWidth: "600px" }} >
                  <div className={`${flex__wrapper} ${flexbox}`}>
                    <div className={`${flex__wrapper} ${flexbox}`}>
                      <div className="d-flex flex-column">                
                        <div className="d-flex flex-column pt-4 pt-md-0">
                          <div
                            style={{marginBottom:"8px",fontSize:'13.125px',fontFamily:"Inter",fontWeight:'600',color:'#42576c', lineHeight: '13.125px'}}>
                            Account
                          </div>
                          {/* form */}
                          <div 
                          className="d-flex flex-column"
                          style={{gap:'12px'}}>
                            {/* username */}
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
                                placeholder="Username or email address"
                                className="border-0 w-100 textField"
                                style={{                             
                                  fontFamily: '"Inter"',
                                  fontSize:'15px',
                                  fontWeight: 500,                                  
                                }}/>
                            </div>
                            {/* password */}
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
                                type="text"
                                name="password_input"
                                id="password-input"
                                placeholder="Password"
                                className="border-0 w-100 textField"
                                style={{                             
                                  fontFamily: '"Inter"',
                                  fontSize:'15px',
                                  fontWeight: 500,                                  
                                }}/>
                              <button 
                              className="button__wrapper border-0"
                              style={{fontSize:'13.125px',fontFamily:"Inter",fontWeight:'400',color:'#42576c'}}
                              >Forgot?</button>
                            </div>                         
                          </div>
                          {/* button */}
                          </div>
                          <div
                            className="d-flex"
                            style={{ paddingTop: "28px", paddingBottom: "16px" }}>
                            <button
                              className="btn btn-light"
                              style={{ padding: "8px 25px"}}>
                              <span style={{fontFamily:'"Inter',fontSize: '15px',fontWeight:500,color:'#42576c'}}>Back</span>
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
    </div>
  );
}