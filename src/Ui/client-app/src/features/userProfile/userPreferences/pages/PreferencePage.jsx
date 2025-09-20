
export default function PreferencePage(){
    return(
      <div>
        {/* navbar */}
        <nav className="border-bottom position-sticky py-2 mb-3 d-flex align-items-center gap-3">
            <div>
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                    <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M20 12H4m0 0l6-6m-6 6l6 6" />
                </svg>
            </div>
            <div>
                <strong>Notification</strong>
            </div>
        </nav>
        {/* notification setting */}
        <div className="d-flex flex-column gap-3">
            {/* title */}
            <div className="d-flex gap-3 align-items-center ">
                <div>
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                        <g fill="none" stroke="currentColor" stroke-width="1.5">
                            <path d="M18.75 9.71v-.705C18.75 5.136 15.726 2 12 2S5.25 5.136 5.25 9.005v.705a4.4 4.4 0 0 1-.692 2.375L3.45 13.81c-1.011 1.575-.239 3.716 1.52 4.214a25.8 25.8 0 0 0 14.06 0c1.759-.498 2.531-2.639 1.52-4.213l-1.108-1.725a4.4 4.4 0 0 1-.693-2.375Z" />
                            <path stroke-linecap="round" d="M7.5 19c.655 1.748 2.422 3 4.5 3s3.845-1.252 4.5-3" />
                        </g>
                    </svg>
                </div>
                <div>
                    <div><strong>Activity from others</strong></div>
                    <div style={{'font-size': '13px','color':'gray'}}>Get notified about post and replies from accounts you choose.</div>
                </div>
            </div>
            {/* group checkbox */}
           <div className="d-flex flex-column gap-2">
                {/* <div class="form-check ">
                    <input class="form-check-input" type="checkbox" value="" id="push-notify"/>
                    <label class="form-check-label" for="push-notify">
                        Push notifications
                    </label>
                </div> */}
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="inapp-notify" />
                    <label class="form-check-label" for="inapp-notify">
                        In-app notifications
                    </label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="email-notify"/>
                    <label class="form-check-label" for="email-notify">
                        Email notifications
                    </label>
                </div>
           </div>
           {/* utils */}
           <div className="border rounded p-2 d-flex gap-2 align-items-center">
                <div>
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                        <g fill="none" stroke="currentColor" stroke-width="1.5">
                            <path d="M10.577 8.704C11.21 7.568 11.527 7 12 7s.79.568 1.423 1.704l.164.294c.18.323.27.484.41.59c.14.107.316.147.665.226l.318.072c1.23.278 1.845.417 1.991.888c.147.47-.273.96-1.111 1.941l-.217.254c-.238.278-.357.418-.41.59c-.055.172-.037.358 0 .73l.032.338c.127 1.308.19 1.962-.193 2.253c-.383.29-.958.026-2.11-.504l-.298-.138c-.327-.15-.49-.226-.664-.226c-.173 0-.337.076-.664.226l-.298.138c-1.152.53-1.727.795-2.11.504c-.383-.29-.32-.945-.193-2.253l.032-.338c.037-.372.055-.558 0-.73c-.053-.172-.172-.312-.41-.59l-.217-.254c-.838-.98-1.258-1.47-1.111-1.941c.146-.47.76-.61 1.99-.888l.319-.072c.35-.08.524-.119.665-.225c.14-.107.23-.268.41-.59z" />
                            <path stroke-linecap="round" d="M12 2v2m0 16v2M2 12h2m16 0h2" opacity="0.5" />
                            <path stroke-linecap="round" d="m6 18l.343-.343M17.657 6.343L18 6m0 12l-.343-.343M6.343 6.343L6 6" />
                        </g>
                    </svg>
                </div>
                <div className="d-flex flex-column gap-1" style={{'font-size': '14px'}}>
                    <div className="d-flex gap-1">Enable notifications for an account by visiting their profile and pressing the <strong>bell icon</strong> 
                        <span>
                            <svg fill="none" viewBox="0 0 24 24" width="12" height="12">
                                <path fill="hsl(211, 28%, 20.1%)" fill-rule="evenodd" clip-rule="evenodd" d="M12 2a7.854 7.854 0 0 1 7.784 6.815l1.207 9.053a1 1 0 0 1-.99 1.132h-3.354c-.904 1.748-2.608 3-4.647 3-2.038 0-3.742-1.252-4.646-3H4a1.002 1.002 0 0 1-.991-1.132l1.207-9.053A7.85 7.85 0 0 1 12 2ZM9.78 19c.608.637 1.398 1 2.221 1s1.613-.363 2.222-1H9.779ZM3.193 2.104a1 1 0 0 1 1.53 1.288A9.47 9.47 0 0 0 2.72 7.464a1 1 0 0 1-1.954-.427 11.46 11.46 0 0 1 2.428-4.933Zm16.205-.122a1 1 0 0 1 1.409.122 11.47 11.47 0 0 1 2.429 4.933 1 1 0 0 1-1.954.427 9.47 9.47 0 0 0-2.006-4.072 1 1 0 0 1 .122-1.41Z"/>                                
                            </svg>
                        </span>
                    </div>
                    <div style={{'font-size': '13px', 'color': 'gray'}}>
                        If you want to restrict who can receive notifications for your account's activity, you can change this in
                        <a href="/settings" className="ps-1">Settings</a>
                    </div>
                </div>
           </div>
        </div>
      </div>
    )
}