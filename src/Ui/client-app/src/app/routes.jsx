// import Layout from "@shared/components/Layout";
import Layout from "#shared/components/Layout/Layout";
import LoginPage from "#features/identity/identities/pages/LoginPage";
import TopupPage from "#features/topup/topups/pages/TopupPage";
import PreferencePage from "#features/userProfile/userPreferences/pages/PreferencePage";
import SettingPage from "#app/pages/Settings/SettingPage";
import NotificationPage from "#features/notification/notifications/pages/NotifcationPage";
import SignUpPage from "#features/identity/identities/pages/SignUpPage";
import NotFoundPage from "#app/pages/NotFoundPage";
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
        element: <SignUpPage/>
    }
]