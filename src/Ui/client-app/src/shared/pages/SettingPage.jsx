import AvatarDefault from '../../assets/images/anonymous2.jpg';
import { useNavigate } from "react-router-dom";
export default function SettingPage(){

    const navigate = useNavigate();
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
                    <strong>Settings</strong>
                </div>
            </nav>
            {/* content */}
            <div>
                {/* thumbnail */}
                 <div className=" d-flex flex-column align-items-center gap-1  py-3">
                    <div>
                        <img 
                        src={AvatarDefault} 
                        class="rounded-circle border" 
                        alt="avatar"
                        style={{'max-width': '86px', height: 'auto'}}
                        />  
                    </div>
                    <div style={{'fontSize':'30px','fontWeight':800}}>Anonymous</div>
                    <div>@user_test</div>
                </div>
                <hr />
                {/* account */}
                <a href="#">
                <div className='d-flex gap-2 align-items-center py-3 px-2 card_effect'>
                    <div>
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                            <g fill="none" stroke="currentColor" stroke-width="1.5">
                                <circle cx="12" cy="6" r="4" />
                                <path d="M20 17.5c0 2.485 0 4.5-8 4.5s-8-2.015-8-4.5S7.582 13 12 13s8 2.015 8 4.5Z" />
                            </g>
                        </svg>
                    </div>
                    <div className='me-auto'>Account*</div>
                    <div>
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                            <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="m9 5l6 7l-6 7" />
                        </svg>
                    </div>
                </div>
                </a>
                {/* privacy and security */}
                <a href="#">
                    <div className='d-flex gap-2 align-items-center py-3 px-2 card_effect'>
                    <div>
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                            <g fill="none" stroke="currentColor" stroke-width="1.5">
                                <path d="M2 16c0-2.828 0-4.243.879-5.121C3.757 10 5.172 10 8 10h8c2.828 0 4.243 0 5.121.879C22 11.757 22 13.172 22 16s0 4.243-.879 5.121C20.243 22 18.828 22 16 22H8c-2.828 0-4.243 0-5.121-.879C2 20.243 2 18.828 2 16Z" />
                                <path stroke-linecap="round" d="M12 14v4m-6-8V8a6 6 0 1 1 12 0v2" />
                            </g>
                        </svg>
                    </div>
                    <div className='me-auto'>Privacy and security*</div>
                    <div>
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                            <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="m9 5l6 7l-6 7" />
                        </svg>
                    </div>
                </div>
                </a>
                {/* notification */}
                <a 
                href="/settings/notification"
                onClick={(e) => {
                    e.preventDefault();
                    navigate("/settings/notification");
                }}
                >
                    <div className='d-flex gap-2 align-items-center py-3 px-2 card_effect'>
                        <div>
                            <div>
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                <g fill="none" stroke="currentColor" stroke-width="1.5">
                                    <path d="M18.75 9.71v-.705C18.75 5.136 15.726 2 12 2S5.25 5.136 5.25 9.005v.705a4.4 4.4 0 0 1-.692 2.375L3.45 13.81c-1.011 1.575-.239 3.716 1.52 4.214a25.8 25.8 0 0 0 14.06 0c1.759-.498 2.531-2.639 1.52-4.213l-1.108-1.725a4.4 4.4 0 0 1-.693-2.375Z" />
                                    <path stroke-linecap="round" d="M7.5 19c.655 1.748 2.422 3 4.5 3s3.845-1.252 4.5-3" />
                                </g>
                            </svg>
                        </div>
                        </div>
                        <div className='me-auto'>Notification</div>
                        <div>
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="m9 5l6 7l-6 7" />
                            </svg>
                        </div>
                    </div>
                </a>
                  {/* notification */}
                 <a href="#">
                    <div className='d-flex gap-2 align-items-center py-3 px-2 card_effect'>
                    <div>
                        <div>
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                <g fill="none" stroke="currentColor" stroke-width="1.5">
                                    <circle cx="12" cy="12" r="10" />
                                    <path d="M6 4.71c.78.711 2.388 2.653 2.575 4.737C8.75 11.396 10.035 12.98 12 13c.755.008 1.518-.537 1.516-1.292c0-.233-.039-.472-.099-.692A1.4 1.4 0 0 1 13.5 10c.61-1.257 1.81-1.595 2.76-2.278c.421-.303.806-.623.975-.88c.469-.71.937-2.131.703-2.842M22 13c-.33.931-.562 3.375-4.282 3.414c0 0-3.293 0-4.281 1.862c-.791 1.49-.33 3.103 0 3.724" />
                                </g>
                            </svg>
                        </div>
                    </div>
                    <div className='me-auto'>Language*</div>
                    <div>
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                            <path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="m9 5l6 7l-6 7" />
                        </svg>
                    </div>
                </div>
                {/* sign-out */}
                 </a>
                <hr />
                <div
                style={{'color':'red'}}
                className='d-flex gap-2 align-items-center py-3 px-2 card_effect'>
                    Sign out
                </div>
            </div>
        </div>
    )
}