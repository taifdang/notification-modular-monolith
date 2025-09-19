import { useState } from "react";
import QrCode from "#assets/images/qrcode.jpg";

export default function TopupPage() {
  const [activeTab, setActiveTab] = useState(1);
  return (
    <div>
      <div className="d-flex row nav-tabs" role="tablist">
        <div
          role="tab"
          aria-selected={activeTab === 1}
          className="text-center col-6 cardProfile"
          onClick={() => setActiveTab(1)}
        >
          <div className={`tab ${activeTab === 1 ? "active" : ""}`}>
            Topup
            <div className="underline"></div>
          </div>
        </div>
        <div
          role="tab"
          aria-selected={activeTab === 2}
          className="text-center col-6 cardProfile"
          onClick={() => setActiveTab(2)}
        >
          <div className={`tab ${activeTab === 2 ? "active" : ""}`}>
            History
            <div className="underline"></div>
          </div>
        </div>
      </div>
      {/* tab_detail*/}
      <div className="border rounded mt-2 p-2 d-flex gap-2 align-items-center">
        <div>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="24"
            height="24"
            viewBox="0 0 24 24"
          >
            <g fill="none" stroke="currentColor" stroke-width="1.5">
              <path d="M10.577 8.704C11.21 7.568 11.527 7 12 7s.79.568 1.423 1.704l.164.294c.18.323.27.484.41.59c.14.107.316.147.665.226l.318.072c1.23.278 1.845.417 1.991.888c.147.47-.273.96-1.111 1.941l-.217.254c-.238.278-.357.418-.41.59c-.055.172-.037.358 0 .73l.032.338c.127 1.308.19 1.962-.193 2.253c-.383.29-.958.026-2.11-.504l-.298-.138c-.327-.15-.49-.226-.664-.226c-.173 0-.337.076-.664.226l-.298.138c-1.152.53-1.727.795-2.11.504c-.383-.29-.32-.945-.193-2.253l.032-.338c.037-.372.055-.558 0-.73c-.053-.172-.172-.312-.41-.59l-.217-.254c-.838-.98-1.258-1.47-1.111-1.941c.146-.47.76-.61 1.99-.888l.319-.072c.35-.08.524-.119.665-.225c.14-.107.23-.268.41-.59z" />
              <path
                stroke-linecap="round"
                d="M12 2v2m0 16v2M2 12h2m16 0h2"
                opacity="0.5"
              />
              <path
                stroke-linecap="round"
                d="m6 18l.343-.343M17.657 6.343L18 6m0 12l-.343-.343M6.343 6.343L6 6"
              />
            </g>
          </svg>
        </div>
        <div
          className="d-flex flex-column gap-1 "
          style={{ "font-size": "14px" }}
        >
          <div className="d-flex gap-1 ">
            This QR code / transaction only supports banking transactions in{" "}
            <strong>Việt Nam</strong>
            <span>
              <svg fill="none" viewBox="0 0 24 24" width="12" height="12">
                <path
                  fill="hsl(211, 28%, 20.1%)"
                  fill-rule="evenodd"
                  clip-rule="evenodd"
                  d="M12 2a7.854 7.854 0 0 1 7.784 6.815l1.207 9.053a1 1 0 0 1-.99 1.132h-3.354c-.904 1.748-2.608 3-4.647 3-2.038 0-3.742-1.252-4.646-3H4a1.002 1.002 0 0 1-.991-1.132l1.207-9.053A7.85 7.85 0 0 1 12 2ZM9.78 19c.608.637 1.398 1 2.221 1s1.613-.363 2.222-1H9.779ZM3.193 2.104a1 1 0 0 1 1.53 1.288A9.47 9.47 0 0 0 2.72 7.464a1 1 0 0 1-1.954-.427 11.46 11.46 0 0 1 2.428-4.933Zm16.205-.122a1 1 0 0 1 1.409.122 11.47 11.47 0 0 1 2.429 4.933 1 1 0 0 1-1.954.427 9.47 9.47 0 0 0-2.006-4.072 1 1 0 0 1 .122-1.41Z"
                />
              </svg>
            </span>
          </div>
          <div style={{ "font-size": "13px", color: "gray" }}>
            If you want to try with international or third-party QR code types,
            please wait for the next update.
          </div>
        </div>
      </div>
      {/* Tabs content */}
      <div className="tab-content pt-3">
        {activeTab === 1 && (
          <div id="panel1" role="tabpanel">
            <div className="d-flex justify-content-center align-items-center mt-2">
              <div
                class="card text-center p-3 shadow-sm border-0"
                style={{ width: "18rem", backgroundColor: "#f8f9fa" }}
              >
                <div
                  class="position-relative rounded p-2 mx-auto"
                  style={{ display: "inline-block" }}
                >
                  <div
                    class="bg-light d-flex align-items-center justify-content-center"
                    style={{ width: "200px", height: "200px" }}
                  >
                    {/* QR Placeholder */}
                    <img
                      src={QrCode}
                      alt="QR Code"
                      style={{ maxWidth: "100%", maxHeight: "100%" }}
                    />
                  </div>
                  <span class="corner top-left"></span>
                  <span class="corner top-right"></span>
                  <span class="corner bottom-left"></span>
                  <span class="corner bottom-right"></span>
                </div>
                <div class="mt-3">
                  <p class="mb-1 ">
                    Beneficiary bank:
                    <span class="fw-semibold"> MBBank</span>
                  </p>
                  <p class="fw-bold mb-1 ">NAPCOIN1200</p>
                  <div className="d-flex text-center justify-content-center align-items-center gap-2">
                    <div>99ZP24334000725953</div>
                    <a href="#">
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="20"
                        height="20"
                        viewBox="0 0 24 24"
                      >
                        <g fill="none" stroke="currentColor" stroke-width="1.5">
                          <path d="M6 11c0-2.828 0-4.243.879-5.121C7.757 5 9.172 5 12 5h3c2.828 0 4.243 0 5.121.879C21 6.757 21 8.172 21 11v5c0 2.828 0 4.243-.879 5.121C19.243 22 17.828 22 15 22h-3c-2.828 0-4.243 0-5.121-.879C6 20.243 6 18.828 6 16z" />
                          <path d="M6 19a3 3 0 0 1-3-3v-6c0-3.771 0-5.657 1.172-6.828S7.229 2 11 2h4a3 3 0 0 1 3 3" />
                        </g>
                      </svg>
                    </a>
                  </div>
                </div>
              </div>
            </div>
            {/* testing____________ */}
            {/* <div>
              <div className="d-flex gap-1 align-items-center mb-1 mt-3">
                <div>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    viewBox="0 0 24 24"
                  >
                    <path
                      fill="currentColor"
                      d="M8.267 1.618a.75.75 0 0 1 1.027-.264l.832.492l9.247 5.307a.75.75 0 1 1-.747 1.301l-.843-.484l-1.505 2.598l-.002-.002l-2.558-1.471a.75.75 0 1 0-.748 1.3l2.556 1.47l-.961 1.66l-.002-.001l-4.203-2.418a.75.75 0 1 0-.748 1.3l4.2 2.417l-.885 1.529l-.002-.002l-2.613-1.503a.75.75 0 0 0-.748 1.3l2.611 1.502l-1.12 1.932a4.86 4.86 0 0 1-6.628 1.77a4.827 4.827 0 0 1-1.776-6.605L9.373 3.143l-.006-.003l-.836-.494a.75.75 0 0 1-.264-1.028M20 17c1.105 0 2-.933 2-2.083c0-.72-.783-1.681-1.37-2.3a.86.86 0 0 0-1.26 0c-.587.619-1.37 1.58-1.37 2.3c0 1.15.895 2.083 2 2.083"
                    />
                  </svg>{" "}
                </div>
                <span style={{ fontSize: "16px", fontWeight: 600 }}>
                  Test enviroment
                </span>
              </div>
              <textarea
                name="textarea-payload_topup"
                id="payload_topup"
                value={
                  '{\n  "amount": 1200,\n  "currency": "VND",\n  "description": "Topup 1200 VND to Napcoin account",\n  "external_id": "NAPCOIN1200",\n  "bank_code": "MBBank",\n  "account_number": "99ZP24334000725953"\n}'
                }
                className="form-control focus-ring focus-ring-light"
                style={{
                  width: "100%",
                  borderRadius: "8px",
                  minHeight: "200px",
                  maxHeight: "300px",
                  fontSize: "14px",
                }}
              ></textarea>
              <div className="d-flex justify-content-end gap-2">
                <button className="btn border btn-sm mt-2 d-flex align-items-center gap-1 card_effect">
                  Copy
                  <div>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="20"
                      height="20"
                      viewBox="0 0 24 24"
                    >
                      <g fill="none" stroke="currentColor" stroke-width="1.5">
                        <path d="M6 11c0-2.828 0-4.243.879-5.121C7.757 5 9.172 5 12 5h3c2.828 0 4.243 0 5.121.879C21 6.757 21 8.172 21 11v5c0 2.828 0 4.243-.879 5.121C19.243 22 17.828 22 15 22h-3c-2.828 0-4.243 0-5.121-.879C6 20.243 6 18.828 6 16z" />
                        <path d="M6 19a3 3 0 0 1-3-3v-6c0-3.771 0-5.657 1.172-6.828S7.229 2 11 2h4a3 3 0 0 1 3 3" />
                      </g>
                    </svg>
                  </div>
                </button>
                <button className="btn btn-primary btn-sm mt-2">
                  Send Request
                </button>
              </div>
            </div> */}
          </div>
        )}
        {activeTab === 2 && (
          <div id="panel2" role="tabpanel">
            {/* <div>
              <div className="d-flex gap-1 align-items-center mb-1">
                <div>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    viewBox="0 0 24 24"
                  >
                    <path
                      fill="currentColor"
                      d="M8.267 1.618a.75.75 0 0 1 1.027-.264l.832.492l9.247 5.307a.75.75 0 1 1-.747 1.301l-.843-.484l-1.505 2.598l-.002-.002l-2.558-1.471a.75.75 0 1 0-.748 1.3l2.556 1.47l-.961 1.66l-.002-.001l-4.203-2.418a.75.75 0 1 0-.748 1.3l4.2 2.417l-.885 1.529l-.002-.002l-2.613-1.503a.75.75 0 0 0-.748 1.3l2.611 1.502l-1.12 1.932a4.86 4.86 0 0 1-6.628 1.77a4.827 4.827 0 0 1-1.776-6.605L9.373 3.143l-.006-.003l-.836-.494a.75.75 0 0 1-.264-1.028M20 17c1.105 0 2-.933 2-2.083c0-.72-.783-1.681-1.37-2.3a.86.86 0 0 0-1.26 0c-.587.619-1.37 1.58-1.37 2.3c0 1.15.895 2.083 2 2.083"
                    />
                  </svg>{" "}
                </div>
                <span style={{ fontSize: "16px", fontWeight: 600 }}>
                  Test enviroment
                </span>
              </div>
              <textarea
                name="textarea-payload_topup"
                id="payload_topup"
                value={
                  '{\n  "amount": 1200,\n  "currency": "VND",\n  "description": "Topup 1200 VND to Napcoin account",\n  "external_id": "NAPCOIN1200",\n  "bank_code": "MBBank",\n  "account_number": "99ZP24334000725953"\n}'
                }
                className="form-control focus-ring focus-ring-light"
                style={{
                  width: "100%",
                  borderRadius: "8px",
                  minHeight: "200px",
                  maxHeight: "300px",
                  fontSize: "14px",
                }}
              ></textarea>
              <div className="d-flex justify-content-end gap-2">
                <button className="btn border btn-sm mt-2 d-flex align-items-center gap-1 card_effect">
                  Copy
                  <div>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="20"
                      height="20"
                      viewBox="0 0 24 24"
                    >
                      <g fill="none" stroke="currentColor" stroke-width="1.5">
                        <path d="M6 11c0-2.828 0-4.243.879-5.121C7.757 5 9.172 5 12 5h3c2.828 0 4.243 0 5.121.879C21 6.757 21 8.172 21 11v5c0 2.828 0 4.243-.879 5.121C19.243 22 17.828 22 15 22h-3c-2.828 0-4.243 0-5.121-.879C6 20.243 6 18.828 6 16z" />
                        <path d="M6 19a3 3 0 0 1-3-3v-6c0-3.771 0-5.657 1.172-6.828S7.229 2 11 2h4a3 3 0 0 1 3 3" />
                      </g>
                    </svg>
                  </div>
                </button>
                <button className="btn btn-primary btn-sm mt-2">
                  Send Request
                </button>
              </div>
            </div> */}
            <div class="table-responsive mt-3 ">
            <table 
            className="table text-nowrap border"
            style={{fontFamily:'"Inter',fontSize:'13.125px'}}>
              <thead className="table-light">
                <tr>
                  <th className="col">#</th>
                  <th className="col-2">Id</th>
                  <th className="col-3">Transfer</th>
                  <th className="col-2">Status</th>
                  <th className="col-3">CreateAt</th>
                  <th className="col">Action</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  {/* <td 
                  className="text-center"
                  colSpan={10}>
                    <div className="d-flex align-items-center gap-2 justify-content-center">                
                     <span style={{fontSize:'13.125px',fontFamily:"Inter",fontWeight:'600',color:'#42576c', lineHeight: '13.125px'}}>No Data Found</span>
                    </div>
                  </td> */}
                </tr>
                <tr>
                  <td>1</td>
                  <td>HD1231231</td>
                  <td>+25.000.000</td>
                  <td>
                    <div className="badge bg-primary">
                      Success
                    </div>
                  </td>
                  <td>2024-01-15 10:30:25</td>
                  <td>
                    <div className="d-flex flex-column flex-grow-1 flex-shrink-1 align-items-strecth align-content-center">
                      <button 
                      className="border-0"
                      style={{backgroundColor:'#fff'}}
                      >
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                          <g fill="none" stroke="currentColor" stroke-width="1.5">
                            <path d="M19 15v-3.062A3.94 3.94 0 0 0 15.063 8H8.936A3.94 3.94 0 0 0 5 11.938V15a7 7 0 1 0 14 0Z" />
                            <path d="M16.5 8.5v-1a4.5 4.5 0 1 0-9 0v1" />
                            <path stroke-linecap="round" d="M19 14h3M5 14H2M14.5 3.5L17 2M9.5 3.5L7 2m13.5 18l-2-.8m2-11.2l-2 .8M3.5 20l2-.8M3.5 8l2 .8M12 21.5V15" />
                          </g>
                        </svg>
                      </button>
                    </div>
                  </td>
                </tr>       
                <tr>
                  <td>1</td>
                  <td>HD1231231</td>
                  <td>+25.000.000</td>
                  <td>
                    <div className="badge bg-danger">
                      Fail
                    </div>
                  </td>
                  <td>2024-01-15 10:30:25</td>
                  <td>
                    <div className="d-flex flex-column flex-grow-1 flex-shrink-1 align-items-strecth align-content-center">
                      <button 
                      className="border-0"
                      style={{backgroundColor:'#fff'}}
                      >
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                          <g fill="none" stroke="currentColor" stroke-width="1.5">
                            <path d="M19 15v-3.062A3.94 3.94 0 0 0 15.063 8H8.936A3.94 3.94 0 0 0 5 11.938V15a7 7 0 1 0 14 0Z" />
                            <path d="M16.5 8.5v-1a4.5 4.5 0 1 0-9 0v1" />
                            <path stroke-linecap="round" d="M19 14h3M5 14H2M14.5 3.5L17 2M9.5 3.5L7 2m13.5 18l-2-.8m2-11.2l-2 .8M3.5 20l2-.8M3.5 8l2 .8M12 21.5V15" />
                          </g>
                        </svg>
                      </button>
                    </div>
                  </td>
                </tr>            
                <tr>
                  <td>1</td>
                  <td>HD1231231</td>
                  <td>+25.000.000</td>
                  <td>
                    <div className="badge bg-warning">
                      In Progress
                    </div>
                  </td>
                  <td>2024-01-15 10:30:25</td>
                  <td>
                    <div className="d-flex flex-column flex-grow-1 flex-shrink-1 align-items-strecth align-content-center">
                      <button 
                      className="border-0"
                      style={{backgroundColor:'#fff'}}
                      >
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24">
                          <g fill="none" stroke="currentColor" stroke-width="1.5">
                            <path d="M19 15v-3.062A3.94 3.94 0 0 0 15.063 8H8.936A3.94 3.94 0 0 0 5 11.938V15a7 7 0 1 0 14 0Z" />
                            <path d="M16.5 8.5v-1a4.5 4.5 0 1 0-9 0v1" />
                            <path stroke-linecap="round" d="M19 14h3M5 14H2M14.5 3.5L17 2M9.5 3.5L7 2m13.5 18l-2-.8m2-11.2l-2 .8M3.5 20l2-.8M3.5 8l2 .8M12 21.5V15" />
                          </g>
                        </svg>
                      </button>
                    </div>
                  </td>
                </tr>     
              </tbody>
            </table>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
