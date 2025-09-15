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
        <div className="d-none d-md-block col-md-4">
          <div
            className="d-flex flex-column flex-grow-1 flex-shrink-1 vh-100"
            style={{ backgroundColor: "rgb(241, 243, 245)" }}>
            <div
              className="d-flex flex-column flex-grow-1 flex-shrink-1 justify-content-center align-items-end"
              style={{
                paddingLeft: "40px",
                paddingRight: "40px",
                paddingBottom: "80px",
              }}>
              <div className="d-flex flex-column align-items-stretch text-end">
                <div
                  style={{
                    fontSize: "clamp(2em, 3vmax, 3em)",
                    color: "var(--color-blue)",
                    fontFamily: '"Inter"',
                    fontWeight: 800,
                  }}>
                  Sign in
                </div>
                <div style={{ color: "var(--color-gray)", fontWeight: 600 }}>
                  Enter your username and password
                </div>
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
                            style={{
                              paddingBottom: "12px",
                              color: "var(--color-gray)",
                              fontWeight: 600,
                            }}>
                            Account
                          </div>
                           <div 
                              className="d-flex flex-column"
                              style={{gap:'12px'}}>
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
                                    fontWeight: 500,
                                  }}
                                />
                              </div>
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
                                  id="pass-input"
                                  placeholder="Password"
                                  className="border-0 w-100 textField"
                                  style={{                             
                                    fontFamily: '"Inter"',
                                    fontWeight: 500,
                                  }}
                                />
                                <button className="button__forgot">
                                  Forgot?
                                </button>
                              </div>
                           </div>
                        </div>
                        <div
                          className="d-flex"
                          style={{ paddingTop: "28px", paddingBottom: "16px" }}>
                          <button
                            className="btn btn-light"
                            style={{ padding: "8px 25px" }}>
                            Back
                          </button>
                          <button
                            className="btn btn-primary ms-auto"
                            style={{ padding: "8px 25px" }}>
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
  );
}
<div className="container-fluid vh-100 p-0">
  <div className="row h-100 w-100 m-0 p-0">
    <div className="col-4 box-left__wrapper">
      <div className="d-flex flex-column">
        <div
          style={{ fontSize: "3em" }}
          className="text-color__header text-content__header"
        >
          Sign in
        </div>
        <div className="text-content__title">
          Enter your username and password
        </div>
      </div>
    </div>
    <div className="col-md-8 box-right__wrapper">
      <div className="my-auto" style={{ width: "600px" }}>
        <div className="">
          <div className="d-flex flex-column gap-2">
            <div>
              <span
                style={{ fontSize: "10x", fontWeight: "500", color: "gray" }}
              >
                Account
              </span>
            </div>
            <div className="group-input__wrapper">
              <div className="group-input">
                <div className="group-input__icon">@</div>
                <input
                  type="text"
                  name="username_input"
                  id="user-input"
                  placeholder="Username or email address"
                  className="border-0 w-100 input__info"
                />
              </div>
              <div className="group-input">
                <div className="group-input__icon">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="20"
                    height="20"
                    viewBox="0 0 24 24"
                  >
                    <g fill="none" stroke="currentColor" stroke-width="1.5">
                      <path d="M2 16c0-2.828 0-4.243.879-5.121C3.757 10 5.172 10 8 10h8c2.828 0 4.243 0 5.121.879C22 11.757 22 13.172 22 16s0 4.243-.879 5.121C20.243 22 18.828 22 16 22H8c-2.828 0-4.243 0-5.121-.879C2 20.243 2 18.828 2 16Z" />
                      <path
                        stroke-linecap="round"
                        d="M6 10V8a6 6 0 1 1 12 0v2"
                      />
                    </g>
                  </svg>
                </div>
                <input
                  type="text"
                  name="password_input"
                  id="pass-input"
                  placeholder="Password"
                  className="border-0 w-100 input__info"
                />
                <button className="button__forgot">Forgot?</button>
              </div>
            </div>
          </div>
          <div className="d-flex mt-2">
            <button className="btn btn-light button__wrapper">Back</button>
            <button className="btn btn-primary ms-auto button__wrapper">
              Next
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>;
