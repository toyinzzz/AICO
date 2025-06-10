import { useState } from "react";
import { useToast } from "@/hooks/use-toast";
import { CompetitorInput } from "./CompetitorInput";
import { AnalysisProgress } from "./AnalysisProgress";
import { AnalysisResults } from "./AnalysisResults";
import { generateReport, downloadReport, generatePdfReport } from "@/utils/reportGenerator";

interface CompetitorSite {
  id: string;
  url: string;
  name: string;
  isYourSite?: boolean;
}

interface AuditMetrics {
  seoScore: number;
  pageSpeed: number;
  mobileScore: number;
  uxScore: number;
  contentScore: number;
  ctaCount: number;
  loadTime: number;
  conversionRate: number;
  trustSignals: number;
}

interface CompetitorData extends CompetitorSite {
  metrics: AuditMetrics;
  strengths: string[];
  weaknesses: string[];
  recommendations: string[];
}

export function CompetitorAnalysis() {
  const { toast } = useToast();
  const [competitorSites, setCompetitorSites] = useState<CompetitorSite[]>([
    { id: "your-site", url: "your-website.com", name: "Your Website", isYourSite: true }
  ]);
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [analysisProgress, setAnalysisProgress] = useState(0);
  const [analysisData, setAnalysisData] = useState<CompetitorData[]>([]);

  console.log("CompetitorAnalysis component rendered");
  console.log("Current competitor sites:", competitorSites);
  console.log("Analysis data:", analysisData);

  const generateMockDataForSite = (site: CompetitorSite): CompetitorData => {
    if (site.isYourSite) {
      return {
        ...site,
        metrics: {
          seoScore: 72,
          pageSpeed: 68,
          mobileScore: 85,
          uxScore: 78,
          contentScore: 80,
          ctaCount: 8,
          loadTime: 2.8,
          conversionRate: 2.4,
          trustSignals: 6
        },
        strengths: ["Strong mobile optimization", "Good content quality", "Effective trust signals"],
        weaknesses: ["Slow page speed", "Limited CTAs", "SEO optimization needed"],
        recommendations: ["Optimize images to improve page speed", "Add more strategic CTAs", "Improve meta tags and keywords"]
      };
    }

    // Generate varied mock data for competitors
    const baseMetrics = {
      seoScore: Math.floor(Math.random() * 40) + 60, // 60-100
      pageSpeed: Math.floor(Math.random() * 40) + 60, // 60-100
      mobileScore: Math.floor(Math.random() * 30) + 70, // 70-100
      uxScore: Math.floor(Math.random() * 30) + 70, // 70-100
      contentScore: Math.floor(Math.random() * 30) + 70, // 70-100
      ctaCount: Math.floor(Math.random() * 8) + 6, // 6-14
      loadTime: (Math.random() * 2 + 1.5).toFixed(1), // 1.5-3.5
      conversionRate: (Math.random() * 2 + 1.5).toFixed(1), // 1.5-3.5
      trustSignals: Math.floor(Math.random() * 6) + 5 // 5-11
    };

    return {
      ...site,
      metrics: {
        ...baseMetrics,
        loadTime: parseFloat(baseMetrics.loadTime),
        conversionRate: parseFloat(baseMetrics.conversionRate)
      },
      strengths: [
        "Excellent SEO optimization",
        "Fast loading speed", 
        "Multiple effective CTAs",
        "Outstanding mobile experience",
        "High-quality content"
      ].slice(0, 3),
      weaknesses: [
        "Mobile experience could improve",
        "Content engagement lower",
        "SEO needs improvement",
        "Fewer trust signals"
      ].slice(0, 2),
      recommendations: [
        "Learn from their SEO strategy",
        "Implement similar CTA placement", 
        "Study their page speed optimizations",
        "Match their mobile optimization",
        "Learn from their content strategy"
      ].slice(0, 3)
    };
  };

  const runAnalysis = async () => {
    console.log("Starting analysis...");
    
    if (competitorSites.length < 2) {
      toast({
        title: "Add Competitors",
        description: "Please add at least one competitor to analyze.",
        variant: "destructive",
      });
      return;
    }

    setIsAnalyzing(true);
    setAnalysisProgress(0);

    // Simulate analysis progress
    const progressInterval = setInterval(() => {
      setAnalysisProgress(prev => {
        if (prev >= 90) {
          clearInterval(progressInterval);
          return 90;
        }
        return prev + 15;
      });
    }, 500);

    // Simulate API call
    setTimeout(() => {
      clearInterval(progressInterval);
      setAnalysisProgress(100);
      setIsAnalyzing(false);
      
      // Generate mock data for all competitor sites
      const generatedData = competitorSites.map(site => generateMockDataForSite(site));
      setAnalysisData(generatedData);
      
      console.log("Analysis complete, setting data:", generatedData);
      
      toast({
        title: "Analysis Complete",
        description: `Successfully analyzed ${competitorSites.length} websites`,
      });
    }, 4000);
  };

  const exportReport = () => {
    console.log("Exporting PDF report...");
    
    if (analysisData.length === 0) {
      toast({
        title: "No Data Available",
        description: "Please run an analysis first before exporting a report.",
        variant: "destructive",
      });
      return;
    }

    try {
      generatePdfReport(analysisData, 'competitor');
      
      toast({
        title: "PDF Report Downloaded",
        description: "Comprehensive competitor analysis report saved as PDF",
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
    <div className="space-y-6">
      <CompetitorInput 
        competitorSites={competitorSites}
        onCompetitorSitesChange={setCompetitorSites}
      />
      
      <AnalysisProgress
        isAnalyzing={isAnalyzing}
        analysisProgress={analysisProgress}
        onRunAnalysis={runAnalysis}
        canAnalyze={competitorSites.length >= 2}
      />

      {analysisData.length > 0 && (
        <AnalysisResults 
          analysisData={analysisData}
          onExportReport={exportReport}
        />
      )}
    </div>
  );
}
