import { Card, Link, Name } from "#shared/components/ProfileCard";
import QrCodeImg from "#assets/images/qrcode.jpg";
import ZaloPayIcon from "#assets/images/zalopaylogo.jpg";
import MomoIcon from "#assets/images/momologo.jpg";
import VcbIcon from "#assets/images/vcblogo.jpg";
import MbbankIcon from "#assets/images/mbbanklogo.jpg";
import VtbLogo from "#assets/images/vtblogo.jpg";
import Bidvlogo from "#assets/images/bidvlogo.jpg";

export function QrCode() {
  return (
    <div>
      <div className="d-flex flex-column align-items-center">
        <div className="mb-3 fs-5 fw-bold" style={{ fontFamily: "Inter" }}>
          Quét QR để thanh toán
        </div>
        <div
          style={{
            backgroundColor: "rgb(239,243,251)",
            borderRadius: "4px",
            padding: "12px",
          }}
        >
          {/* QrCode */}
          <div className="position-relative p-2 ">
            <div
              className="p-3"
              style={{
                width: "200px",
                height: "200px",
                backgroundColor: "rgba(255,255,255)",
                borderRadius: "4px",
              }}
            >
              <img
                src={QrCodeImg}
                alt="QR Code"
                style={{ maxWidth: "100%", maxHeight: "100%" }}
              />
            </div>
            <span className="corner top-left"></span>
            <span className="corner top-right"></span>
            <span className="corner bottom-left"></span>
            <span className="corner bottom-right"></span>
          </div>
          {/* Description */}
          <div className="d-flex flex-column gap-1 align-items-center mt-2">
            <div
              style={{
                marginTop: "8px",
                fontSize: "13.125px",
                fontFamily: "Inter",
                fontWeight: "600",
                color: "#42576c",
                lineHeight: "13.125px",
              }}
            >
              Ngân hàng thụ hưởng: MBBank
            </div>
            <div
              style={{
                marginTop: "8px",
                fontSize: "13.125px",
                fontFamily: "Inter",
                fontWeight: "700",
                color: "rgb(46,60,72)",
                lineHeight: "13.125px",
              }}
            >
              NAPCOIN4000
            </div>
            <div className="d-flex justify-content-center align-items-center gap-2">
              <div
                style={{
                  fontSize: "13.125px",
                  fontFamily: "Inter",
                  fontWeight: "600",
                  color: "#42576c",
                  lineHeight: "13.125px",
                }}
              >
                99ZP24334000725953
              </div>
              <a href="#">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="18"
                  height="18"
                  viewBox="0 0 24 24"
                >
                  <g fill="none" stroke="currentColor" strokeWidth="1.5">
                    <path d="M6 11c0-2.828 0-4.243.879-5.121C7.757 5 9.172 5 12 5h3c2.828 0 4.243 0 5.121.879C21 6.757 21 8.172 21 11v5c0 2.828 0 4.243-.879 5.121C19.243 22 17.828 22 15 22h-3c-2.828 0-4.243 0-5.121-.879C6 20.243 6 18.828 6 16z" />
                    <path d="M6 19a3 3 0 0 1-3-3v-6c0-3.771 0-5.657 1.172-6.828S7.229 2 11 2h4a3 3 0 0 1 3 3" />
                  </g>
                </svg>
              </a>
            </div>
          </div>
        </div>
        <div className="mt-4 fw-bold" style={{ fontSize:"14px",fontFamily: "Inter" }}>
          Mở ứng dụng có VietQR để thanh toán
        </div>
        <div className="mt-3 d-flex flex-column gap-0">
           <div className="d-flex">
            
            <div 
            className="d-flex align-items-center justify-content-center border rounded-circle "
            style={{width:'48px',height:'48px',zIndex:10,backgroundColor:'rgb(255,255,255)'}}>
              <div
              className="d-flex"
              style={{width:'32px',height:'32px',zIndex:10}}>
                   <img
                  src={ZaloPayIcon}
                  className=""
                  alt="QR Code"
                  style={{maxWidth: "100%", maxHeight: "100%" }}/>
              </div>
            </div>
             <div 
            className="d-flex align-items-center justify-content-center border rounded-circle "
            style={{width:'48px',height:'48px',zIndex:15,backgroundColor:'rgb(255,255,255)',marginLeft:'-8px'}}>
              <div
              className="d-flex"
              style={{width:'32px',height:'32px'}}>
                   <img
                  src={MomoIcon}
                  className=""
                  alt="QR Code"
                  style={{maxWidth: "100%", maxHeight: "100%" }}/>
              </div>
            </div>
             <div 
            className="d-flex align-items-center justify-content-center border rounded-circle "
            style={{width:'48px',height:'48px',zIndex:15,backgroundColor:'rgb(255,255,255)',marginLeft:'-8px'}}>
              <div
              className="d-flex"
              style={{width:'32px',height:'32px'}}>
                   <img
                  src={VcbIcon}
                  className=""
                  alt="QR Code"
                  style={{maxWidth: "100%", maxHeight: "100%" }}/>
              </div>
            </div>
             <div 
            className="d-flex align-items-center justify-content-center border rounded-circle "
            style={{width:'48px',height:'48px',zIndex:15,backgroundColor:'rgb(255,255,255)',marginLeft:'-8px'}}>
              <div
              className="d-flex"
              style={{width:'32px',height:'32px'}}>
                   <img
                  src={VtbLogo}
                  className=""
                  alt="QR Code"
                  style={{maxWidth: "100%", maxHeight: "100%" }}/>
              </div>
            </div>
             <div 
            className="d-flex align-items-center justify-content-center border rounded-circle "
            style={{width:'48px',height:'48px',zIndex:15,backgroundColor:'rgb(255,255,255)',marginLeft:'-8px'}}>
              <div
              className="d-flex"
              style={{width:'32px',height:'32px'}}>
                   <img
                  src={MbbankIcon}
                  className=""
                  alt="QR Code"
                  style={{maxWidth: "100%", maxHeight: "100%" }}/>
              </div>
            </div>
             <div 
            className="d-flex align-items-center justify-content-center border rounded-circle "
            style={{width:'48px',height:'48px',zIndex:15,backgroundColor:'rgba(108, 148, 228, 1)',marginLeft:'-8px'}}>
              <div
              className="d-flex align-items-center"
              style={{width:'32px',height:'32px'}}>
                  <div style={{color:'rgb(255,255,255)'}}>+34</div>
              </div>
            </div>             
           </div>       
        </div>
        <div className="mt-3">
           <a href="#" className="d-flex align-items-center gap-1">
            <div style={{ fontSize:"13.125px",fontFamily: "Inter",fontWeight:700 }}>Hướng dẫn thanh toán</div>
            <div>
              <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24">
                <g fill="none">
                  <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="1.5" />
                  <path stroke="currentColor" stroke-linecap="round" stroke-width="1.5" d="M10.125 8.875a1.875 1.875 0 1 1 2.828 1.615c-.475.281-.953.708-.953 1.26V13" />
                  <circle cx="12" cy="16" r="1" fill="currentColor" />
                </g>
              </svg>
            </div>
          </a>

        </div>
      </div>
    </div>
  );
}
