
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart";
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, ResponsiveContainer } from "recharts";

interface CompetitorData {
  id: string;
  url: string;
  name: string;
  isYourSite?: boolean;
  metrics: {
    seoScore: number;
    pageSpeed: number;
    mobileScore: number;
    uxScore: number;
    contentScore: number;
    ctaCount: number;
    loadTime: number;
    conversionRate: number;
    trustSignals: number;
  };
  strengths: string[];
  weaknesses: string[];
  recommendations: string[];
}

interface ComparisonChartProps {
  analysisData: CompetitorData[];
}

const CHART_COLORS = ["#8b5cf6", "#06b6d4", "#10b981", "#f59e0b", "#ef4444"];

export function ComparisonChart({ analysisData }: ComparisonChartProps) {
  const prepareChartData = () => {
    if (analysisData.length === 0) return [];
    
    const metrics = ['seoScore', 'pageSpeed', 'mobileScore', 'uxScore', 'contentScore'];
    return metrics.map(metric => {
      const data: any = { metric: metric.replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase()) };
      analysisData.forEach((site, index) => {
        data[site.name] = site.metrics[metric as keyof typeof site.metrics];
      });
      return data;
    });
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Performance Comparison</CardTitle>
        <CardDescription>Key metrics comparison across all websites</CardDescription>
      </CardHeader>
      <CardContent>
        <ChartContainer
          config={{
            seoScore: { label: "SEO Score", color: CHART_COLORS[0] },
            pageSpeed: { label: "Page Speed", color: CHART_COLORS[1] },
            mobileScore: { label: "Mobile Score", color: CHART_COLORS[2] },
            uxScore: { label: "UX Score", color: CHART_COLORS[3] },
            contentScore: { label: "Content Score", color: CHART_COLORS[4] }
          }}
          className="h-80"
        >
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={prepareChartData()}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="metric" />
              <YAxis />
              <ChartTooltip content={<ChartTooltipContent />} />
              {analysisData.map((site, index) => (
                <Bar 
                  key={site.id} 
                  dataKey={site.name} 
                  fill={CHART_COLORS[index % CHART_COLORS.length]} 
                />
              ))}
            </BarChart>
          </ResponsiveContainer>
        </ChartContainer>
      </CardContent>
    </Card>
  );
}
