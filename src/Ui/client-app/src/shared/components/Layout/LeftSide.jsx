import AvatarDefault from "#assets/images/pigman.jpg";
import { Avatar } from "../ProfileCard";
import { Card, Link, Name } from "../ProfileCard";
import { HomeIcon, SettingIcon ,BellIcon, CreditIcon, ExitIcon } from "../icons"

export default function LeftSide() {
  return (
    <nav className="col-12 col-md-2 mb-3 mb-md-0 pt-4 px-3">
      <div>
        <Card className="d-flex align-items-center">
           <Avatar 
            src={AvatarDefault} 
            className="img-thumbnail" 
            style={{marginBottom:"10px"}}/> 
        </Card>
        <Card>
          <Link href="/">
            <HomeIcon size={24}/>
            <Name text="Home" />       
          </Link>
        </Card>
        <Card>
          <Link href="/notifications">
            <BellIcon size={24}/>
            <Name text="Notifications" />     
          </Link>
        </Card>
        <Card>
          <Link href="/topup">
            <CreditIcon size={24}/>
            <Name text="Topup" />
          </Link>
        </Card>
        <Card>
          <Link href="/settings">
            <SettingIcon size={24}/>
            <Name text="Settings" />
          </Link>
        </Card>
        <Card>
          <Link href="/login">
            <ExitIcon size={24}/>
            <Name text="Logout" />
          </Link>
        </Card>
      </div>
    </nav>
  );
}
