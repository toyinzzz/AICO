import { useState, useEffect } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { MoneyLostChart } from "@/components/Dashboard/MoneyLostChart";
import { ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart";
import { LineChart, Line, XAxis, YAxis, ResponsiveContainer, AreaChart, Area, BarChart, Bar } from "recharts";
import { useNavigate } from "react-router-dom";
import {
  TrendingUp,
  TrendingDown,
  Users,
  MousePointer,
  DollarSign,
  Target,
  Zap,
  AlertTriangle,
  CheckCircle,
  Clock,
  BarChart3,
  Activity,
  Menu
} from "lucide-react";

export default function Dashboard() {
  const [timeRange, setTimeRange] = useState("7d");
  const navigate = useNavigate();

  // Modern business metrics
  const metrics = {
    conversionRate: 3.24,
    conversionChange: 12.5,
    totalVisitors: 24867,
    visitorsChange: 8.3,
    revenue: 125430,
    revenueChange: 15.7,
    engagement: 4.2,
    engagementChange: 8.9
  };

  // User journey funnel data
  const funnelData = [
    { stage: "Visitors", count: 24867, rate: 100 },
    { stage: "Engaged Users", count: 12434, rate: 50.0 },
    { stage: "Interested", count: 3730, rate: 15.0 },
    { stage: "Intent", count: 1243, rate: 5.0 },
    { stage: "Conversion", count: 806, rate: 3.24 }
  ];

  // Performance trends over time
  const performanceData = [
    { date: "Jan", conversionRate: 2.8, revenue: 98000, visitors: 18500 },
    { date: "Feb", conversionRate: 2.9, revenue: 105000, visitors: 19200 },
    { date: "Mar", conversionRate: 3.1, revenue: 112000, visitors: 20100 },
    { date: "Apr", conversionRate: 3.0, revenue: 108000, visitors: 19800 },
    { date: "May", conversionRate: 3.2, revenue: 118000, visitors: 21200 },
    { date: "Jun", conversionRate: 3.24, revenue: 125430, visitors: 24867 }
  ];

  // Recent optimization tests
  const optimizationTests = [
    {
      id: "1",
      name: "Landing Page Headlines",
      status: "running",
      uplift: 8.3,
      confidence: 85,
      variant: "Benefit vs Feature"
    },
    {
      id: "2", 
      name: "CTA Button Placement",
      status: "completed",
      uplift: 12.7,
      confidence: 95,
      variant: "Above vs Below Fold"
    },
    {
      id: "3",
      name: "Social Proof Display",
      status: "completed",
      uplift: 15.2,
      confidence: 98,
      variant: "Testimonials vs Stats"
    }
  ];

  // AI-powered recommendations
  const recommendations = [
    {
      id: "1",
      title: "Optimize user onboarding flow",
      impact: "High",
      expectedUplift: "+18%",
      effort: "Medium",
      category: "User Experience"
    },
    {
      id: "2",
      title: "Implement progressive disclosure",
      impact: "High", 
      expectedUplift: "+12%",
      effort: "Low",
      category: "Interface Design"
    },
    {
      id: "3",
      title: "Add micro-interactions",
      impact: "Medium",
      expectedUplift: "+8%",
      effort: "Low", 
      category: "Engagement"
    },
    {
      id: "4",
      title: "Personalize content recommendations",
      impact: "Medium",
      expectedUplift: "+6%",
      effort: "Medium",
      category: "Personalization"
    }
  ];

  const getStatusIcon = (status: string) => {
    switch (status) {
      case "completed":
        return <CheckCircle className="w-4 h-4 text-green-500" />;
      case "running":
        return <Clock className="w-4 h-4 text-blue-500" />;
      default:
        return <AlertTriangle className="w-4 h-4 text-yellow-500" />;
    }
  };

  const getImpactColor = (impact: string) => {
    switch (impact) {
      case "High":
        return "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200";
      case "Medium":
        return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200";
      case "Low":
        return "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200";
      default:
        return "bg-gray-100 text-gray-800 dark:bg-gray-800 dark:text-gray-200";
    }
  };

  const handleViewAllRecommendations = () => {
    navigate("/analysis");
  };

  return (
    <div className="flex-1 bg-gradient-to-br from-background via-background to-muted/20">
      <main className="flex-1 overflow-y-auto p-8">
        <div className="space-y-8 max-w-7xl mx-auto">
          {/* Modern Header */}
          <div className="flex items-center justify-between">
            <div className="space-y-2">
              <h1 className="text-4xl font-bold bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent">
                Analytics Dashboard
              </h1>
              <p className="text-muted-foreground text-lg">
                Conversion optimization insights for {new Date().toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' })}
              </p>
            </div>
            <div className="flex items-center space-x-3">
              <Button 
                variant={timeRange === "7d" ? "default" : "outline"} 
                size="sm"
                onClick={() => setTimeRange("7d")}
                className="rounded-full"
              >
                7 Days
              </Button>
              <Button 
                variant={timeRange === "30d" ? "default" : "outline"} 
                size="sm"
                onClick={() => setTimeRange("30d")}
                className="rounded-full"
              >
                30 Days
              </Button>
              <Button 
                variant={timeRange === "90d" ? "default" : "outline"} 
                size="sm"
                onClick={() => setTimeRange("90d")}
                className="rounded-full"
              >
                90 Days
              </Button>
            </div>
          </div>

          {/* Revenue Loss Analytics */}
          <div className="grid grid-cols-1 gap-8">
            <MoneyLostChart />
          </div>

          {/* Modern Key Metrics */}
          <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
            <Card className="relative overflow-hidden border-0 shadow-lg bg-gradient-to-br from-primary/5 to-primary/10 hover:shadow-xl transition-all duration-300">
              <div className="absolute inset-0 bg-gradient-to-br from-primary/10 to-transparent opacity-50" />
              <CardHeader className="relative flex flex-row items-center justify-between space-y-0 pb-3">
                <div className="space-y-1">
                  <CardTitle className="text-sm font-medium text-muted-foreground">Conversion Rate</CardTitle>
                  <div className="text-3xl font-bold">{metrics.conversionRate}%</div>
                  <div className="flex items-center text-xs text-green-600">
                    <TrendingUp className="w-3 h-3 mr-1" />
                    +{metrics.conversionChange}% vs last period
                  </div>
                </div>
                <div className="p-3 bg-primary/20 rounded-xl">
                  <Target className="h-6 w-6 text-primary" />
                </div>
              </CardHeader>
            </Card>

            <Card className="relative overflow-hidden border-0 shadow-lg bg-gradient-to-br from-blue-500/5 to-blue-500/10 hover:shadow-xl transition-all duration-300">
              <div className="absolute inset-0 bg-gradient-to-br from-blue-500/10 to-transparent opacity-50" />
              <CardHeader className="relative flex flex-row items-center justify-between space-y-0 pb-3">
                <div className="space-y-1">
                  <CardTitle className="text-sm font-medium text-muted-foreground">Total Visitors</CardTitle>
                  <div className="text-3xl font-bold">{metrics.totalVisitors.toLocaleString()}</div>
                  <div className="flex items-center text-xs text-green-600">
                    <TrendingUp className="w-3 h-3 mr-1" />
                    +{metrics.visitorsChange}% vs last period
                  </div>
                </div>
                <div className="p-3 bg-blue-500/20 rounded-xl">
                  <Users className="h-6 w-6 text-blue-500" />
                </div>
              </CardHeader>
            </Card>

            <Card className="relative overflow-hidden border-0 shadow-lg bg-gradient-to-br from-green-500/5 to-green-500/10 hover:shadow-xl transition-all duration-300">
              <div className="absolute inset-0 bg-gradient-to-br from-green-500/10 to-transparent opacity-50" />
              <CardHeader className="relative flex flex-row items-center justify-between space-y-0 pb-3">
                <div className="space-y-1">
                  <CardTitle className="text-sm font-medium text-muted-foreground">Revenue</CardTitle>
                  <div className="text-3xl font-bold">${metrics.revenue.toLocaleString()}</div>
                  <div className="flex items-center text-xs text-green-600">
                    <TrendingUp className="w-3 h-3 mr-1" />
                    +{metrics.revenueChange}% vs last period
                  </div>
                </div>
                <div className="p-3 bg-green-500/20 rounded-xl">
                  <DollarSign className="h-6 w-6 text-green-500" />
                </div>
              </CardHeader>
            </Card>

            <Card className="relative overflow-hidden border-0 shadow-lg bg-gradient-to-br from-purple-500/5 to-purple-500/10 hover:shadow-xl transition-all duration-300">
              <div className="absolute inset-0 bg-gradient-to-br from-purple-500/10 to-transparent opacity-50" />
              <CardHeader className="relative flex flex-row items-center justify-between space-y-0 pb-3">
                <div className="space-y-1">
                  <CardTitle className="text-sm font-medium text-muted-foreground">Engagement Score</CardTitle>
                  <div className="text-3xl font-bold">{metrics.engagement}/5</div>
                  <div className="flex items-center text-xs text-green-600">
                    <TrendingUp className="w-3 h-3 mr-1" />
                    +{metrics.engagementChange}% vs last period
                  </div>
                </div>
                <div className="p-3 bg-purple-500/20 rounded-xl">
                  <Activity className="h-6 w-6 text-purple-500" />
                </div>
              </CardHeader>
            </Card>
          </div>

          {/* Charts Row - Performance Trends and Conversion Funnel */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
            {/* Performance Trends */}
            <Card className="border-0 shadow-lg hover:shadow-xl transition-all duration-300">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-xl">
                  <div className="p-2 bg-primary/20 rounded-lg">
                    <BarChart3 className="h-5 w-5 text-primary" />
                  </div>
                  Performance Trends
                </CardTitle>
                <CardDescription className="text-base">Conversion rate and revenue over time</CardDescription>
              </CardHeader>
              <CardContent>
                <ChartContainer
                  config={{
                    conversionRate: { label: "Conversion Rate", color: "hsl(var(--primary))" },
                    revenue: { label: "Revenue", color: "hsl(var(--secondary))" }
                  }}
                  className="h-[320px]"
                >
                  <ResponsiveContainer width="100%" height="100%">
                    <AreaChart data={performanceData}>
                      <defs>
                        <linearGradient id="conversionGradient" x1="0" y1="0" x2="0" y2="1">
                          <stop offset="5%" stopColor="hsl(var(--primary))" stopOpacity={0.8}/>
                          <stop offset="95%" stopColor="hsl(var(--primary))" stopOpacity={0.1}/>
                        </linearGradient>
                      </defs>
                      <XAxis dataKey="date" />
                      <YAxis />
                      <ChartTooltip content={<ChartTooltipContent />} />
                      <Area
                        type="monotone"
                        dataKey="conversionRate"
                        stroke="hsl(var(--primary))"
                        fillOpacity={1}
                        fill="url(#conversionGradient)"
                        strokeWidth={3}
                      />
                    </AreaChart>
                  </ResponsiveContainer>
                </ChartContainer>
              </CardContent>
            </Card>

            {/* Conversion Funnel */}
            <Card className="border-0 shadow-lg hover:shadow-xl transition-all duration-300">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-xl">
                  <div className="p-2 bg-blue-500/20 rounded-lg">
                    <Zap className="h-5 w-5 text-blue-500" />
                  </div>
                  User Journey Funnel
                </CardTitle>
                <CardDescription className="text-base">User engagement breakdown</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-6">
                  {funnelData.map((stage, index) => (
                    <div key={stage.stage} className="group">
                      <div className="flex items-center justify-between mb-2">
                        <div className="flex items-center space-x-3">
                          <div className="w-3 h-3 rounded-full bg-gradient-to-r from-primary to-primary/60" />
                          <span className="font-semibold text-sm">{stage.stage}</span>
                        </div>
                        <div className="flex items-center space-x-4">
                          <span className="text-sm font-medium">
                            {stage.count.toLocaleString()} ({stage.rate}%)
                          </span>
                        </div>
                      </div>
                      <div className="w-full bg-muted/50 rounded-full h-3 overflow-hidden">
                        <div 
                          className="bg-gradient-to-r from-primary to-primary/60 h-full rounded-full transition-all duration-500 group-hover:from-primary group-hover:to-primary"
                          style={{ width: `${stage.rate}%` }}
                        />
                      </div>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>
          </div>

          {/* Optimization Tests and AI Recommendations */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
            {/* Active Optimization Tests */}
            <Card className="border-0 shadow-lg hover:shadow-xl transition-all duration-300">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-xl">
                  <div className="p-2 bg-orange-500/20 rounded-lg">
                    <Target className="h-5 w-5 text-orange-500" />
                  </div>
                  Optimization Tests
                </CardTitle>
                <CardDescription className="text-base">Current and recent experiments</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  {optimizationTests.map((test) => (
                    <div key={test.id} className="flex items-center justify-between p-4 border rounded-xl hover:bg-muted/30 transition-colors">
                      <div className="flex items-center space-x-3">
                        {getStatusIcon(test.status)}
                        <div>
                          <p className="font-semibold text-sm">{test.name}</p>
                          <p className="text-xs text-muted-foreground">{test.variant}</p>
                        </div>
                      </div>
                      <div className="text-right">
                        <div className="text-sm font-bold text-green-600">
                          +{test.uplift}%
                        </div>
                        <div className="text-xs text-muted-foreground">
                          {test.confidence}% confidence
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>

            {/* AI Recommendations */}
            <Card className="border-0 shadow-lg hover:shadow-xl transition-all duration-300">
              <CardHeader className="pb-4">
                <CardTitle className="flex items-center gap-3 text-xl">
                  <div className="p-2 bg-gradient-to-r from-purple-500/20 to-pink-500/20 rounded-lg">
                    <Zap className="h-5 w-5 text-purple-500" />
                  </div>
                  AI Recommendations
                </CardTitle>
                <CardDescription className="text-base">Optimization opportunities</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  {recommendations.map((rec) => (
                    <div key={rec.id} className="flex items-center justify-between p-4 border rounded-xl hover:bg-muted/30 transition-colors group">
                      <div className="flex-1">
                        <p className="font-semibold text-sm group-hover:text-primary transition-colors">{rec.title}</p>
                        <p className="text-xs text-muted-foreground">{rec.category}</p>
                      </div>
                      <div className="flex items-center space-x-3">
                        <Badge className={getImpactColor(rec.impact)}>
                          {rec.impact}
                        </Badge>
                        <span className="text-sm font-bold text-green-600">
                          {rec.expectedUplift}
                        </span>
                      </div>
                    </div>
                  ))}
                  <Button 
                    variant="outline" 
                    className="w-full mt-6 h-12 rounded-xl font-semibold hover:bg-primary hover:text-primary-foreground transition-all duration-300"
                    onClick={handleViewAllRecommendations}
                  >
                    View All Recommendations
                  </Button>
                </div>
              </CardContent>
            </Card>
          </div>
        </div>
      </main>
    </div>
  );
}
