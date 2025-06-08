
import { useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarProvider,
  SidebarTrigger,
  useSidebar,
} from "@/components/ui/sidebar";
import {
  LayoutDashboard,
  Search,
  Settings,
  BarChart3,
  Zap,
  Sparkles,
  MessageCircle,
} from "lucide-react";
import { CROExpertChat } from "../CROExpertChat/CROExpertChat";

const navigation = [
  { 
    name: "Dashboard", 
    href: "/dashboard", 
    icon: LayoutDashboard,
    description: "Overview & metrics"
  },
  { 
    name: "Analysis", 
    href: "/analysis", 
    icon: Search,
    description: "Site optimization"
  },
  { 
    name: "Competitor Analysis", 
    href: "/competitor-analysis", 
    icon: BarChart3,
    description: "Market insights"
  },
  { 
    name: "Settings", 
    href: "/settings", 
    icon: Settings,
    description: "Account preferences"
  },
];

export function ModernSidebar() {
  const location = useLocation();
  const [showCROChat, setShowCROChat] = useState(false);

  return (
    <Sidebar className="border-r border-border/40 bg-gradient-to-b from-sidebar to-sidebar/95">
      <SidebarHeader className="p-6 border-b border-sidebar-border/50">
        <div className="flex items-center space-x-3">
          <div className="relative">
            <div className="flex items-center justify-center w-10 h-10 bg-gradient-to-br from-primary via-primary to-primary/80 rounded-xl shadow-lg">
              <Zap className="w-6 h-6 text-white" />
            </div>
            <div className="absolute -top-1 -right-1 w-4 h-4 bg-green-500 rounded-full border-2 border-sidebar animate-pulse" />
          </div>
          <div className="flex-1">
            <h1 className="text-xl font-bold bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent">
              AICO
            </h1>
            <p className="text-xs text-muted-foreground">CRO Analytics Platform</p>
          </div>
        </div>
      </SidebarHeader>

      <SidebarContent className="px-4">
        <SidebarGroup>
          <SidebarGroupLabel className="text-xs font-semibold text-muted-foreground/70 uppercase tracking-wider mb-2">
            Navigation
          </SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu className="space-y-1">
              {navigation.map((item) => {
                const isActive = location.pathname === item.href || 
                  (item.href !== "/dashboard" && location.pathname.startsWith(item.href));
                return (
                  <SidebarMenuItem key={item.name}>
                    <SidebarMenuButton asChild className={cn(
                      "group relative h-12 rounded-lg transition-all duration-200",
                      isActive ? 
                        "bg-primary/10 text-primary border border-primary/20 shadow-sm" : 
                        "hover:bg-sidebar-accent/50 hover:text-sidebar-accent-foreground"
                    )}>
                      <Link to={item.href} className="flex items-center space-x-3 px-3 py-2">
                        <item.icon className={cn(
                          "w-5 h-5 transition-colors flex-shrink-0",
                          isActive ? "text-primary" : "text-muted-foreground group-hover:text-sidebar-accent-foreground"
                        )} />
                        <div className="flex-1 min-w-0 text-left">
                          <div className="font-medium text-sm leading-tight">{item.name}</div>
                          <div className="text-xs text-muted-foreground/70 leading-tight">{item.description}</div>
                        </div>
                        {isActive && (
                          <div className="w-2 h-2 bg-primary rounded-full animate-pulse flex-shrink-0" />
                        )}
                      </Link>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                );
              })}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter className="p-4 border-t border-sidebar-border/50">
        <Button
          variant="ghost"
          onClick={() => setShowCROChat(true)}
          className="flex items-center space-x-3 p-3 bg-gradient-to-r from-muted/50 to-muted/30 rounded-lg border border-border/30 h-auto hover:bg-gradient-to-r hover:from-primary/10 hover:to-primary/5 transition-all duration-200"
        >
          <div className="relative">
            <div className="w-8 h-8 bg-gradient-to-br from-primary to-primary/60 rounded-full flex items-center justify-center shadow-sm">
              <MessageCircle className="w-4 h-4 text-white" />
            </div>
            <div className="absolute -bottom-1 -right-1 w-3 h-3 bg-green-500 rounded-full border border-sidebar" />
          </div>
          <div className="flex-1 min-w-0 text-left">
            <p className="text-sm font-medium">CRO Expert</p>
            <p className="text-xs text-muted-foreground">AI Assistant • Online</p>
          </div>
          <div className="text-xs text-muted-foreground">
            <Sparkles className="w-4 h-4 text-primary animate-pulse" />
          </div>
        </Button>
      </SidebarFooter>

      {showCROChat && (
        <CROExpertChat 
          isOpen={showCROChat} 
          onClose={() => setShowCROChat(false)} 
        />
      )}
    </Sidebar>
  );
}
