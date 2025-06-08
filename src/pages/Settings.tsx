import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Badge } from "@/components/ui/badge";
import { Switch } from "@/components/ui/switch";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useForm } from "react-hook-form";
import { toast } from "@/hooks/use-toast";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { mockSites } from "@/lib/mockData";
import {
  BarChart3,
  ShoppingCart,
  CreditCard,
  Mail,
  Bell,
  Shield,
  Key,
  Webhook,
  Globe,
  Trash2,
  Plus,
  Check,
  Settings as SettingsIcon
} from "lucide-react";

export default function Settings() {
  const [sites, setSites] = useState(mockSites);
  const [currentPlan, setCurrentPlan] = useState("Professional");
  const [selectedPlan, setSelectedPlan] = useState("Professional");
  const [isChangingPlan, setIsChangingPlan] = useState(false);
  
  const plans = [
    {
      name: "Starter",
      price: 29,
      features: [
        "3 websites",
        "Basic recommendations", 
        "Email support"
      ]
    },
    {
      name: "Professional",
      price: 99,
      features: [
        "10 websites",
        "Advanced AI recommendations",
        "Real-time metrics",
        "Priority support"
      ]
    },
    {
      name: "Agency",
      price: 299,
      features: [
        "Unlimited websites",
        "White-label reports",
        "Multi-client management",
        "Dedicated support"
      ]
    }
  ];

  const integrations = [
    {
      name: "Google Analytics 4",
      icon: BarChart3,
      description: "Connect GA4 to track conversion metrics and user behavior",
      status: "connected",
      lastSync: "2 hours ago"
    },
    {
      name: "Shopify",
      icon: ShoppingCart, 
      description: "Sync e-commerce data, orders, and revenue metrics",
      status: "disconnected",
      lastSync: null
    },
    {
      name: "Stripe",
      icon: CreditCard,
      description: "Track payment conversions and revenue attribution",
      status: "connected",
      lastSync: "5 minutes ago"
    }
  ];

  const { register, handleSubmit, reset, formState: { errors } } = useForm({
    defaultValues: {
      siteName: "",
      siteUrl: ""
    }
  });

  const onSubmit = (data: { siteName: string, siteUrl: string }) => {
    // Generate a unique ID for the new site
    const newSiteId = `${sites.length + 1}`;
    
    // Create the new site object
    const newSite = {
      id: newSiteId,
      name: data.siteName,
      url: data.siteUrl,
      score: 0,
      conversionRate: 0,
      revenue: 0,
      change: 0,
      status: "pending",
      lastAnalyzed: "Never"
    };

    // Add the new site to the sites array
    setSites([...sites, newSite]);
    
    // Show success message
    toast({
      title: "Site added",
      description: `${data.siteName} was successfully added.`,
    });
    
    // Reset the form
    reset();
  };

  const deleteSite = (siteId: string) => {
    setSites(sites.filter(site => site.id !== siteId));
    toast({
      title: "Site removed",
      description: "The site was successfully removed.",
      variant: "destructive"
    });
  };

  const handlePlanChange = (planName: string) => {
    if (planName === currentPlan) return;
    
    setIsChangingPlan(true);
    setSelectedPlan(planName);
    
    // Simulate plan change
    setTimeout(() => {
      setCurrentPlan(planName);
      setIsChangingPlan(false);
      
      if (planName === "Agency") {
        toast({
          title: "Plan upgraded!",
          description: `Successfully upgraded to ${planName} plan.`,
        });
      } else if (planName === "Starter") {
        toast({
          title: "Plan downgraded",
          description: `Successfully changed to ${planName} plan.`,
          variant: "destructive"
        });
      } else {
        toast({
          title: "Plan changed",
          description: `Successfully changed to ${planName} plan.`,
        });
      }
    }, 2000);
  };

  return (
    <main className="flex-1 overflow-y-auto p-8">
      <div className="max-w-6xl mx-auto space-y-8">
        <div className="space-y-2">
          <h1 className="text-4xl font-bold bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent flex items-center gap-3">
            <div className="p-2 bg-primary/20 rounded-xl">
              <SettingsIcon className="h-8 w-8 text-primary" />
            </div>
            Settings
          </h1>
          <p className="text-muted-foreground text-lg">
            Manage your account, integrations, and preferences
          </p>
        </div>

        <div className="border-0 shadow-lg rounded-xl bg-card/50 backdrop-blur-sm">
          <Tabs defaultValue="sites" className="w-full">
            <div className="p-6 pb-0">
              <TabsList className="grid w-full grid-cols-5 bg-muted/50">
                <TabsTrigger value="sites" className="data-[state=active]:bg-background data-[state=active]:shadow-sm">Sites</TabsTrigger>
                <TabsTrigger value="integrations" className="data-[state=active]:bg-background data-[state=active]:shadow-sm">Integrations</TabsTrigger>
                <TabsTrigger value="notifications" className="data-[state=active]:bg-background data-[state=active]:shadow-sm">Notifications</TabsTrigger>
                <TabsTrigger value="api" className="data-[state=active]:bg-background data-[state=active]:shadow-sm">API & Webhooks</TabsTrigger>
                <TabsTrigger value="billing" className="data-[state=active]:bg-background data-[state=active]:shadow-sm">Billing</TabsTrigger>
              </TabsList>
            </div>

            <TabsContent value="sites" className="p-6 pt-6 space-y-6">
                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <Globe className="h-5 w-5 text-primary" />
                        Managed Sites
                      </CardTitle>
                      <CardDescription>
                        View and manage the websites AICO is monitoring for conversion optimization
                      </CardDescription>
                    </CardHeader>
                    <CardContent>
                      <div className="border rounded-lg overflow-hidden bg-background/50">
                        <Table>
                          <TableHeader>
                            <TableRow className="bg-muted/50">
                              <TableHead className="font-semibold">Name</TableHead>
                              <TableHead className="font-semibold">URL</TableHead>
                              <TableHead className="font-semibold">Status</TableHead>
                              <TableHead className="font-semibold">Conversion Rate</TableHead>
                              <TableHead className="font-semibold">Last Analyzed</TableHead>
                              <TableHead></TableHead>
                            </TableRow>
                          </TableHeader>
                          <TableBody>
                            {sites.length > 0 ? (
                              sites.map((site) => (
                                <TableRow key={site.id} className="hover:bg-muted/30 transition-colors">
                                  <TableCell className="font-medium">{site.name}</TableCell>
                                  <TableCell className="text-muted-foreground">{site.url}</TableCell>
                                  <TableCell>
                                    <Badge variant={site.status === "improving" ? "default" : site.status === "declining" ? "destructive" : "secondary"} className="font-medium">
                                      {site.status}
                                    </Badge>
                                  </TableCell>
                                  <TableCell className="font-medium">{site.conversionRate}%</TableCell>
                                  <TableCell className="text-muted-foreground">{site.lastAnalyzed}</TableCell>
                                  <TableCell>
                                    <Button variant="ghost" size="icon" onClick={() => deleteSite(site.id)} className="hover:bg-destructive/10 hover:text-destructive">
                                      <Trash2 className="h-4 w-4" />
                                    </Button>
                                  </TableCell>
                                </TableRow>
                              ))
                            ) : (
                              <TableRow>
                                <TableCell colSpan={6} className="text-center py-12">
                                  <div className="space-y-2">
                                    <Globe className="h-12 w-12 text-muted-foreground mx-auto" />
                                    <p className="text-muted-foreground">No sites added yet. Add your first site below.</p>
                                  </div>
                                </TableCell>
                              </TableRow>
                            )}
                          </TableBody>
                        </Table>
                      </div>
                    </CardContent>
                  </Card>

                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <Plus className="h-5 w-5 text-primary" />
                        Add New Site
                      </CardTitle>
                      <CardDescription>
                        Add a website to monitor for conversion optimization opportunities
                      </CardDescription>
                    </CardHeader>
                    <CardContent>
                      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                          <div className="space-y-2">
                            <Label htmlFor="siteName" className="text-sm font-medium">Site Name</Label>
                            <Input 
                              id="siteName" 
                              placeholder="My E-commerce Store" 
                              className="bg-background/50 border-muted-foreground/20"
                              {...register("siteName", { required: "Site name is required" })}
                            />
                            {errors.siteName && (
                              <p className="text-sm text-destructive">{errors.siteName.message}</p>
                            )}
                          </div>
                          <div className="space-y-2">
                            <Label htmlFor="siteUrl" className="text-sm font-medium">Site URL</Label>
                            <Input 
                              id="siteUrl" 
                              placeholder="https://example.com" 
                              className="bg-background/50 border-muted-foreground/20"
                              {...register("siteUrl", { 
                                required: "Site URL is required",
                                pattern: {
                                  value: /^(https?:\/\/)?([\da-z.-]+)\.([a-z.]{2,6})([/\w .-]*)*\/?$/,
                                  message: "Please enter a valid URL"
                                }
                              })}
                            />
                            {errors.siteUrl && (
                              <p className="text-sm text-destructive">{errors.siteUrl.message}</p>
                            )}
                          </div>
                        </div>
                        <Button type="submit" className="flex items-center gap-2 bg-gradient-to-r from-primary to-primary/80 hover:from-primary/90 hover:to-primary/70">
                          <Plus className="h-4 w-4" /> Add Site
                        </Button>
                      </form>
                    </CardContent>
                  </Card>
                </TabsContent>

                <TabsContent value="integrations" className="p-6 pt-6 space-y-6">
                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <BarChart3 className="h-5 w-5 text-primary" />
                        Data Integrations
                      </CardTitle>
                      <CardDescription>
                        Connect your analytics and e-commerce platforms to enable accurate conversion tracking
                      </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-4">
                      {integrations.map((integration) => (
                        <div key={integration.name} className="flex items-center justify-between p-4 border rounded-lg bg-background/30 hover:bg-background/50 transition-colors">
                          <div className="flex items-center space-x-4">
                            <div className="p-3 bg-primary/10 rounded-lg">
                              <integration.icon className="w-6 h-6 text-primary" />
                            </div>
                            <div>
                              <h3 className="font-semibold">{integration.name}</h3>
                              <p className="text-sm text-muted-foreground">{integration.description}</p>
                              {integration.lastSync && (
                                <p className="text-xs text-muted-foreground mt-1">
                                  Last sync: {integration.lastSync}
                                </p>
                              )}
                            </div>
                          </div>
                          <div className="flex items-center space-x-3">
                            <Badge variant={integration.status === "connected" ? "default" : "secondary"} className="font-medium">
                              {integration.status}
                            </Badge>
                            <Button variant="outline" size="sm" className="bg-background/50">
                              {integration.status === "connected" ? "Configure" : "Connect"}
                            </Button>
                          </div>
                        </div>
                      ))}
                    </CardContent>
                  </Card>

                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <Shield className="h-5 w-5 text-primary" />
                        Connection Test
                      </CardTitle>
                      <CardDescription>
                        Test your integrations to ensure data is flowing correctly
                      </CardDescription>
                    </CardHeader>
                    <CardContent>
                      <Button className="bg-gradient-to-r from-primary to-primary/80 hover:from-primary/90 hover:to-primary/70">Run Connection Test</Button>
                    </CardContent>
                  </Card>
                </TabsContent>

                <TabsContent value="notifications" className="p-6 pt-6 space-y-6">
                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <Mail className="h-5 w-5 text-primary" />
                        Email Notifications
                      </CardTitle>
                      <CardDescription>
                        Configure when you want to receive email updates
                      </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-6">
                      <div className="flex items-center justify-between p-4 border rounded-lg bg-background/30">
                        <div>
                          <h4 className="font-medium">Weekly CRO Reports</h4>
                          <p className="text-sm text-muted-foreground">
                            Summary of conversion improvements and recommendations
                          </p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <div className="flex items-center justify-between p-4 border rounded-lg bg-background/30">
                        <div>
                          <h4 className="font-medium">Critical Issues</h4>
                          <p className="text-sm text-muted-foreground">
                            Immediate alerts for significant conversion drops
                          </p>
                        </div>
                        <Switch defaultChecked />
                      </div>
                      <div className="flex items-center justify-between p-4 border rounded-lg bg-background/30">
                        <div>
                          <h4 className="font-medium">New Recommendations</h4>
                          <p className="text-sm text-muted-foreground">
                            When AI identifies new optimization opportunities
                          </p>
                        </div>
                        <Switch />
                      </div>
                    </CardContent>
                  </Card>

                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <Bell className="h-5 w-5 text-primary" />
                        Push Notifications
                      </CardTitle>
                      <CardDescription>
                        Real-time browser notifications for urgent updates
                      </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-4">
                      <div className="flex items-center justify-between p-4 border rounded-lg bg-background/30">
                        <div>
                          <h4 className="font-medium">Conversion Anomalies</h4>
                          <p className="text-sm text-muted-foreground">
                            Unusual patterns in conversion data
                          </p>
                        </div>
                        <Switch />
                      </div>
                    </CardContent>
                  </Card>
                </TabsContent>

                <TabsContent value="api" className="p-6 pt-6 space-y-6">
                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <Key className="h-5 w-5 text-primary" />
                        API Keys
                      </CardTitle>
                      <CardDescription>
                        Manage API keys for programmatic access to AICO
                      </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-4">
                      <div className="flex items-center justify-between p-4 border rounded-lg bg-background/30">
                        <div className="flex items-center space-x-3">
                          <Key className="w-5 h-5 text-primary" />
                          <div>
                            <h4 className="font-medium">Production API Key</h4>
                            <p className="text-sm text-muted-foreground font-mono">aico_prod_********************************</p>
                          </div>
                        </div>
                        <Button variant="outline" size="sm" className="bg-background/50">Regenerate</Button>
                      </div>
                      <Button className="bg-gradient-to-r from-primary to-primary/80 hover:from-primary/90 hover:to-primary/70">Create New API Key</Button>
                    </CardContent>
                  </Card>

                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <Webhook className="h-5 w-5 text-primary" />
                        Webhooks
                      </CardTitle>
                      <CardDescription>
                        Configure webhooks to receive real-time updates
                      </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-4">
                      <div className="space-y-2">
                        <Label htmlFor="webhook-url" className="text-sm font-medium">Webhook URL</Label>
                        <Input
                          id="webhook-url"
                          placeholder="https://your-app.com/webhooks/aico"
                          className="bg-background/50 border-muted-foreground/20"
                        />
                      </div>
                      <div className="flex items-center space-x-2 text-sm text-muted-foreground">
                        <Webhook className="w-4 h-4" />
                        <span>Events: Recommendations, Metric Updates, Alerts</span>
                      </div>
                      <Button className="bg-gradient-to-r from-primary to-primary/80 hover:from-primary/90 hover:to-primary/70">Save Webhook</Button>
                    </CardContent>
                  </Card>
                </TabsContent>

                <TabsContent value="billing" className="p-6 pt-6 space-y-6">
                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <CreditCard className="h-5 w-5 text-primary" />
                        Choose Your Plan
                      </CardTitle>
                      <CardDescription>
                        Select the plan that best fits your conversion optimization needs
                      </CardDescription>
                    </CardHeader>
                    <CardContent>
                      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                        {plans.map((plan) => (
                          <div 
                            key={plan.name}
                            className={`relative p-6 border rounded-xl transition-all flex flex-col bg-gradient-to-br from-background to-background/80 ${
                              currentPlan === plan.name 
                                ? "border-2 border-primary bg-primary/5" 
                                : "border hover:border-primary/30 hover:bg-background/60"
                            }`}
                          >
                            <div className="flex items-center justify-between mb-4">
                              <h3 className="font-semibold text-lg">{plan.name}</h3>
                              {currentPlan === plan.name && (
                                <Badge className="bg-primary text-primary-foreground">Current</Badge>
                              )}
                            </div>
                            
                            <p className="text-3xl font-bold mb-4">
                              ${plan.price}
                              <span className="text-sm font-normal text-muted-foreground">/month</span>
                            </p>
                            
                            <ul className="space-y-3 mb-6 flex-1">
                              {plan.features.map((feature, index) => (
                                <li key={index} className="flex items-center text-sm">
                                  <Check className="w-4 h-4 text-green-500 mr-2 flex-shrink-0" />
                                  {feature}
                                </li>
                              ))}
                            </ul>
                            
                            <Button 
                              className={`w-full mt-auto ${
                                currentPlan === plan.name 
                                  ? "bg-muted text-muted-foreground" 
                                  : "bg-gradient-to-r from-primary to-primary/80 hover:from-primary/90 hover:to-primary/70"
                              }`}
                              disabled={currentPlan === plan.name || isChangingPlan}
                              onClick={() => handlePlanChange(plan.name)}
                            >
                              {isChangingPlan && selectedPlan === plan.name ? (
                                "Changing Plan..."
                              ) : currentPlan === plan.name ? (
                                "Current Plan"
                              ) : (
                                "Select Plan"
                              )}
                            </Button>
                          </div>
                        ))}
                      </div>
                    </CardContent>
                  </Card>

                  <Card className="border-0 shadow-md bg-gradient-to-br from-card to-card/80">
                    <CardHeader>
                      <CardTitle className="flex items-center gap-2">
                        <CreditCard className="h-5 w-5 text-primary" />
                        Billing History
                      </CardTitle>
                      <CardDescription>
                        Download invoices and view payment history
                      </CardDescription>
                    </CardHeader>
                    <CardContent>
                      <div className="space-y-3">
                        <div className="flex items-center justify-between p-4 border rounded-lg bg-background/30">
                          <div>
                            <p className="font-medium">Dec 2024 - {currentPlan} Plan</p>
                            <p className="text-sm text-muted-foreground">Paid on Dec 1, 2024</p>
                          </div>
                          <div className="flex items-center space-x-3">
                            <span className="font-medium">
                              ${plans.find(p => p.name === currentPlan)?.price}.00
                            </span>
                            <Button variant="outline" size="sm" className="bg-background/50">Download</Button>
                          </div>
                        </div>
                        <div className="flex items-center justify-between p-4 border rounded-lg bg-background/30">
                          <div>
                            <p className="font-medium">Nov 2024 - {currentPlan} Plan</p>
                            <p className="text-sm text-muted-foreground">Paid on Nov 1, 2024</p>
                          </div>
                          <div className="flex items-center space-x-3">
                            <span className="font-medium">
                              ${plans.find(p => p.name === currentPlan)?.price}.00
                            </span>
                            <Button variant="outline" size="sm" className="bg-background/50">Download</Button>
                          </div>
                        </div>
                      </div>
                    </CardContent>
                  </Card>
                </TabsContent>
              </Tabs>
        </div>
      </div>
    </main>
  );
}
