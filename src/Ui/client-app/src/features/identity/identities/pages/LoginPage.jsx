export default function LoginPage(){
    return(
        <div>
             <div className="container-fluid vh-100 p-0">
                <div className="row h-100 w-100 m-0 p-0">   
                     <div className="col-4 box-left__wrapper">
                        <div className="d-flex flex-column">
                             <div 
                              style={{fontSize:'3em'}}
                              className="text-color__header text-content__header">
                                  Sign in
                              </div>
                              <div className="text-content__title">Enter your username and password</div>
                        </div>
                    </div>               
                    <div className="col-md-8 box-right__wrapper">
                      <div className="my-auto" style={{width:'600px'}}>
                          <div className="" >                           
                            <div 
                            
                            className="d-flex flex-column gap-2">
                              <div>
                                <span style={{fontSize:'10x',fontWeight:'500',color:'gray'}}>Account</span>
                              </div>
                              <div className="group-input__wrapper">
                                <div className="group-input">
                                    <div className="group-input__icon">@</div>
                                    <input 
                                    type="text" 
                                    name="username_input" 
                                    id="user-input" 
                                    placeholder="Username or email address"
                                    className="border-0 w-100 input__info"/>
                                </div>
                                <div className="group-input">
                                    <div className="group-input__icon">
                                      <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-width="1.5"><path d="M2 16c0-2.828 0-4.243.879-5.121C3.757 10 5.172 10 8 10h8c2.828 0 4.243 0 5.121.879C22 11.757 22 13.172 22 16s0 4.243-.879 5.121C20.243 22 18.828 22 16 22H8c-2.828 0-4.243 0-5.121-.879C2 20.243 2 18.828 2 16Z"/><path stroke-linecap="round" d="M6 10V8a6 6 0 1 1 12 0v2"/></g></svg>
                                    </div>
                                    <input 
                                    type="text" 
                                    name="password_input" 
                                    id="pass-input" 
                                    placeholder="Password"
                                    className="border-0 w-100 input__info"/>
                                    <button className="button__forgot">
                                      Forgot?
                                    </button>  
                                </div>                                    
                            </div>
                            </div>                            
                            <div className="d-flex mt-2">
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
    )
}