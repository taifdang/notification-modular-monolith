import { Outlet } from "react-router-dom";
import RightSide from "#shared/components/Layout/RightSide"
import LeftSide from "#shared/components/Layout/LeftSide"

export default function Layout(){
    return(
        <div className="d-flex flex-column min-vh-100 pt-2 group__layout">
            {/* <Header /> */}
            <div className="flex-grow-1 d-flex justify-content-center">
                <div className="container-fluid ">
                    <div className="row p-0 m-0">
                       <LeftSide />
                        <main className="col-12 col-md-7 mb-3 mb-md-0 border-end border-start">
                            <Outlet />
                        </main>      
                        <RightSide />
                    </div>
                </div>
            </div>
        </div>
    )
}