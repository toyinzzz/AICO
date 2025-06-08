import { useState } from "react";
import { useParams } from "react-router-dom";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Checkbox } from "@/components/ui/checkbox";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, ResponsiveContainer, BarChart, Bar, PieChart, Pie, Cell } from "recharts";
import { BeforeAfterComparison } from "@/components/Analysis/BeforeAfterComparison";
import { useToast } from "@/hooks/use-toast";
import { generateAnalysisReport, generatePdfReport } from "@/utils/reportGenerator";
import {
  Search,
  Globe,
  Zap,
  TrendingUp,
  CheckCircle,
  Clock,
  AlertTriangle,
  Target,
  Users,
  MousePointer,
  BarChart3,
  PieChart as PieChartIcon,
  Activity,
  Download
} from "lucide-react";

export default function Analysis() {
  const { siteId } = useParams();
  const [url, setUrl] = useState("");
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [activeTab, setActiveTab] = useState("all");
  const { toast } = useToast();

  // Chart data
  const conversionData = [
    { month: 'Jan', rate: 1.2, visitors: 8500, conversions: 102 },
    { month: 'Feb', rate: 1.4, visitors: 9200, conversions: 129 },
    { month: 'Mar', rate: 1.6, visitors: 10500, conversions: 168 },
    { month: 'Apr', rate: 1.8, visitors: 11800, conversions: 212 },
    { month: 'May', rate: 1.9, visitors: 12500, conversions: 238 },
    { month: 'Jun', rate: 2.1, visitors: 14500, conversions: 305 },
  ];

  const deviceData = [
    { name: 'Desktop', value: 45, color: '#3b82f6' },
    { name: 'Mobile', value: 35, color: '#10b981' },
    { name: 'Tablet', value: 20, color: '#f59e0b' },
  ];

  const revenueData = [
    { month: 'Jan', revenue: 85000, costs: 32000 },
    { month: 'Feb', revenue: 92000, costs: 35000 },
    { month: 'Mar', revenue: 105000, costs: 38000 },
    { month: 'Apr', revenue: 118000, costs: 42000 },
    { month: 'May', revenue: 125000, costs: 45000 },
    { month: 'Jun', revenue: 145000, costs: 48000 },
  ];

  // Recommendations state
  const [recommendations, setRecommendations] = useState([
    {
      id: "1",
      title: "Add trust badges and security certificates",
      description: "Display SSL certificates, payment security badges, and customer testimonials near the checkout area to increase trust and reduce abandonment.",
      impact: "High",
      expectedUplift: "+15%",
      difficulty: "Easy",
      estimatedTime: "2 hours",
      category: "Trust & Security",
      implemented: false,
      details: [
        "Add SSL certificate badge",
        "Display payment security icons", 
        "Include customer testimonials",
        "Show money-back guarantee"
      ]
    },
    {
      id: "2",
      title: "Optimize mobile checkout flow",
      description: "Simplify the mobile checkout process by reducing steps, improving form design, and adding mobile-specific payment options.",
      impact: "High", 
      expectedUplift: "+22%",
      difficulty: "Medium",
      estimatedTime: "1 week",
      category: "Mobile UX",
      implemented: false,
      details: [
        "Reduce checkout steps from 4 to 2",
        "Implement autofill for forms",
        "Add Apple Pay and Google Pay",
        "Optimize button sizes for mobile"
      ]
    },
    {
      id: "3",
      title: "A/B test call-to-action button variations",
      description: "Test different CTA button colors, sizes, and text to find the highest converting combination.",
      impact: "Medium",
      expectedUplift: "+8%", 
      difficulty: "Easy",
      estimatedTime: "3 hours",
      category: "CTA Optimization",
      implemented: true,
      details: [
        "Test 3 button colors: blue, green, orange",
        "Try different button text variations",
        "Test button placement",
        "Measure click-through rates"
      ]
    },
    {
      id: "4",
      title: "Reduce contact form fields",
      description: "Minimize form friction by removing non-essential fields and implementing progressive disclosure.",
      impact: "Medium",
      expectedUplift: "+12%",
      difficulty: "Easy", 
      estimatedTime: "1 hour",
      category: "Form Optimization",
      implemented: false,
      details: [
        "Remove optional fields",
        "Use smart defaults",
        "Implement field validation",
        "Add progress indicators"
      ]
    }
  ]);

  // Site data
  const siteData = {
    name: "E-commerce Store",
    url: "shop.example.com",
    score: 72,
    lastAnalyzed: "2 hours ago",
    metrics: {
      conversionRate: 2.4,
      bounceRate: 45,
      avgSessionDuration: "3:24",
      pageViews: 15420
    }
  };

  // Handle analyze function
  const handleAnalyze = async () => {
    if (!url.trim()) {
      toast({
        title: "URL Required",
        description: "Please enter a valid URL to analyze.",
        variant: "destructive",
      });
      return;
    }

    // Basic URL validation
    try {
      new URL(url.startsWith('http') ? url : `https://${url}`);
    } catch {
      toast({
        title: "Invalid URL",
        description: "Please enter a valid URL (e.g., https://example.com).",
        variant: "destructive",
      });
      return;
    }

    setIsAnalyzing(true);
    
    // Simulate API call
    setTimeout(() => {
      setIsAnalyzing(false);
      toast({
        title: "Analysis Complete",
        description: `Successfully analyzed ${url}`,
      });
    }, 3000);
  };

  // Handle mark as done function
  const handleMarkAsDone = (recommendationId: string) => {
    setRecommendations(prev => 
      prev.map(rec => 
        rec.id === recommendationId 
          ? { ...rec, implemented: true }
          : rec
      )
    );
    
    toast({
      title: "Recommendation Completed",
      description: "Marked recommendation as implemented.",
    });
  };

  // Handle toggle recommendation function
  const handleToggleRecommendation = (recommendationId: string) => {
    setRecommendations(prev => 
      prev.map(rec => 
        rec.id === recommendationId 
          ? { ...rec, implemented: !rec.implemented }
          : rec
      )
    );
  };

  // Get filtered recommendations function
  const getFilteredRecommendations = () => {
    switch (activeTab) {
      case "pending":
        return recommendations.filter(r => !r.implemented);
      case "implemented":
        return recommendations.filter(r => r.implemented);
      case "high":
        return recommendations.filter(r => r.impact === "High");
      default:
        return recommendations;
    }
  };

  const exportAnalysisReport = () => {
    console.log("Exporting analysis PDF report...");
    
    try {
      const analysisData = {
        name: siteData.name,
        url: siteData.url,
        score: siteData.score,
        metrics: siteData.metrics,
        recommendations: recommendations
      };

      generatePdfReport(analysisData, 'analysis');
      
      toast({
        title: "PDF Report Downloaded",
        description: "Comprehensive website analysis report saved as PDF",
      });
    } catch (error) {
      console.error("Failed to generate PDF report:", error);
      toast({
        title: "Export Failed",
        description: "Unable to generate PDF report. Please try again.",
        variant: "destructive",
      });
    }
  };

  return (
    <div className="flex-1 bg-gradient-to-br from-background via-background to-muted/20">
      <main className="flex-1 overflow-y-auto p-8">
        <div className="space-y-8 max-w-7xl mx-auto">
          {/* Header with Export Button */}
          <div className="flex items-center justify-between">
            <div className="space-y-2">
              <h1 className="text-4xl font-bold bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent">
                Website Analysis
              </h1>
              <p className="text-muted-foreground text-lg">
                AI-powered conversion rate optimization analysis and recommendations
              </p>
            </div>
            <Button onClick={exportAnalysisReport} className="flex items-center space-x-2 shadow-lg hover:shadow-xl transition-shadow">
              <Download className="w-4 h-4" />
              <span>Export PDF Report</span>
            </Button>
          </div>

          {/* URL Analysis Section */}
          <Card className="border-0 shadow-lg hover:shadow-xl transition-shadow bg-card/50 backdrop-blur-sm">
            <CardHeader>
              <CardTitle className="flex items-center space-x-2 text-xl">
                <div className="p-2 bg-primary/20 rounded-xl">
                  <Search className="w-5 h-5 text-primary" />
                </div>
                <span>Website Analysis</span>
              </CardTitle>
              <CardDescription className="text-base">
                Enter a URL to get AI-powered conversion optimization recommendations
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="flex space-x-4">
                <div className="flex-1">
                  <Input
                    placeholder="https://example.com"
                    value={url}
                    onChange={(e) => setUrl(e.target.value)}
                    onKeyDown={(e) => e.key === 'Enter' && handleAnalyze()}
                    className="h-12"
                  />
                </div>
                <Button onClick={handleAnalyze} disabled={isAnalyzing || !url.trim()} className="h-12 px-6 shadow-md hover:shadow-lg transition-shadow">
                  {isAnalyzing ? (
                    <>
                      <Zap className="w-4 h-4 mr-2 animate-spin" />
                      Analyzing...
                    </>
                  ) : (
                    <>
                      <Globe className="w-4 h-4 mr-2" />
                      Analyze
                    </>
                  )}
                </Button>
              </div>
              {isAnalyzing && (
                <div className="mt-4 space-y-2">
                  <Progress value={33} className="h-2" />
                  <p className="text-sm text-muted-foreground">
                    Crawling website and analyzing conversion potential...
                  </p>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Site Overview */}
          <Card className="border-0 shadow-lg hover:shadow-xl transition-shadow bg-card/50 backdrop-blur-sm">
            <CardHeader>
              <div className="flex items-center justify-between">
                <div>
                  <CardTitle className="text-xl">{siteData.name}</CardTitle>
                  <CardDescription className="text-base">{siteData.url}</CardDescription>
                </div>
                <Badge variant="outline" className="text-sm">Last analyzed: {siteData.lastAnalyzed}</Badge>
              </div>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-6">
                <div className="lg:col-span-1">
                  <div className="text-center p-6 border rounded-xl bg-gradient-to-br from-primary/5 to-primary/10">
                    <div className="text-3xl font-bold mb-2 text-primary">{siteData.score}</div>
                    <Progress value={siteData.score} className="h-2 mb-2" />
                    <p className="text-sm text-muted-foreground font-medium">Optimization Score</p>
                  </div>
                </div>
                <div className="lg:col-span-4 grid grid-cols-2 lg:grid-cols-4 gap-4">
                  <div className="text-center p-6 border rounded-xl bg-gradient-to-br from-blue-50 to-blue-100 dark:from-blue-950/20 dark:to-blue-900/20 hover:shadow-md transition-shadow">
                    <MousePointer className="w-6 h-6 mx-auto mb-2 text-blue-500" />
                    <div className="text-lg font-semibold">{siteData.metrics.conversionRate}%</div>
                    <p className="text-sm text-muted-foreground">Conversion Rate</p>
                  </div>
                  <div className="text-center p-6 border rounded-xl bg-gradient-to-br from-green-50 to-green-100 dark:from-green-950/20 dark:to-green-900/20 hover:shadow-md transition-shadow">
                    <TrendingUp className="w-6 h-6 mx-auto mb-2 text-green-500" />
                    <div className="text-lg font-semibold">{siteData.metrics.bounceRate}%</div>
                    <p className="text-sm text-muted-foreground">Bounce Rate</p>
                  </div>
                  <div className="text-center p-6 border rounded-xl bg-gradient-to-br from-purple-50 to-purple-100 dark:from-purple-950/20 dark:to-purple-900/20 hover:shadow-md transition-shadow">
                    <Clock className="w-6 h-6 mx-auto mb-2 text-purple-500" />
                    <div className="text-lg font-semibold">{siteData.metrics.avgSessionDuration}</div>
                    <p className="text-sm text-muted-foreground">Avg. Session</p>
                  </div>
                  <div className="text-center p-6 border rounded-xl bg-gradient-to-br from-orange-50 to-orange-100 dark:from-orange-950/20 dark:to-orange-900/20 hover:shadow-md transition-shadow">
                    <Users className="w-6 h-6 mx-auto mb-2 text-orange-500" />
                    <div className="text-lg font-semibold">{siteData.metrics.pageViews.toLocaleString()}</div>
                    <p className="text-sm text-muted-foreground">Page Views</p>
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>

          {/* Before vs After Comparison */}
          <div className="border-0 shadow-lg rounded-xl bg-card/50 backdrop-blur-sm">
            <BeforeAfterComparison />
          </div>

          {/* Analytics Charts */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <Card className="border-0 shadow-lg hover:shadow-xl transition-shadow bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <CardTitle className="flex items-center space-x-2 text-xl">
                  <div className="p-2 bg-primary/20 rounded-xl">
                    <Activity className="w-5 h-5 text-primary" />
                  </div>
                  <span>Conversion Trends</span>
                </CardTitle>
                <CardDescription className="text-base">Monthly conversion rate performance</CardDescription>
              </CardHeader>
              <CardContent>
                <ChartContainer
                  config={{
                    rate: { label: "Conversion Rate", color: "hsl(var(--primary))" },
                    visitors: { label: "Visitors", color: "hsl(var(--muted))" }
                  }}
                  className="h-[300px]"
                >
                  <ResponsiveContainer width="100%" height="100%">
                    <LineChart data={conversionData}>
                      <CartesianGrid strokeDasharray="3 3" />
                      <XAxis dataKey="month" />
                      <YAxis />
                      <ChartTooltip content={<ChartTooltipContent />} />
                      <Line
                        type="monotone"
                        dataKey="rate"
                        stroke="hsl(var(--primary))"
                        strokeWidth={3}
                        dot={{ fill: "hsl(var(--primary))" }}
                      />
                    </LineChart>
                  </ResponsiveContainer>
                </ChartContainer>
              </CardContent>
            </Card>

            <Card className="border-0 shadow-lg hover:shadow-xl transition-shadow bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <CardTitle className="flex items-center space-x-2 text-xl">
                  <div className="p-2 bg-primary/20 rounded-xl">
                    <PieChartIcon className="w-5 h-5 text-primary" />
                  </div>
                  <span>Traffic by Device</span>
                </CardTitle>
                <CardDescription className="text-base">Visitor distribution across devices</CardDescription>
              </CardHeader>
              <CardContent>
                <ChartContainer
                  config={{
                    desktop: { label: "Desktop", color: "#3b82f6" },
                    mobile: { label: "Mobile", color: "#10b981" },
                    tablet: { label: "Tablet", color: "#f59e0b" }
                  }}
                  className="h-[300px]"
                >
                  <ResponsiveContainer width="100%" height="100%">
                    <PieChart>
                      <Pie
                        data={deviceData}
                        cx="50%"
                        cy="50%"
                        outerRadius={80}
                        dataKey="value"
                        label={({ name, value }) => `${name}: ${value}%`}
                      >
                        {deviceData.map((entry, index) => (
                          <Cell key={`cell-${index}`} fill={entry.color} />
                        ))}
                      </Pie>
                      <ChartTooltip content={<ChartTooltipContent />} />
                    </PieChart>
                  </ResponsiveContainer>
                </ChartContainer>
              </CardContent>
            </Card>
          </div>

          <Card className="border-0 shadow-lg hover:shadow-xl transition-shadow bg-card/50 backdrop-blur-sm">
            <CardHeader>
              <CardTitle className="flex items-center space-x-2 text-xl">
                <div className="p-2 bg-primary/20 rounded-xl">
                  <BarChart3 className="w-5 h-5 text-primary" />
                </div>
                <span>Revenue Analytics</span>
              </CardTitle>
              <CardDescription className="text-base">Monthly revenue vs costs comparison</CardDescription>
            </CardHeader>
            <CardContent>
              <ChartContainer
                config={{
                  revenue: { label: "Revenue", color: "hsl(var(--primary))" },
                  costs: { label: "Costs", color: "hsl(var(--destructive))" }
                }}
                className="h-[400px]"
              >
                <ResponsiveContainer width="100%" height="100%">
                  <BarChart data={revenueData}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="month" />
                    <YAxis />
                    <ChartTooltip content={<ChartTooltipContent />} />
                    <Bar dataKey="revenue" fill="hsl(var(--primary))" radius={[4, 4, 0, 0]} />
                    <Bar dataKey="costs" fill="hsl(var(--destructive))" radius={[4, 4, 0, 0]} />
                  </BarChart>
                </ResponsiveContainer>
              </ChartContainer>
            </CardContent>
          </Card>

          {/* Recommendations */}
          <Card className="border-0 shadow-lg hover:shadow-xl transition-shadow bg-card/50 backdrop-blur-sm">
            <CardHeader>
              <CardTitle className="text-xl">AI Recommendations</CardTitle>
              <CardDescription className="text-base">
                Prioritized list of conversion optimization opportunities
              </CardDescription>
            </CardHeader>
            <CardContent>
              <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
                <TabsList className="grid w-full grid-cols-4">
                  <TabsTrigger value="all">All ({recommendations.length})</TabsTrigger>
                  <TabsTrigger value="pending">Pending ({recommendations.filter(r => !r.implemented).length})</TabsTrigger>
                  <TabsTrigger value="implemented">Done ({recommendations.filter(r => r.implemented).length})</TabsTrigger>
                  <TabsTrigger value="high">High Impact</TabsTrigger>
                </TabsList>
                
                <TabsContent value={activeTab} className="mt-6">
                  <div className="space-y-4">
                    {getFilteredRecommendations().map((rec) => (
                      <Card key={rec.id} className={`transition-all hover:shadow-lg border-0 shadow-md ${rec.implemented ? 'bg-green-50/50 dark:bg-green-950/20' : 'bg-card/50'} backdrop-blur-sm`}>
                        <CardContent className="p-6">
                          <div className="flex items-start justify-between mb-4">
                            <div className="flex items-start space-x-3">
                              <Checkbox
                                checked={rec.implemented}
                                onCheckedChange={() => handleToggleRecommendation(rec.id)}
                                className="mt-1"
                              />
                              <div className="flex-1">
                                <h3 className="font-semibold text-lg mb-2">{rec.title}</h3>
                                <p className="text-muted-foreground mb-3">{rec.description}</p>
                                
                                <div className="flex flex-wrap gap-2 mb-3">
                                  <Badge variant={rec.impact === "High" ? "destructive" : "default"}>
                                    {rec.impact} Impact
                                  </Badge>
                                  <Badge variant="outline">{rec.expectedUplift}</Badge>
                                  <Badge variant="secondary">{rec.difficulty}</Badge>
                                  <Badge variant="outline">{rec.estimatedTime}</Badge>
                                </div>

                                <details className="mt-3">
                                  <summary className="cursor-pointer text-sm font-medium text-primary hover:text-primary/80 transition-colors">
                                    View Implementation Details
                                  </summary>
                                  <ul className="mt-2 ml-4 space-y-1">
                                    {rec.details.map((detail, index) => (
                                      <li key={index} className="text-sm text-muted-foreground">
                                        • {detail}
                                      </li>
                                    ))}
                                  </ul>
                                </details>
                              </div>
                            </div>
                            
                            <div className="flex items-center space-x-2">
                              {rec.implemented ? (
                                <CheckCircle className="w-5 h-5 text-green-500" />
                              ) : (
                                <Button 
                                  size="sm" 
                                  onClick={() => handleMarkAsDone(rec.id)}
                                  className="shadow-md hover:shadow-lg transition-shadow"
                                >
                                  Mark as Done
                                </Button>
                              )}
                            </div>
                          </div>
                        </CardContent>
                      </Card>
                    ))}
                  </div>
                </TabsContent>
              </Tabs>
            </CardContent>
          </Card>
        </div>
      </main>
    </div>
  );
}
