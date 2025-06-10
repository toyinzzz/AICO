
import { Button } from "@/components/ui/button";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Download, Share2 } from "lucide-react";
import { useToast } from "@/hooks/use-toast";
import { ComparisonChart } from "./ComparisonChart";
import { MetricsTable } from "./MetricsTable";
import { GapAnalysis } from "./GapAnalysis";
import { CompetitorInsights } from "./CompetitorInsights";

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

interface AnalysisResultsProps {
  analysisData: CompetitorData[];
  onExportReport: () => void;
}

export function AnalysisResults({ analysisData, onExportReport }: AnalysisResultsProps) {
  const { toast } = useToast();

  const handleShare = async () => {
    console.log("Sharing analysis results...");
    
    try {
      const shareUrl = window.location.href;
      await navigator.clipboard.writeText(shareUrl);
      
      toast({
        title: "Link Copied",
        description: "Analysis link has been copied to your clipboard",
      });
    } catch (error) {
      console.error("Failed to copy to clipboard:", error);
      
      toast({
        title: "Share Failed",
        description: "Unable to copy link. Please share the URL manually.",
        variant: "destructive",
      });
    }
  };

  return (
    <Tabs defaultValue="comparison" className="w-full">
      <div className="flex items-center justify-between mb-4">
        <TabsList className="grid w-full max-w-md grid-cols-3">
          <TabsTrigger value="comparison">Comparison</TabsTrigger>
          <TabsTrigger value="gaps">Gap Analysis</TabsTrigger>
          <TabsTrigger value="insights">Insights</TabsTrigger>
        </TabsList>
        <div className="flex space-x-2">
          <Button variant="outline" size="sm" onClick={onExportReport}>
            <Download className="w-4 h-4 mr-2" />
            Export Report
          </Button>
          <Button variant="outline" size="sm" onClick={handleShare}>
            <Share2 className="w-4 h-4 mr-2" />
            Share
          </Button>
        </div>
      </div>

      <TabsContent value="comparison" className="space-y-6">
        <ComparisonChart analysisData={analysisData} />
        <MetricsTable analysisData={analysisData} />
      </TabsContent>

      <TabsContent value="gaps" className="space-y-6">
        <GapAnalysis analysisData={analysisData} />
      </TabsContent>

      <TabsContent value="insights" className="space-y-6">
        <CompetitorInsights analysisData={analysisData} />
      </TabsContent>
    </Tabs>
  );
}
