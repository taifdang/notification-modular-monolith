import Layout from "../shared/components/Layout";
import LoginPage from "../features/identity/identities/pages/LoginPage";
import TopupPage from "../features/topup/topups/pages/TopupPage";
import PreferencePage from "../features/userProfile/userPreferences/pages/PreferencePage";
import SettingPage from "../shared/pages/SettingPage";
import NotificationPage from "../features/notification/notifications/pages/NotifcationPage";
import RegisterPage from "../features/identity/identities/pages/RegisterPage";
import NotFoundPage from "../shared/pages/NotFoundPage";
export const routes = [
    {
        element: <Layout/>,
        errorElement: <NotFoundPage/>,
        children:[
            {path:"/", element: <TopupPage/> },
            {path:"/topup", element:<TopupPage/> },
            {path:"/settings", element:<SettingPage/> },
            {path:"/settings/notification", element:<PreferencePage/> },
            {path:"/notifications", element:<NotificationPage/> },
        ],      
    },
    {
        path: "/login",
        element: <LoginPage />,
    },
    {
        path: "/signup",
        element: <RegisterPage/>
    }
]