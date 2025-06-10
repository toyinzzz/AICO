
import { NotificationCenter, useNotifications } from "@/components/ui/notifications";
import { useNavigate } from "react-router-dom";
import { Bell } from "lucide-react";

export default function NotificationsPage() {
  const navigate = useNavigate();
  const {
    notifications,
    markAsRead,
    markAllAsRead,
    dismissNotification,
    clearAll,
  } = useNotifications();

  const handleAction = (notification: any) => {
    if (notification.actionUrl) {
      navigate(notification.actionUrl);
    }
  };

  return (
    <div className="flex-1 bg-gradient-to-br from-background via-background to-muted/20">
      <main className="flex-1 overflow-y-auto p-8">
        <div className="max-w-4xl mx-auto space-y-8">
          <div className="space-y-2">
            <h1 className="text-4xl font-bold bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent flex items-center gap-3">
              <div className="p-2 bg-primary/20 rounded-xl">
                <Bell className="h-8 w-8 text-primary" />
              </div>
              Notifications
            </h1>
            <p className="text-muted-foreground text-lg">
              Stay updated with your website optimization progress and recommendations
            </p>
          </div>
          
          <div className="border-0 shadow-lg rounded-xl bg-card/50 backdrop-blur-sm">
            <NotificationCenter
              notifications={notifications}
              onMarkAsRead={markAsRead}
              onMarkAllAsRead={markAllAsRead}
              onDismiss={dismissNotification}
              onClearAll={clearAll}
              onAction={handleAction}
            />
          </div>
        </div>
      </main>
    </div>
  );
}
