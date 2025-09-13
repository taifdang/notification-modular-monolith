import { Outlet } from "react-router-dom";
import Footer from "./Footer";
import Header from "./Header";
import LeftSide from "./LeftSide";
import RightBar from "./RightBar";

export default function Layout(){
    return(
        <div className="d-flex flex-column min-vh-100 pt-2 group__layout">
            {/* <Header /> */}
            <div className="flex-grow-1 d-flex justify-content-center">
                <div className="container-fluid ">
                    <div className="row">
                        {/* Left Sidebar */}
                       <LeftSide />
                        {/* Main Content */}
                        <main className="col-12 col-md-7 mb-3 mb-md-0 border-end border-start">
                            <Outlet />
                        </main>      
                         {/* Right Bar  */}
                        <RightBar />
                    </div>
                </div>
            </div>
            {/* <Footer /> */}
        </div>
    )
}