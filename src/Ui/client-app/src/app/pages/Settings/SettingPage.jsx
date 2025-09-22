import AvatarDefault from "#assets/images/pigman.jpg";
import { useNavigate } from "react-router-dom";
import {
  ArrowLeftIcon,
  ArrowRightIcon,
  PersonIcon,
  BellIcon,
} from "#shared/components/icons";
import { Card, Link, Name, Avatar } from "#shared/components/ProfileCard";

export default function SettingPage() {
  const navigate = useNavigate();
  return (
    <div>
      <nav className="border-bottom position-sticky py-2 mb-3 d-flex align-items-center gap-3">
        <Card className="d-flex align-items-center gap-2">
          <ArrowLeftIcon size={24} />
          <Name text="Settings" style={{ fontWeight: 700, fontSize: "16px" }} />
        </Card>
      </nav>
      <Card>
        <Card className="d-flex flex-column align-items-center gap-1 py-3">
          <Card>
            <Avatar src={AvatarDefault} style={{ "max-width": "112px", height: "auto" }}/>
          </Card>
          <Name text="Anonymous" style={{ fontSize: "30px", fontWeight: 800 }}/>
          <Name text="@user_test" />
        </Card>
        <hr />
        <Card>
          <Link href="/nopage">
            <PersonIcon size={24} />
            <Name text="Account" />
            <ArrowRightIcon size={24} className="ms-auto" />
          </Link>
        </Card>
        <Card>
          <Link
            href="/settings/notification"
            onClick={(e) => {
              e.preventDefault();
              navigate("/settings/notification");
            }}>
            <BellIcon size={24} />
            <Name text="Notification" />
            <ArrowRightIcon size={24} className="ms-auto" />
          </Link>
        </Card>
        <hr />
        <Card className="d-flex cardProfile">
          <button className="btn flex-fill text-start border-0" style={{ padding: "12px" }}>
            <Name text="Sign out" style={{ color: "red" }} />
          </button>
        </Card>
      </Card>
    </div>
  );
}
