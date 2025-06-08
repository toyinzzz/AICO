
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Target, CheckCircle } from "lucide-react";

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

interface GapAnalysisProps {
  analysisData: CompetitorData[];
}

export function GapAnalysis({ analysisData }: GapAnalysisProps) {
  const getGapAnalysis = () => {
    if (analysisData.length < 2) return [];
    
    const yourSite = analysisData.find(d => d.isYourSite);
    const competitors = analysisData.filter(d => !d.isYourSite);
    
    if (!yourSite) return [];

    const gaps = [];
    
    competitors.forEach(competitor => {
      Object.entries(competitor.metrics).forEach(([metric, value]) => {
        const yourValue = yourSite.metrics[metric as keyof typeof yourSite.metrics];
        if (typeof value === 'number' && typeof yourValue === 'number' && value > yourValue) {
          const gap = value - yourValue;
          gaps.push({
            metric: metric.replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase()),
            competitor: competitor.name,
            gap: gap,
            opportunity: `Improve by ${gap.toFixed(1)} points to match ${competitor.name}`
          });
        }
      });
    });

    return gaps.sort((a, b) => b.gap - a.gap).slice(0, 5);
  };

  const gapAnalysis = getGapAnalysis();

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center space-x-2">
          <Target className="w-5 h-5 text-blue-500" />
          <span>Gap Analysis & Opportunities</span>
        </CardTitle>
        <CardDescription>
          Areas where competitors outperform you and actionable recommendations
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {gapAnalysis.map((gap, index) => (
            <div key={index} className="p-4 border rounded-lg">
              <div className="flex items-center justify-between mb-2">
                <h4 className="font-medium">{gap.metric}</h4>
                <Badge variant="outline" className="text-red-600">
                  -{gap.gap.toFixed(1)} points
                </Badge>
              </div>
              <p className="text-sm text-muted-foreground">{gap.opportunity}</p>
            </div>
          ))}
          {gapAnalysis.length === 0 && (
            <div className="text-center py-8">
              <CheckCircle className="w-12 h-12 mx-auto mb-4 text-green-500" />
              <h3 className="text-lg font-medium mb-2">You're Leading!</h3>
              <p className="text-muted-foreground">Your website performs better than competitors in all key metrics.</p>
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
