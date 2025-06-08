
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { 
  TrendingUp, 
  TrendingDown, 
  Target, 
  CheckCircle, 
  AlertTriangle, 
  ExternalLink 
} from "lucide-react";

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

interface CompetitorInsightsProps {
  analysisData: CompetitorData[];
}

export function CompetitorInsights({ analysisData }: CompetitorInsightsProps) {
  const competitors = analysisData.filter(d => !d.isYourSite);

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
      {competitors.map(competitor => (
        <Card key={competitor.id}>
          <CardHeader>
            <CardTitle className="flex items-center justify-between">
              <span>{competitor.name}</span>
              <Button variant="outline" size="sm">
                <ExternalLink className="w-4 h-4 mr-2" />
                Visit
              </Button>
            </CardTitle>
            <CardDescription>{competitor.url}</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div>
              <h4 className="font-medium text-green-600 mb-2 flex items-center">
                <TrendingUp className="w-4 h-4 mr-2" />
                Strengths
              </h4>
              <ul className="text-sm space-y-1">
                {competitor.strengths.map((strength, idx) => (
                  <li key={idx} className="flex items-start space-x-2">
                    <CheckCircle className="w-3 h-3 text-green-500 mt-1" />
                    <span>{strength}</span>
                  </li>
                ))}
              </ul>
            </div>
            <div>
              <h4 className="font-medium text-red-600 mb-2 flex items-center">
                <TrendingDown className="w-4 h-4 mr-2" />
                Weaknesses
              </h4>
              <ul className="text-sm space-y-1">
                {competitor.weaknesses.map((weakness, idx) => (
                  <li key={idx} className="flex items-start space-x-2">
                    <AlertTriangle className="w-3 h-3 text-red-500 mt-1" />
                    <span>{weakness}</span>
                  </li>
                ))}
              </ul>
            </div>
            <div>
              <h4 className="font-medium text-blue-600 mb-2 flex items-center">
                <Target className="w-4 h-4 mr-2" />
                Learn From Them
              </h4>
              <ul className="text-sm space-y-1">
                {competitor.recommendations.map((rec, idx) => (
                  <li key={idx} className="flex items-start space-x-2">
                    <Target className="w-3 h-3 text-blue-500 mt-1" />
                    <span>{rec}</span>
                  </li>
                ))}
              </ul>
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
