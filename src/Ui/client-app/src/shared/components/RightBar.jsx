
import { useState } from "react";
export default function RightBar(){
    const [stepStatus] = useState([true, false, false]);
    return(
        <aside className="col-12 col-md-2 pt-4 px-3 d-none d-md-block">
            {/* card about */}
            {/* <div className="card p-3 mb-3 border">
                <h5 className="mb-3">About</h5>
                <p>This is a demo notification system built with a modular monolith architecture.</p>
                <p>It showcases how to structure a project for better maintainability and scalability.</p>
                <a href="/about" className="btn btn-primary btn-sm">Learn More</a>
            </div> */}

            {/* stepper */}
            
            <ol class="stepper">
                <li class="stepper__item">
                    <div class="stepper__circle">
                         <div class="spinner-border" role="status"></div>
                    </div>
                    <div class="stepper__content">
                    <h3 class="stepper__title">STEP 1</h3>
                    <p class="stepper__desc">Topup Confirm</p>
                    </div>
                </li>
                <li class="stepper__item">
                    <div class="stepper__circle">
                       
                    </div>
                    <div class="stepper__content">
                    <h3 class="stepper__title">STEP 2</h3>
                    <p class="stepper__desc">Update Balance</p>
                    </div>
                </li>
                <li class="stepper__item">
                    <div class="stepper__circle">
                        
                    </div>
                    <div class="stepper__content">
                    <h3 class="stepper__title">STEP 3</h3>
                    <p class="stepper__desc">Push Notification</p>
                    </div>
                </li>
            </ol>
             
             {/* <div style={{fontWeight:"bold"}}>Stepper</div>
             <hr />
             <div className="stepper-container position-relative">
                <div 
                className="progress-bar" 
                >      
                </div>
                {stepStatus.map((status, index) => (
                    <div key={index} className="step-wrapper d-flex align-items-center position-relative">
                  
                    <div className="step-background"></div>
                 
                    <div 
                        style={{ width: "40px", height: "40px", borderRadius: "50%" }} 
                        className="d-flex border align-items-center justify-content-center step-circle"
                    >
                        {status ? '✔' : '✖'} 
                        
                    </div>
                 
                    <div className="d-flex flex-column ms-3">
                        <div style={{ fontSize: "10px" }}>STEP {index + 1}</div>
                        <div style={{ fontSize: "12px", fontWeight: "600" }}>
                        {index === 0 ? 'Topup Confirm' : index === 1 ? 'Deposit' : 'Push Notification'}
                        </div>
                    </div>
                    </div>
                ))}
             </div> */}
        </aside>
    )
}