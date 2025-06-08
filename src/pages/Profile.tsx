
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Separator } from "@/components/ui/separator";
import { Badge } from "@/components/ui/badge";
import { Switch } from "@/components/ui/switch";
import { 
  User, 
  Mail, 
  Shield, 
  Bell, 
  Palette, 
  Globe, 
  Clock,
  Activity,
  Trash2,
  Edit,
  Save,
  Camera
} from "lucide-react";

export default function Profile() {
  return (
    <main className="flex-1 overflow-auto bg-gradient-to-br from-background via-background to-muted/20">
      <div className="container mx-auto p-8 space-y-8 max-w-6xl">
        {/* Modern Header Section */}
        <div className="relative overflow-hidden rounded-3xl bg-gradient-to-r from-primary/10 via-primary/5 to-transparent p-8 border border-border/50">
          <div className="absolute inset-0 bg-gradient-to-r from-primary/5 to-transparent opacity-50" />
          <div className="relative flex items-center justify-between">
            <div className="flex items-center space-x-6">
              <div className="relative group">
                <Avatar className="h-24 w-24 border-4 border-background shadow-2xl transition-all duration-300 group-hover:scale-105">
                  <AvatarFallback className="bg-gradient-to-br from-primary via-primary/90 to-primary/80 text-2xl font-bold text-primary-foreground">
                    AI
                  </AvatarFallback>
                </Avatar>
                <Button
                  size="sm"
                  className="absolute -bottom-2 -right-2 rounded-full h-8 w-8 p-0 shadow-lg"
                  variant="secondary"
                >
                  <Camera className="h-4 w-4" />
                </Button>
              </div>
              <div className="space-y-2">
                <div className="flex items-center gap-3">
                  <h1 className="text-4xl font-bold bg-gradient-to-r from-foreground to-foreground/80 bg-clip-text">
                    AI Assistant
                  </h1>
                  <Badge variant="secondary" className="px-3 py-1">
                    <Activity className="w-3 h-3 mr-1" />
                    Active
                  </Badge>
                </div>
                <p className="text-lg text-muted-foreground">Conversion Rate Optimization Expert</p>
                <div className="flex items-center gap-4 text-sm text-muted-foreground">
                  <div className="flex items-center gap-1">
                    <Clock className="w-4 h-4" />
                    Last seen: 2 minutes ago
                  </div>
                  <div className="flex items-center gap-1">
                    <Globe className="w-4 h-4" />
                    admin@aico.com
                  </div>
                </div>
              </div>
            </div>
            <Button variant="outline" className="gap-2">
              <Edit className="w-4 h-4" />
              Edit Profile
            </Button>
          </div>
        </div>

        {/* Modern Grid Layout */}
        <div className="grid gap-8 lg:grid-cols-3">
          {/* Left Column - Main Info */}
          <div className="lg:col-span-2 space-y-6">
            <Card className="border-0 shadow-xl bg-gradient-to-br from-card to-card/80">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-xl">
                  <div className="p-2 bg-primary/10 rounded-xl">
                    <User className="h-5 w-5 text-primary" />
                  </div>
                  Personal Information
                </CardTitle>
                <CardDescription className="text-base">
                  Manage your personal details and account information
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="grid gap-6 md:grid-cols-2">
                  <div className="space-y-3">
                    <Label htmlFor="name" className="text-sm font-medium">Display Name</Label>
                    <Input 
                      id="name" 
                      defaultValue="AI Assistant" 
                      className="h-11 rounded-xl border-border/50 focus:border-primary transition-colors"
                    />
                  </div>
                  <div className="space-y-3">
                    <Label htmlFor="email" className="text-sm font-medium">Email Address</Label>
                    <Input 
                      id="email" 
                      type="email" 
                      defaultValue="admin@aico.com" 
                      className="h-11 rounded-xl border-border/50 focus:border-primary transition-colors"
                    />
                  </div>
                </div>
                <div className="space-y-3">
                  <Label htmlFor="role" className="text-sm font-medium">Role</Label>
                  <Input 
                    id="role" 
                    defaultValue="CRO Expert" 
                    disabled 
                    className="h-11 rounded-xl bg-muted/50"
                  />
                </div>
                <div className="flex gap-3 pt-2">
                  <Button className="gap-2 rounded-xl">
                    <Save className="w-4 h-4" />
                    Save Changes
                  </Button>
                  <Button variant="outline" className="rounded-xl">
                    Cancel
                  </Button>
                </div>
              </CardContent>
            </Card>

            <Card className="border-0 shadow-xl bg-gradient-to-br from-card to-card/80">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-xl">
                  <div className="p-2 bg-destructive/10 rounded-xl">
                    <Shield className="h-5 w-5 text-destructive" />
                  </div>
                  Security Settings
                </CardTitle>
                <CardDescription className="text-base">
                  Update your password and security preferences
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="space-y-4">
                  <div className="space-y-3">
                    <Label htmlFor="current-password" className="text-sm font-medium">Current Password</Label>
                    <Input 
                      id="current-password" 
                      type="password" 
                      className="h-11 rounded-xl border-border/50 focus:border-primary transition-colors"
                    />
                  </div>
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-3">
                      <Label htmlFor="new-password" className="text-sm font-medium">New Password</Label>
                      <Input 
                        id="new-password" 
                        type="password" 
                        className="h-11 rounded-xl border-border/50 focus:border-primary transition-colors"
                      />
                    </div>
                    <div className="space-y-3">
                      <Label htmlFor="confirm-password" className="text-sm font-medium">Confirm Password</Label>
                      <Input 
                        id="confirm-password" 
                        type="password" 
                        className="h-11 rounded-xl border-border/50 focus:border-primary transition-colors"
                      />
                    </div>
                  </div>
                </div>
                <Button variant="outline" className="gap-2 rounded-xl">
                  <Shield className="w-4 h-4" />
                  Update Password
                </Button>
              </CardContent>
            </Card>
          </div>

          {/* Right Column - Settings & Activity */}
          <div className="space-y-6">
            <Card className="border-0 shadow-xl bg-gradient-to-br from-card to-card/80">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-lg">
                  <div className="p-2 bg-blue-500/10 rounded-xl">
                    <Bell className="h-5 w-5 text-blue-600" />
                  </div>
                  Notifications
                </CardTitle>
                <CardDescription>
                  Configure your notification preferences
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="space-y-4">
                  <div className="flex items-center justify-between p-3 rounded-xl bg-muted/30">
                    <div className="space-y-1">
                      <p className="font-medium text-sm">Email Notifications</p>
                      <p className="text-xs text-muted-foreground">Get updates via email</p>
                    </div>
                    <Switch defaultChecked />
                  </div>
                  <div className="flex items-center justify-between p-3 rounded-xl bg-muted/30">
                    <div className="space-y-1">
                      <p className="font-medium text-sm">CRO Alerts</p>
                      <p className="text-xs text-muted-foreground">Optimization recommendations</p>
                    </div>
                    <Switch defaultChecked />
                  </div>
                  <div className="flex items-center justify-between p-3 rounded-xl bg-muted/30">
                    <div className="space-y-1">
                      <p className="font-medium text-sm">Weekly Reports</p>
                      <p className="text-xs text-muted-foreground">Performance summaries</p>
                    </div>
                    <Switch />
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card className="border-0 shadow-xl bg-gradient-to-br from-card to-card/80">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-lg">
                  <div className="p-2 bg-green-500/10 rounded-xl">
                    <Activity className="h-5 w-5 text-green-600" />
                  </div>
                  Account Activity
                </CardTitle>
                <CardDescription>
                  Recent account activity and statistics
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-4">
                  <div className="flex justify-between items-center p-3 rounded-xl bg-muted/30">
                    <span className="text-sm font-medium">Last Login</span>
                    <Badge variant="outline">Today, 2:30 PM</Badge>
                  </div>
                  <div className="flex justify-between items-center p-3 rounded-xl bg-muted/30">
                    <span className="text-sm font-medium">Member Since</span>
                    <Badge variant="outline">Jan 1, 2024</Badge>
                  </div>
                  <div className="flex justify-between items-center p-3 rounded-xl bg-muted/30">
                    <span className="text-sm font-medium">Total Sessions</span>
                    <Badge variant="outline">1,247</Badge>
                  </div>
                </div>
                <Separator className="my-4" />
                <Button variant="destructive" className="w-full gap-2 rounded-xl">
                  <Trash2 className="w-4 h-4" />
                  Delete Account
                </Button>
              </CardContent>
            </Card>

            <Card className="border-0 shadow-xl bg-gradient-to-br from-card to-card/80">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-lg">
                  <div className="p-2 bg-purple-500/10 rounded-xl">
                    <Palette className="h-5 w-5 text-purple-600" />
                  </div>
                  Preferences
                </CardTitle>
                <CardDescription>
                  Customize your experience
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="flex items-center justify-between p-3 rounded-xl bg-muted/30">
                  <div className="space-y-1">
                    <p className="font-medium text-sm">Dark Mode</p>
                    <p className="text-xs text-muted-foreground">Toggle dark theme</p>
                  </div>
                  <Switch />
                </div>
                <div className="flex items-center justify-between p-3 rounded-xl bg-muted/30">
                  <div className="space-y-1">
                    <p className="font-medium text-sm">Auto-save</p>
                    <p className="text-xs text-muted-foreground">Save changes automatically</p>
                  </div>
                  <Switch defaultChecked />
                </div>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </main>
  );
}
