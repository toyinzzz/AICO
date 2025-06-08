import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import { useTheme } from "@/providers/ThemeProvider";
import { Bell, Search, Sun, Moon, User, Settings, LogOut } from "lucide-react";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { useNavigate } from "react-router-dom";

export function Header() {
  const { theme, setTheme } = useTheme();
  const navigate = useNavigate();

  const handleProfileClick = () => {
    navigate("/profile");
  };

  const handleSettingsClick = () => {
    navigate("/settings");
  };

  const handleNotificationsClick = () => {
    navigate("/notifications");
  };

  const handleSignOut = () => {
    // Clear any authentication tokens/session data here
    localStorage.clear();
    sessionStorage.clear();
    navigate("/");
  };

  return (
    <header className="flex items-center justify-between px-6 py-3.5 bg-background border-b shadow-soft">
      <div className="flex items-center space-x-4">
        <div className="relative w-80 max-w-full">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground w-4 h-4" />
          <Input
            placeholder="Search sites, recommendations..."
            className="pl-10 rounded-full bg-background border-muted"
          />
        </div>
      </div>

      <div className="flex items-center space-x-3">
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button 
              variant="ghost" 
              size="sm" 
              className="rounded-full relative animate-fade-in hover:bg-accent/50 transition-all duration-300 group"
            >
              <Bell className="w-5 h-5 transition-all duration-300 group-hover:scale-110 group-hover:text-primary" />
              <Badge 
                variant="destructive" 
                className="absolute -top-1 -right-1 px-1.5 min-w-[18px] h-5 flex items-center justify-center text-xs rounded-full animate-pulse shadow-sm border-2 border-background group-hover:scale-110 transition-transform duration-300"
              >
                3
              </Badge>
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-80 animate-scale-in">
            <div className="p-3 border-b">
              <h4 className="font-medium text-sm">Notifications</h4>
              <p className="text-xs text-muted-foreground">You have 3 unread notifications</p>
            </div>
            <DropdownMenuItem className="cursor-pointer p-3 hover:bg-accent/50">
              <div className="flex flex-col space-y-1">
                <p className="text-sm font-medium">New recommendation available</p>
                <p className="text-xs text-muted-foreground">Site optimization suggestions for your homepage</p>
                <p className="text-xs text-muted-foreground">2 minutes ago</p>
              </div>
            </DropdownMenuItem>
            <DropdownMenuItem className="cursor-pointer p-3 hover:bg-accent/50">
              <div className="flex flex-col space-y-1">
                <p className="text-sm font-medium">Analysis completed</p>
                <p className="text-xs text-muted-foreground">Your site analysis has finished processing</p>
                <p className="text-xs text-muted-foreground">1 hour ago</p>
              </div>
            </DropdownMenuItem>
            <DropdownMenuItem className="cursor-pointer p-3 hover:bg-accent/50">
              <div className="flex flex-col space-y-1">
                <p className="text-sm font-medium">Weekly report ready</p>
                <p className="text-xs text-muted-foreground">Your weekly performance report is available</p>
                <p className="text-xs text-muted-foreground">3 hours ago</p>
              </div>
            </DropdownMenuItem>
            <div className="p-3 border-t">
              <Button 
                variant="ghost" 
                size="sm" 
                className="w-full text-xs"
                onClick={handleNotificationsClick}
              >
                View all notifications
              </Button>
            </div>
          </DropdownMenuContent>
        </DropdownMenu>

        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button 
              variant="ghost" 
              size="sm" 
              className="rounded-full w-9 h-9 p-0 animate-fade-in"
            >
              {theme === "light" ? <Sun className="w-5 h-5" /> : <Moon className="w-5 h-5" />}
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-40 animate-scale-in">
            <DropdownMenuItem onClick={() => setTheme("light")} className="cursor-pointer">
              <Sun className="mr-2 h-4 w-4" />
              Light
            </DropdownMenuItem>
            <DropdownMenuItem onClick={() => setTheme("dark")} className="cursor-pointer">
              <Moon className="mr-2 h-4 w-4" />
              Dark
            </DropdownMenuItem>
            <DropdownMenuItem onClick={() => setTheme("system")} className="cursor-pointer">
              <span className="mr-2 flex h-4 w-4 items-center justify-center text-xs">
                <Sun className="absolute h-3 w-3 rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100" />
                <Moon className="h-3 w-3 rotate-0 scale-100 transition-all dark:rotate-90 dark:scale-0" />
              </span>
              System
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>

        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button 
              variant="ghost" 
              size="sm" 
              className="rounded-full p-0 animate-fade-in flex items-center justify-center hover:bg-accent/50 transition-all duration-300 group relative overflow-hidden"
            >
              <Avatar className="h-9 w-9 border-2 border-primary/20 shadow-lg transition-all duration-300 group-hover:border-primary/40 group-hover:shadow-xl group-hover:scale-105 relative overflow-hidden">
                <AvatarFallback className="bg-gradient-to-br from-primary to-primary/80 text-sm font-bold text-primary-foreground transition-all duration-300 group-hover:from-primary/90 group-hover:to-primary relative overflow-hidden">
                  <span className="relative z-10">AI</span>
                  <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/10 to-transparent transform -skew-x-12 -translate-x-full group-hover:translate-x-full transition-transform duration-700"></div>
                </AvatarFallback>
              </Avatar>
              <div className="absolute inset-0 rounded-full bg-gradient-to-r from-primary/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-56 animate-scale-in">
            <div className="flex items-center justify-start p-2">
              <div className="flex flex-col space-y-0.5 leading-none">
                <p className="font-medium text-sm">AI Assistant</p>
                <p className="text-xs text-muted-foreground">admin@aico.com</p>
              </div>
            </div>
            <DropdownMenuSeparator />
            <DropdownMenuItem 
              className="cursor-pointer hover:bg-accent/50 transition-colors"
              onClick={handleProfileClick}
            >
              <User className="mr-2 h-4 w-4" />
              Profile
            </DropdownMenuItem>
            <DropdownMenuItem 
              className="cursor-pointer hover:bg-accent/50 transition-colors"
              onClick={handleSettingsClick}
            >
              <Settings className="mr-2 h-4 w-4" />
              Settings
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem 
              className="cursor-pointer text-destructive hover:bg-destructive/10 hover:text-destructive focus:text-destructive transition-colors"
              onClick={handleSignOut}
            >
              <LogOut className="mr-2 h-4 w-4" />
              Sign out
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </header>
  );
}
